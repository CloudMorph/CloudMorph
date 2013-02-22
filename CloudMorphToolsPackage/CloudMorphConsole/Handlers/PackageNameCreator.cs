using System;
using System.IO;

namespace CloudMorphConsole.Handlers
{
    public class PackageNameCreator
    {
        public string GetUniquePackageName(string packageName)
        {
            string name = Path.GetFileNameWithoutExtension(packageName);

            return name + "_" + Guid.NewGuid();
        }
    }
}