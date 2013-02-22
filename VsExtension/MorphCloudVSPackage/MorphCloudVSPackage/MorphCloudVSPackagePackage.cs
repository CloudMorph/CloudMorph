using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.ComponentModel.Design;
using System.Windows;
using BlueMetalArchitects.MorphCloudVSPackage.Extension;
using BlueMetalArchitects.MorphCloudVSPackage.IoC;
using EnvDTE;
using EnvDTE80;
using Microsoft.Win32;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Shell;

namespace BlueMetalArchitects.MorphCloudVSPackage
{
    /// <summary>
    /// This is the class that implements the package exposed by this assembly.
    ///
    /// The minimum requirement for a class to be considered a valid package for Visual Studio
    /// is to implement the IVsPackage interface and register itself with the shell.
    /// This package uses the helper classes defined inside the Managed Package Framework (MPF)
    /// to do it: it derives from the Package class that provides the implementation of the 
    /// IVsPackage interface and uses the registration attributes defined in the framework to 
    /// register itself and its components with the shell.
    /// </summary>
    // This attribute tells the PkgDef creation utility (CreatePkgDef.exe) that this class is
    // a package.
    [PackageRegistration(UseManagedResourcesOnly = true)]
    // This attribute is used to register the informations needed to show the this package
    // in the Help/About dialog of Visual Studio.
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)]
    // This attribute is needed to let the shell know that this package exposes some menus.
    [ProvideMenuResource("Menus.ctmenu", 1)]
    // This attribute registers a tool window exposed by this package.
    [ProvideToolWindow(typeof(MyToolWindow))]
    [Guid(GuidList.guidMorphCloudVSPackagePkgString)]
    public sealed class MorphCloudVSPackagePackage : Package, IVsSolutionEvents, IVsUpdateSolutionEvents
    {
        private uint shellPropertyChangesCookie;
        private uint solutionEventsCookie;
        private uint updateSolutionEventsCookie;

        private IVsShell vsShell = null;
        private IVsSolution2 solution = null;
        private IVsSolutionBuildManager2 sbm = null;
        private IVsSolutionBuildManager bm;
        private IVsHierarchy hier;
        private IVsMonitorSelection mon;
        private DTE _applicationObject;
        private DTE2 _dte;

        /// <summary>
        /// Default constructor of the package.
        /// Inside this method you can place any initialization code that does not require 
        /// any Visual Studio service because at this point the package object is created but 
        /// not sited yet inside Visual Studio environment. The place to do all the other 
        /// initialization is the Initialize method.
        /// </summary>
        public MorphCloudVSPackagePackage()
        {
            Trace.WriteLine(string.Format(CultureInfo.CurrentCulture, "Entering constructor for: {0}", this.ToString()));
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            // Unadvise all events
            if (vsShell != null && shellPropertyChangesCookie != 0)
                vsShell.UnadviseShellPropertyChanges(shellPropertyChangesCookie);

            if (sbm != null && updateSolutionEventsCookie != 0)
                sbm.UnadviseUpdateSolutionEvents(updateSolutionEventsCookie);

            if (solution != null && solutionEventsCookie != 0)
                solution.UnadviseSolutionEvents(solutionEventsCookie);
        }
        /////////////////////////////////////////////////////////////////////////////
        // Overriden Package Implementation
        #region Package Members

        /// <summary>
        /// Initialization of the package; this method is called right after the package is sited, so this is the place
        /// where you can put all the initilaization code that rely on services provided by VisualStudio.
        /// </summary>
        protected override void Initialize()
        {
            Trace.WriteLine (string.Format(CultureInfo.CurrentCulture, "Entering Initialize() of: {0}", this.ToString()));
            base.Initialize();

            // Add our command handlers for menu (commands must exist in the .vsct file)
            OleMenuCommandService mcs = GetService(typeof(IMenuCommandService)) as OleMenuCommandService;
            if ( null != mcs )
            {
                // Create the command for the menu item.
                CommandID menuCommandID = new CommandID(GuidList.guidMorphCloudVSPackageCmdSet, (int)PkgCmdIDList.cmdidMorphCloudExplorer);
                MenuCommand menuItem = new MenuCommand(MenuItemCallback, menuCommandID );
                mcs.AddCommand( menuItem );

                // Create the command for the tool window
                CommandID toolwndCommandID = new CommandID(GuidList.guidMorphCloudVSPackageCmdSet, (int)PkgCmdIDList.cmdidMorphCloudExplorerTool);
                MenuCommand menuToolWin = new MenuCommand(ShowToolWindow, toolwndCommandID);
                mcs.AddCommand( menuToolWin );

                // Create the command for the Project menu
                CommandID menuProjCommandID = new CommandID(GuidList.guidMorphCloudVSPackageCmdSet, (int)PkgCmdIDList.cmdidProjectDeploy);
                MenuCommand menuProjlWin = new MenuCommand(DeployToTheMorphCloud, menuProjCommandID);
                mcs.AddCommand(menuProjlWin);

                // Create the command for the Project menu
                CommandID menuProjCommandID2 = new CommandID(GuidList.guidMorphCloudVSPackageCmdSet, (int)PkgCmdIDList.cmdidProjectDeploy2);
                MenuCommand menuProjlWin2 = new MenuCommand(DeployToTheMorphCloud2, menuProjCommandID2);
                mcs.AddCommand(menuProjlWin2);
            }

            _dte = GetService(typeof(SDTE)) as DTE2;

            InitializePackage();
        }
        #endregion

        private List<object> _cache = new List<object>();

        #region Initialize Package
        private void InitializePackage()
        {
            // Get shell object
            vsShell = ServiceProvider.GlobalProvider.GetService(typeof(SVsShell)) as IVsShell;

            // Get solution
            solution = ServiceProvider.GlobalProvider.GetService(typeof(SVsSolution)) as IVsSolution2;
            if (solution != null)
            {
                // Get count of any currently loaded projects
                object count;
                solution.GetProperty((int)__VSPROPID.VSPROPID_ProjectCount, out count);
                var totalProjects = (int)count;

                // Register for solution events
                solution.AdviseSolutionEvents(this, out solutionEventsCookie);
            }

            // Get solution build manager
            sbm = ServiceProvider.GlobalProvider.GetService(typeof(SVsSolutionBuildManager)) as IVsSolutionBuildManager2;
            if (sbm != null)
            {
                sbm.AdviseUpdateSolutionEvents(this, out updateSolutionEventsCookie);
            }

/*
            // Get build manager
            bm = ServiceProvider.GlobalProvider.GetService(typeof (IVsSolutionBuildManager)) as IVsSolutionBuildManager;
            if (bm != null)
            {
                //bm.AdviseUpdateSolutionEvents(this, out someCookie);
            }
*/

/*
            mon = ServiceProvider.GlobalProvider.GetService(typeof (IVsMonitorSelection)) as IVsMonitorSelection;
            IVsMultiItemSelect mms;
            IntPtr ppHier, ppSc;
            uint ppmms = VSConstants.VSITEMID_SELECTION;
            mon.GetCurrentSelection(out ppHier, out ppmms, out mms, out ppSc);
            IVsHierarchy hierarchy = Marshal.GetTypedObjectForIUnknown(ppHier, typeof(IVsHierarchy)) as IVsHierarchy;
*/

            //hier = ServiceProvider.GlobalProvider.GetService(typeof(IVsHierarchy)) as IVsHierarchy;

            _applicationObject = (DTE)GetService(typeof(DTE));
//            string solutionDir = System.IO.Path.GetDirectoryName(_applicationObject.Solution.FullName);

/*
            var doc = _applicationObject.ActiveDocument;
            var projectItem = doc.ProjectItem;
            var project = projectItem.ContainingProject;
            var evalProject = Microsoft.Build.Evaluation.ProjectCollection.GlobalProjectCollection.LoadProject(project.FullName);
            var execProject = evalProject.CreateProjectInstance();
*/

            


        }

        private void WriteToOutputWindow(string text)
        {
            //Insert this section------------------
            var outputWindow = GetService(typeof(SVsOutputWindow)) as IVsOutputWindow;
            IVsOutputWindowPane pane;
            Guid guidGeneralPane = VSConstants.GUID_OutWindowGeneralPane;
            outputWindow.GetPane(ref guidGeneralPane, out pane);
            if (pane != null)
            {
                pane.OutputString(string.Format(
                    "To Do item created: {0}\r\n",
                    text));
            }
            //-------------------------------------        
        }

        #endregion

        /// <summary>
        /// This function is the callback used to execute a command when the a menu item is clicked.
        /// See the Initialize method to see how the menu item is associated to this function using
        /// the OleMenuCommandService service and the MenuCommand class.
        /// </summary>
        private void MenuItemCallback(object sender, EventArgs e)
        {
            // Show a Message Box to prove we were here
/*
            IVsUIShell uiShell = (IVsUIShell)GetService(typeof(SVsUIShell));
            Guid clsid = Guid.Empty;
            int result;
            Microsoft.VisualStudio.ErrorHandler.ThrowOnFailure(uiShell.ShowMessageBox(
                       0,
                       ref clsid,
                       "MorphCloudVSPackage",
                       string.Format(CultureInfo.CurrentCulture, "Inside {0}.MenuItemCallback()", this.ToString()),
                       string.Empty,
                       0,
                       OLEMSGBUTTON.OLEMSGBUTTON_OK,
                       OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST,
                       OLEMSGICON.OLEMSGICON_INFO,
                       0,        // false
                       out result));
*/
        }

        /// <summary>
        /// This function is called when the user clicks the menu item that shows the 
        /// tool window. See the Initialize method to see how the menu item is associated to 
        /// this function using the OleMenuCommandService service and the MenuCommand class.
        /// </summary>
        private void ShowToolWindow(object sender, EventArgs e)
        {
            // Get the instance number 0 of this tool window. This window is single instance so this instance
            // is actually the only one.
            // The last flag is set to true so that if the tool window does not exists it will be created.
            ToolWindowPane window = this.FindToolWindow(typeof(MyToolWindow), 0, true);
            if ((null == window) || (null == window.Frame))
            {
                throw new NotSupportedException(Resources.CanNotCreateWindow);
            }
            IVsWindowFrame windowFrame = (IVsWindowFrame)window.Frame;
            Microsoft.VisualStudio.ErrorHandler.ThrowOnFailure(windowFrame.Show());
        }


        /// <summary>
        /// This function is called when the user clicks the menu item that shows the 
        /// tool window. See the Initialize method to see how the menu item is associated to 
        /// this function using the OleMenuCommandService service and the MenuCommand class.
        /// </summary>
        private void DeployToTheMorphCloud(object sender, EventArgs e)
        {
            MessageBox.Show("Hello");

/* TODO: prevent the changes in the project while modifying it
            Project project = null;
            var bb = ToVsProjectBuildSystem(project);
            bb.StartBatchEdit();
*/

            DeployAction();

/*
            bb.EndBatchEdit();
*/
            //BuildSelectedProjectVsPackage();
        }

        private void DeployToTheMorphCloud2(object sender, EventArgs e)
        {
            MessageBox.Show("Hello 2");

            BuildSelectedProjectVsAddIn();
        }

        private void DeployAction()
        {
            try
            {
                //bool buildSuccedded = BuildActiveProject();
                bool buildSuccedded = BuildSolution();
                if (!buildSuccedded) throw new Exception("Could not build the solution.");

                Project startUpProject = GetStartUpProject();
                if (startUpProject == null) throw new Exception("Could not locate a startUp project.");

                string assemblyPath = GetOutputAssemblyPath(startUpProject);
                if (string.IsNullOrEmpty(assemblyPath)) throw new Exception("Could not locate the output assembly.");
                if (Path.GetExtension(assemblyPath) != ".exe") throw new Exception("The output of the startUp project is not an executable.");
                if (!File.Exists(assemblyPath)) throw new Exception("The output executable file could not be found.");
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Profiler initialization failed.", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

/*
        private void ShowWindow()
        {
            _window = this.FindToolWindow(typeof(VisualProfilerToolWindow), 0, true) as VisualProfilerToolWindow;
            if ((null == _window) || (null == _window.Frame)) throw new NotSupportedException("Cannot create a window.");

            UILogic uiLogic = _window.VisualProfilerUIView.UILogic;
            _window.VisualProfilerUIView.UILogic.MethodClick += mvm => OnMethodClick(uiLogic, mvm);
            _window.VisualProfilerUIView.Profile(profilerType, assemblyPath);
            _window.VisualProfilerUIView.DataUpdate += ContainingUnitView.UpdateDataOfContainingUnits;
            _window.Caption = string.Format("Visual Profiler - {0} Mode", GetModeString(profilerType));
            IVsWindowFrame windowFrame = (IVsWindowFrame)_window.Frame;
            ErrorHandler.ThrowOnFailure(windowFrame.Show());
        }
*/

        private bool BuildSolution()
        {
            _dte.Solution.SolutionBuild.Build(true);
            int numberOfFailures = _dte.Solution.SolutionBuild.LastBuildInfo;
            bool buildSucceeded = numberOfFailures == 0;
            return buildSucceeded;
        }

        private bool BuildActiveProject()
        {
            //var _solutionManager = ServiceLocator.GetInstance<ISolutionManager>();

            /*var projects = _dte.ActiveSolutionProjects as System.Array;
            if (projects.Length > 0)
            {
                var proj = projects.GetValue(0) as Project;
                proj.
            }*/

            var _vsMonitorSelection = (IVsMonitorSelection)GetService(typeof(IVsMonitorSelection));
            var activeProject = _vsMonitorSelection.GetActiveProject();

            var buildSystem = ToVsProjectBuildSystem(activeProject);
            bool success;
            buildSystem.BuildTarget("CalledFromIde", out success);

            return success;
        }

        private Project GetStartUpProject()
        {
            Array startupProjects = _dte.Solution.SolutionBuild.StartupProjects as Array;
            string value = startupProjects.GetValue(0) as string;

            Project startUpProject = null;
            foreach (var project in _dte.Solution.Projects)
            {
                var project1 = project as Project;
                if (project1.FullName.EndsWith(value))
                {
                    startUpProject = project1;
                }
            }

            return startUpProject;
        }

        private string GetOutputAssemblyPath(EnvDTE.Project vsProject)
        {
            string fullPath = vsProject.Properties.Item("FullPath").Value.ToString();
            string outputPath = vsProject.ConfigurationManager.ActiveConfiguration.Properties.Item("OutputPath").Value.ToString();
            string outputDir = Path.Combine(fullPath, outputPath);
            string outputFileName = vsProject.Properties.Item("OutputFileName").Value.ToString();
            string assemblyPath = Path.Combine(outputDir, outputFileName);
            return assemblyPath;
        }

        private void BuildSelectedProjectVsAddIn()
        {
            var projects = _applicationObject.ActiveSolutionProjects as System.Array;
            if (projects.Length > 0)
            {
                var proj = projects.GetValue(0) as Project;

                var evalProject = Microsoft.Build.Evaluation.ProjectCollection.GlobalProjectCollection.LoadProject(proj.FullName);
                var execProject = evalProject.CreateProjectInstance();
                //bool success = execProject.Build("CalledFromIde", null);
                execProject.Build("Build", null);
            }

        }

        private void BuildSelectedProjectVsPackage()
        {
            // Retrieve shell interface in order to get current selection
            IVsMonitorSelection monitorSelection = Package.GetGlobalService(typeof(SVsShellMonitorSelection)) as IVsMonitorSelection;
            if (monitorSelection == null)
                throw new InvalidOperationException();

            IVsHierarchy hierarchy = null;
            IntPtr hierarchyPtr = IntPtr.Zero;
            IntPtr selectionContainer = IntPtr.Zero;
            uint itemid;

            try
            {
                // Get the current project hierarchy, project item, and selection container for the current selection
                // If the selection spans multiple hierachies, hierarchyPtr is Zero
                IVsMultiItemSelect multiItemSelect = null;
                ErrorHandler.ThrowOnFailure(monitorSelection.GetCurrentSelection(out hierarchyPtr, out itemid, out multiItemSelect, out selectionContainer));

                // We only care if there is only one node selected in the tree
                if (!(itemid == VSConstants.VSITEMID_NIL || hierarchyPtr == IntPtr.Zero || multiItemSelect != null || itemid == VSConstants.VSITEMID_SELECTION))
                {
                    hierarchy = Marshal.GetObjectForIUnknown(hierarchyPtr) as IVsHierarchy;

/*
                    IVsBuildPropertyStorage buildPropertyStorage = hierarchy as IVsBuildPropertyStorage;
                    if (buildPropertyStorage != null)
                    {
                        buildPropertyStorage.SetItemAttribute(itemid, "Author", "Igor");
                    }
*/

                }
            }
            finally
            {
                if (hierarchyPtr != IntPtr.Zero)
                    Marshal.Release(hierarchyPtr);
                if (selectionContainer != IntPtr.Zero)
                    Marshal.Release(selectionContainer);
            }

/*
            IVsSolutionBuildManager buildManager = (IVsSolutionBuildManager)GetService(typeof(SVsSolutionBuildManager));
            _cache.Add(buildManager);

            IVsProjectCfg[] ppIVsProjectCfg = new IVsProjectCfg[1];
            buildManager.FindActiveProjectCfg(IntPtr.Zero, IntPtr.Zero, hierarchy, ppIVsProjectCfg);

            IVsBuildableProjectCfg ppIVsBuildableProjectCfg;
            ppIVsProjectCfg[0].get_BuildableProjectCfg(out ppIVsBuildableProjectCfg);
            _cache.Add(ppIVsBuildableProjectCfg);

            var outputWindow = GetService(typeof(SVsOutputWindow)) as IVsOutputWindow;
            _cache.Add(outputWindow);
            IVsOutputWindowPane pane;
            Guid guidGeneralPane = VSConstants.GUID_OutWindowGeneralPane;
            outputWindow.GetPane(ref guidGeneralPane, out pane);
            _cache.Add(pane);
            ppIVsBuildableProjectCfg.StartBuild(pane, VSConstants.VS_BUILDABLEPROJECTCFGOPTS_REBUILD);
*/

            IVsProjectBuildSystem projectBuildSystem = null;
            bool bSuccess = false;
            projectBuildSystem.BuildTarget("Debug", out bSuccess);

            WriteToOutputWindow("Done");

        }

        public static IVsProjectBuildSystem ToVsProjectBuildSystem(Project project)
        {
            if (project == null)
            {
                throw new ArgumentNullException("project");
            }
            // Convert the project to an IVsHierarchy and see if it implements IVsProjectBuildSystem
            return project.ToVsHierarchy() as IVsProjectBuildSystem;
        }

        #region Implementation of IVsSolutionEvents

        public int OnAfterOpenProject(IVsHierarchy pHierarchy, int fAdded)
        {
            return VSConstants.S_OK;
        }

        public int OnQueryCloseProject(IVsHierarchy pHierarchy, int fRemoving, ref int pfCancel)
        {
            return VSConstants.S_OK;
        }

        public int OnBeforeCloseProject(IVsHierarchy pHierarchy, int fRemoved)
        {
            return VSConstants.S_OK;
        }

        public int OnAfterLoadProject(IVsHierarchy pStubHierarchy, IVsHierarchy pRealHierarchy)
        {
            return VSConstants.S_OK;
        }

        public int OnQueryUnloadProject(IVsHierarchy pRealHierarchy, ref int pfCancel)
        {
            return VSConstants.S_OK;
        }

        public int OnBeforeUnloadProject(IVsHierarchy pRealHierarchy, IVsHierarchy pStubHierarchy)
        {
            return VSConstants.S_OK;
        }

        public int OnAfterOpenSolution(object pUnkReserved, int fNewSolution)
        {
            return VSConstants.S_OK;
        }

        public int OnQueryCloseSolution(object pUnkReserved, ref int pfCancel)
        {
            return VSConstants.S_OK;
        }

        public int OnBeforeCloseSolution(object pUnkReserved)
        {
            return VSConstants.S_OK;
        }

        public int OnAfterCloseSolution(object pUnkReserved)
        {
            return VSConstants.S_OK;
        }

        #endregion

        #region Implementation of IVsUpdateSolutionEvents

        public int UpdateSolution_Begin(ref int pfCancelUpdate)
        {
            return VSConstants.S_OK;
        }

        public int UpdateSolution_Done(int fSucceeded, int fModified, int fCancelCommand)
        {
            return VSConstants.S_OK;
        }

        public int UpdateSolution_StartUpdate(ref int pfCancelUpdate)
        {
            return VSConstants.S_OK;
        }

        public int UpdateSolution_Cancel()
        {
            return VSConstants.S_OK;
        }

        public int OnActiveProjectCfgChange(IVsHierarchy pIVsHierarchy)
        {
            return VSConstants.S_OK;
        }

        #endregion
    }
}
