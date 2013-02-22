using System.Runtime.InteropServices;
using BlueMetalArchitects.MorphCloudVSPackage.IoC;
using EnvDTE;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;

namespace BlueMetalArchitects.MorphCloudVSPackage.Extension
{
    // https://hg01.codeplex.com/nuget/file/a99ed78b0382/src/VisualStudio/ProjectExtensions.cs
    public static class ProjectExtensions
    {
        public static IVsHierarchy ToVsHierarchy(this Project project) {
         IVsHierarchy hierarchy;

         // Get the vs solution
         IVsSolution solution = ServiceLocator.GetInstance<IVsSolution>();
         int hr = solution.GetProjectOfUniqueName(project.UniqueName, out hierarchy);

         if (hr != VSConstants.S_OK) {
             Marshal.ThrowExceptionForHR(hr);
         }

         return hierarchy;
     }
    }
}