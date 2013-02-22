using System;
using System.IO;
using System.Net;
using CloudMorphConsole.Messages;
using CloudMorphPackagingTools;

namespace CloudMorphConsole.Handlers
{
    public class DeployJobHandler : Handles<DeployJobMessage>
    {
        public static string TempFolder = "TempPackages";

        public void Handle(DeployJobMessage message)
        {
            if (!File.Exists(message.PackagePath))
            {
                Console.WriteLine("Package doesn't exist");
                return;
            }

            using (var client = new WebClient())
            {
                client.UploadFile(message.ToUri, message.PackagePath);
            }
        }
    }
}