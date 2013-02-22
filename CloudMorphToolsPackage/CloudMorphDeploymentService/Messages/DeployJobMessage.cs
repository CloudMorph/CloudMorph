namespace CloudMorphDeploymentService.Messages
{
    public class DeployJobMessage : Message
    {
        public const string CommandText = "launch-job";
        public string Command;
        public string PackageName;
    }
}