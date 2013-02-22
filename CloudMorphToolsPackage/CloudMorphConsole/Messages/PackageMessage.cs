using System.Collections.Generic;
using CloudMorphPackagingTools;

namespace CloudMorphConsole.Messages
{
    public class PackageMessage
    {
        public IEnumerable<PackageCompressor.FileMapInfo> SourceMap { get; set; }
        public string PackageFolderDestination { get; set; }
        public string PackageName { get; set; }
        public string UniquePackageName { get; set; }
    }
}