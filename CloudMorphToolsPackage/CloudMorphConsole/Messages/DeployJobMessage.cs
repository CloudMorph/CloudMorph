using System.IO;

namespace CloudMorphConsole.Messages
{
    public class DeployJobMessage
    {
        public string PackagePath { get; set; }
        public string ToUri { get; set; }

        
    }
}