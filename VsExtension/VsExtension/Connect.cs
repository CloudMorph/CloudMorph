using System;
using System.Windows.Forms;
using Extensibility;
using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.CommandBars;
using System.Resources;
using System.Reflection;
using System.Globalization;
using VSLangProj;
using VSLangProj80;

namespace VsExtension
{
	/// <summary>The object for implementing an Add-in.</summary>
	/// <seealso class='IDTExtensibility2' />
	public class Connect : IDTExtensibility2, IDTCommandTarget
	{
		/// <summary>Implements the constructor for the Add-in object. Place your initialization code within this method.</summary>
		public Connect()
		{
		}

		/// <summary>Implements the OnConnection method of the IDTExtensibility2 interface. Receives notification that the Add-in is being loaded.</summary>
		/// <param term='application'>Root object of the host application.</param>
		/// <param term='connectMode'>Describes how the Add-in is being loaded.</param>
		/// <param term='addInInst'>Object representing this Add-in.</param>
		/// <seealso class='IDTExtensibility2' />
		public void OnConnection(object application, ext_ConnectMode connectMode, object addInInst, ref Array custom)
		{
			_applicationObject = (DTE2)application;
			_addInInstance = (AddIn)addInInst;
            //BuildManagerTest((DTE2)_applicationObject);

			if(connectMode == ext_ConnectMode.ext_cm_UISetup)
			{
				object []contextGUIDS = new object[] { };
				Commands2 commands = (Commands2)_applicationObject.Commands;
				string toolsMenuName = "Tools";

				//Place the command on the tools menu.
				//Find the MenuBar command bar, which is the top-level command bar holding all the main menu items:
				var menuBarCommandBar = ((Microsoft.VisualStudio.CommandBars.CommandBars)_applicationObject.CommandBars)["MenuBar"];

/*
				//Find the Tools command bar on the MenuBar command bar:
				CommandBarControl toolsControl = menuBarCommandBar.Controls[toolsMenuName];
				CommandBarPopup toolsPopup = (CommandBarPopup)toolsControl;

				//This try/catch block can be duplicated if you wish to add multiple commands to be handled by your Add-in,
				//  just make sure you also update the QueryStatus/Exec method to include the new command names.
				try
				{
					//Add a command to the Commands collection:
					Command command = commands.AddNamedCommand2(_addInInstance, "VsExtension", "VsExtension", "Executes the command for VsExtension", true, 59, ref contextGUIDS, 
									(int)vsCommandStatus.vsCommandStatusSupported+(int)vsCommandStatus.vsCommandStatusEnabled, 
									(int)vsCommandStyle.vsCommandStylePictAndText,
									vsCommandControlType.vsCommandControlTypeButton);

					//Add a control for the command to the tools menu:
					if((command != null) && (toolsPopup != null))
					{
						command.AddControl(toolsPopup.CommandBar, 1);
					}
				}
				catch(System.ArgumentException)
				{
					//If we are here, then the exception is probably because a command with that name
					//  already exists. If so there is no need to recreate the command and we can 
					//  safely ignore the exception.
				}
*/

                var projectPopup = ((Microsoft.VisualStudio.CommandBars.CommandBars)_applicationObject.CommandBars)["Project"];

                //CommandBarControl projectControl = menuBarCommandBar.Controls["Project"];
                //CommandBarPopup projectPopup = (CommandBarPopup)projectControl;

				try
				{
					Command command = //commands.AddNamedCommand2(_addInInstance, "MyAddin1", "My Addin...", "Executes the command for My Addin", true, 59, ref contextGUIDS,
                                      commands.AddNamedCommand2(_addInInstance, "ProjectDo", "ProjectDo", "Executes the command for VsExtension", false, 1, ref contextGUIDS,
									  (int)vsCommandStatus.vsCommandStatusSupported + (int)vsCommandStatus.vsCommandStatusEnabled,
									  (int)vsCommandStyle.vsCommandStylePictAndText,
									  vsCommandControlType.vsCommandControlTypeButton);
                    if ((command != null) && (projectPopup != null))
					{
                        //var ctrl = (CommandBarControl)command.AddControl(projectPopup, 1);
                        var ctrl = (CommandBarControl)command.AddControl(projectPopup, 1);
						ctrl.TooltipText = "Executes the command for MyAddin";
                        //command.AddControl(projectPopup.CommandBar, 1);
					}
				}
				catch (System.ArgumentException)
				{
				}
			}
		}


/*
        AddCommandToContextMenu(
               WorkItemTrackingMenus.QueryBuilderContextMenu, // context menu Name
               "ClearQuery", // menu reference name
               "Clear", // display name
               47, // command icon
               1)    // command placement, 1= first item on top
*/
        /// <summary>
        /// Adds a command to VS context menu
        /// </summary>
        /// <param name="menuName">The name of the menu</param>
        /// <param name="commandName">Reference name of the command</param>
        /// <param name="commandText">Display name of the command</param>
        /// <param name="iconId">MSO icon id of the command</param>
        /// <param name="position">position of command 1 = first item in the menu</param>
        private void AddCommandToContextMenu(string menuName, string commandName, string commandText, int iconId, int position)
        {
            CommandBar contextMenu = ((CommandBars)_applicationObject.CommandBars)[menuName];
            AddCommand(contextMenu, commandName, commandText, iconId, position);
        }

        private void AddCommand(CommandBar parent, string commandName, string commandText, int iconId, int position)
        {
            Commands2 commands = (Commands2)_applicationObject.Commands;
            //create the command
            Command newCommand = commands.AddNamedCommand2(_addInInstance, commandName, commandText, commandText, true, iconId);
            // add it to parent menu
            newCommand.AddControl(parent, position);
        }

