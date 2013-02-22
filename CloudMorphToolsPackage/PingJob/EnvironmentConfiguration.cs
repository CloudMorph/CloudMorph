using System;
using CloudAbstractions.EnvironmentConfiguration;

namespace PingJob
{
    public class EnvironmentConfiguration : IEnvironmentSetup
    {
        public void EnvironmentRequirements(IEnvironmentConfiguration cfg)
        {
            cfg
                .WithOutQueue()
                .WithInQueue();
            //.WithResource("inQueue")
            //.WithResource("inQueue", serializer => (ISerializer)null, deserializer => (IDeserializer)null )
            //.WithResource("outQueue")
            //.WithResource("inQueue", env => CreateInQueue(env))
            //.WithResource("outQueue", (IResource) null);

            //PresiceEnvironmentConfiguration.Configure(cfg)
            //    .WithQueueBroker();
        }
    }

/*
    public class PresiceEnvironmentConfiguration
    {
        private readonly IEnvironmentConfiguration _cfg;

        public static PresiceEnvironmentConfiguration Configure(IEnvironmentConfiguration cfg)
        {
            return new PresiceEnvironmentConfiguration(cfg);
        }

        PresiceEnvironmentConfiguration(IEnvironmentConfiguration cfg)
        {
            _cfg = cfg;
        }

        public void WithQueueBroker()
        {
            throw new NotImplementedException();
        }
    }
*/
}