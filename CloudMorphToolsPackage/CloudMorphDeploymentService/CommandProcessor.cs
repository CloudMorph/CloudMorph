using System;
using CloudMorphDeploymentService.MessageHandlers;
using CloudMorphDeploymentService.Messages;
using log4net;
using Newtonsoft.Json.Linq;
using Topshelf.Model;

namespace CloudMorphDeploymentService
{
    public class CommandProcessor
    {
        private readonly IServiceChannel _coordinatorChannel;
        readonly ILog _log = LogManager.GetLogger(typeof(CommandProcessor));

        public CommandProcessor(IServiceChannel coordinatorChannel)
        {
            _coordinatorChannel = coordinatorChannel;
        }

        public void Process(string body)
        {
            _log.Info("Processing message: " + body);

            var obj = JObject.Parse(body);

            switch ((string)obj["Command"])
            {
                case DeployJobMessage.CommandText:
                    try
                    {
                        var mm = obj.ToObject<DeployJobMessage>();
                        new DeployJobMessageHandler().Handle(mm);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                    break;
            }
        }
    }
}