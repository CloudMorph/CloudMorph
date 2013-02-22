using System;
using System.Collections.Generic;
using System.IO;
using Ionic.Zip;

namespace CloudMorphPackagingTools
{
    public static class PackageCompressor
    {
        public static void Compress(string sourcePath, string targetpath)
        {
            GetFilesMap(sourcePath).Compress(targetpath);
        }

        public static void CompressFiles(IEnumerable<string> files, string rootPath, string targetpath)
        {
            files.GetExistingFiles(rootPath).Compress(targetpath);
        }

        private static IEnumerable<FileMapInfo> GetExistingFiles(this IEnumerable<string> files, string rootPath)
        {
            var rootUri = new Uri(rootPath);

            foreach (var file in files)
            {
                if (File.Exists(file))
                {
                    var fileUri = new Uri(file);
                    var relativePath = rootUri.MakeRelativeUri(fileUri).ToString();

                    if (! string.IsNullOrEmpty(relativePath))
                        relativePath = relativePath.Replace('/', Path.DirectorySeparatorChar);

                    yield return new FileMapInfo()
                                     {
                                         FullPath = file,
                                         RelativePath = relativePath
                                     };
                    
                }
            }
        }

        public static void Compress(this IEnumerable<FileMapInfo> files, string targetpath)
        {
            using (var zip = new ZipFile())
            {
                zip.AlternateEncodingUsage = ZipOption.AsNecessary;
                zip.CompressionLevel = Ionic.Zlib.CompressionLevel.BestCompression;
                zip.Comment = "This zip was created at " + System.DateTime.Now.ToString("G");

                foreach (var file in files)
                {
                    // add this map file into the "images" directory in the zip archive
                    // zip.AddFile("c:\\images\\personal\\7440-N49th.png", "images");

                    var entry = zip.AddFile(file.FullPath, file.RelativePath);
                    if (!string.IsNullOrEmpty(file.RenameName))
                        entry.FileName = file.RenameName;
                }

                zip.Save(targetpath);
            }
        }

        public static void AddPackageContents(string sourcePackage, string additionalPackage, string uniquPackageName1)
        {
            using (var zip = new ZipFile(sourcePackage))
            {
                using (var zip2 = new ZipFile(additionalPackage))
                {
                    foreach (var entry in zip2)
                    {
                        try
                        {
                            if (! zip.ContainsEntry(entry.FileName))
                            {
                                var entry1 = zip.AddEntry(entry.FileName, entry.OpenReader());
                                if (entry.FileName == "package.config")
                                    entry1.FileName = uniquPackageName1 + ".config";
                                zip.Save();
                            }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                        }
                    }
                    //zip.Save();
                }
            }
        }

        public static IEnumerable<FileMapInfo> GetFilesMap(string sourcePath)
        {
            string sourceDir = sourcePath;

            if (File.Exists(sourcePath))
                sourceDir = Path.GetDirectoryName(sourcePath);

            var dirs = new Queue<FileMapInfo>();
            dirs.Enqueue(new FileMapInfo() { FullPath = sourceDir, RelativePath = "/"});

            while (dirs.Count > 0)
            {
                var currentDir = dirs.Dequeue();
                
                foreach (var dir in Directory.GetDirectories(currentDir.FullPath))
                {
                    string folderName = Path.GetFileName(dir);
                    dirs.Enqueue(new FileMapInfo() { FullPath = dir, RelativePath = Path.Combine(currentDir.RelativePath, folderName)});
                }

                foreach (var file in Directory.GetFiles(currentDir.FullPath))
                {
                    string ext = Path.GetExtension(file);
                    if (!string.IsNullOrEmpty(ext) &&  (ext.ToLower() != ".pdb"))
                    {
                        yield return new FileMapInfo() {FullPath = file, RelativePath = currentDir.RelativePath};
                    }
                }
            }
        }

        public static void Decompress(string sourcePath, string targetpath)
        {
            using (ZipFile zip1 = ZipFile.Read(sourcePath))
            {
                zip1.ExtractExistingFile = ExtractExistingFileAction.OverwriteSilently;
                zip1.ExtractAll(targetpath);

/*
                // here, we extract every entry, but we could extract conditionally
                // based on entry name, size, date, checkbox status, etc.  );
                foreach (ZipEntry e in zip1)
                {
                    e.Extract(unpackDirectory, ExtractExistingFileAction.OverwriteSilently);
                }
*/
            }
        }


        public class FileMapInfo
        {
            public string FullPath { get; set; }
            public string RelativePath { get; set; }
            public string RenameName { get; set; }
        }
    }
}