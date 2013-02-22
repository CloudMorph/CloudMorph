using System;

namespace CloudAbstractions.EnvironmentConfiguration
{
    public interface IEnvironmentConfiguration
    {
        IEnvironmentConfiguration WithResource(string resourceId);
        IEnvironmentConfiguration WithResource(string resourceId, Func<IEnvironmentConfiguration, IResource> func);
        IEnvironmentConfiguration WithResource(string resourceId, IResource resource);
        IEnvironmentConfiguration WithResource(string resourceId, Func<IEnvironmentConfiguration, ISerializer> serializerFactory, Func<object, IDeserializer> deserializerFactory);
        IEnvironmentConfiguration WithOutQueue();
        IEnvironmentConfiguration WithOutQueue(string name);
        IEnvironmentConfiguration WithInQueue();
        IEnvironmentConfiguration WithInQueue(string name);
    }
}