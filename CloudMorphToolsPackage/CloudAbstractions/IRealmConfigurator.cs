using System;
using CloudAbstractions.Messaging;

namespace CloudAbstractions
{
    public interface IRealmConfigurator
    {
        IRealmConfigurator WithStorageProvider(Action<IStorageProvider> factory);
        IRealmConfigurator WithQueueProvider(Action<IQueueProvider> factory);
    }
}