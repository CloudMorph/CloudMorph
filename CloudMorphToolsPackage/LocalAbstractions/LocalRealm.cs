using System;
using CloudAbstractions;
using CloudAbstractions.Messaging;

namespace LocalAbstractions
{
    public class LocalRealm : IRealm
    {
        public IRealmConfigurator Configure()
        {
            throw new NotImplementedException();
        }

        public IStorageProvider StorageProvider
        {
            get { throw new NotImplementedException(); }
        }

        public IComputeInfrastructureProvider ComputeInfrastructureProvider
        {
            get { throw new NotImplementedException(); }
        }

        public IQueueProvider QueueProvider
        {
            get { throw new NotImplementedException(); }
        }

        public IKvStorageProvider KvStorageProvider
        {
            get { throw new NotImplementedException(); }
        }

        public IEnvironment Current
        {
            get { throw new NotImplementedException(); }
        }
    }
}