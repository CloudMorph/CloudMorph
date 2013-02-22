using System;
using System.Collections.Generic;
using System.IO;
using CloudMorphConsole.Messages;
using CloudMorphPackagingTools;

namespace CloudMorphConsole.Handlers
{
    public class PackageMetadataHandler : Handles<FileMapMessage>
    {
        private string _packageName;

        public void Handle(FileMapMessage message)
        {
            _packageName = message.PackageName;

            Map = PackageCompressor.GetFilesMap(message.SourcePath);
        }

        private IEnumerable<PackageCompressor.FileMapInfo> _map;
        public IEnumerable<PackageCompressor.FileMapInfo> Map
        {
            get
            {
                foreach (var fileMapInfo in _map)
                {
                    if (Path.GetFileName(fileMapInfo.FullPath) == "package.config")
                        fileMapInfo.RenameName = _packageName + ".config";

                    yield return fileMapInfo;
                }
            }
            internal set { _map = value; }
        }
    }
}