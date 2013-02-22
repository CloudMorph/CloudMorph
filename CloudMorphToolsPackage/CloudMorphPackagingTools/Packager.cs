using System;
using System.IO;

namespace CloudMorphPackagingTools
{
    public class Packager
    {
        public string PackageForDeployment(string startAssembly)
        {
            if (! File.Exists(startAssembly))
                throw new InvalidOperationException("The target file path is not valid");

            string targetPath = Path.Combine(Path.GetDirectoryName(startAssembly),
                                             "..\\" + Path.GetFileNameWithoutExtension(startAssembly) + ".zip");

            PackageCompressor.Compress(startAssembly, targetPath);

            return targetPath;
        }

        public string Unpack(string sourcePath)
        {
            string sourceDir = Path.GetDirectoryName(sourcePath);
            string targetDir = Path.GetFileNameWithoutExtension(sourcePath);
            string targetPath = Path.Combine(sourceDir, targetDir);

            PackageCompressor.Decompress(sourcePath, targetPath);

            return targetPath;
        }
    }
}