		/// <summary>Implements the OnDisconnection method of the IDTExtensibility2 interface. Receives notification that the Add-in is being unloaded.</summary>
		/// <param term='disconnectMode'>Describes how the Add-in is being unloaded.</param>
		/// <param term='custom'>Array of parameters that are host application specific.</param>
		/// <seealso class='IDTExtensibility2' />
		public void OnDisconnection(ext_DisconnectMode disconnectMode, ref Array custom)
		{
		}

		/// <summary>Implements the OnAddInsUpdate method of the IDTExtensibility2 interface. Receives notification when the collection of Add-ins has changed.</summary>
		/// <param term='custom'>Array of parameters that are host application specific.</param>
		/// <seealso class='IDTExtensibility2' />		
		public void OnAddInsUpdate(ref Array custom)
		{
		}

		/// <summary>Implements the OnStartupComplete method of the IDTExtensibility2 interface. Receives notification that the host application has completed loading.</summary>
		/// <param term='custom'>Array of parameters that are host application specific.</param>
		/// <seealso class='IDTExtensibility2' />
		public void OnStartupComplete(ref Array custom)
		{
		}

		/// <summary>Implements the OnBeginShutdown method of the IDTExtensibility2 interface. Receives notification that the host application is being unloaded.</summary>
		/// <param term='custom'>Array of parameters that are host application specific.</param>
		/// <seealso class='IDTExtensibility2' />
		public void OnBeginShutdown(ref Array custom)
		{
		}
		
		/// <summary>Implements the QueryStatus method of the IDTCommandTarget interface. This is called when the command's availability is updated</summary>
		/// <param term='commandName'>The name of the command to determine state for.</param>
		/// <param term='neededText'>Text that is needed for the command.</param>
		/// <param term='status'>The state of the command in the user interface.</param>
		/// <param term='commandText'>Text requested by the neededText parameter.</param>
		/// <seealso class='Exec' />
		public void QueryStatus(string commandName, vsCommandStatusTextWanted neededText, ref vsCommandStatus status, ref object commandText)
		{
			if(neededText == vsCommandStatusTextWanted.vsCommandStatusTextWantedNone)
			{
			    switch (commandName)
			    {
			        case "VsExtension.Connect.VsExtension":
					    status = (vsCommandStatus)vsCommandStatus.vsCommandStatusSupported|vsCommandStatus.vsCommandStatusEnabled;
					    return;
                    case "VsExtension.Connect.ProjectDo":
                        status = (vsCommandStatus)vsCommandStatus.vsCommandStatusSupported | vsCommandStatus.vsCommandStatusEnabled;
                        return;
                }
			}
		}

		/// <summary>Implements the Exec method of the IDTCommandTarget interface. This is called when the command is invoked.</summary>
		/// <param term='commandName'>The name of the command to execute.</param>
		/// <param term='executeOption'>Describes how the command should be run.</param>
		/// <param term='varIn'>Parameters passed from the caller to the command handler.</param>
		/// <param term='varOut'>Parameters passed from the command handler to the caller.</param>
		/// <param term='handled'>Informs the caller if the command was handled or not.</param>
		/// <seealso class='Exec' />
		public void Exec(string commandName, vsCommandExecOption executeOption, ref object varIn, ref object varOut, ref bool handled)
		{
			handled = false;
			if(executeOption == vsCommandExecOption.vsCommandExecOptionDoDefault)
			{
                if (commandName == "VsExtension.Connect.ProjectDo")
				{
                   
/*
                    var doc = _applicationObject.ActiveDocument;
                    var projectItem = doc.ProjectItem;
                    var project = projectItem.ContainingProject;
                    var evalProject = Microsoft.Build.Evaluation.ProjectCollection.GlobalProjectCollection.LoadProject(project.FullName);
                    var execProject = evalProject.CreateProjectInstance();

                    bool success = execProject.Build("CalledFromIde", null);

                    var window = _applicationObject.Windows.Item(Constants.vsWindowKindOutput);
                    var output = (OutputWindow)window.Object;
                    OutputWindowPane pane = output.OutputWindowPanes.Add("BuildAddin");
                    pane.OutputString(success ? "built /t:CalledFromIde" : "build failed");
*/

				    var projects = _applicationObject.ActiveSolutionProjects as System.Array;
                    if (projects.Length > 0)
                    {
                        var proj = projects.GetValue(0) as Project;
                    }

					handled = true;
					return;
				}
			}
		}

        public void BuildManagerTest(DTE2 dte)
        {
            Project aProject = null;
            VSProject2 aVSProject = null;
            aProject = _applicationObject.Solution.Projects.Item(1);
            MessageBox.Show(@"Project kind is: " + aProject.Kind + @"\n" + @"Project name is: " + aProject.Name);
            aVSProject = ((VSProject2)( _applicationObject.Solution.Projects.Item(1).Object));
            MessageBox.Show(@"The full name of the project is:" + @"\n" + aVSProject.Project.FullName);
            MessageBox.Show(@"The BuildManager's containing project is: "+ aVSProject.BuildManager.ContainingProject.Name);
            MessageBox.Show(@"The Buildmanager's design time output monikers type is:" + @"\n" + aVSProject.BuildManager.DesignTimeOutputMonikers.GetType().ToString());

        }

        void FindReference()
        {
            foreach (Project project in (object[])_applicationObject.ActiveSolutionProjects)
            {
                VSProject vsProject = project.Object as VSProject;
                if (vsProject != null)
                {
                    foreach (Reference reference in vsProject.References)
                    {
                        // Do cool stuff here
                    }
                }
            }
        }

		private DTE2 _applicationObject;
		private AddIn _addInInstance;
	}
}