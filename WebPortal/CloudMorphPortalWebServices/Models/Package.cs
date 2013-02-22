using System;

namespace CloudMorphPortalWebServices.Models
{
    public class Package
    {
        public Guid Guid { get; set; }
        public string Name { get; set; }
        public long Size { get; set; }
    }
}