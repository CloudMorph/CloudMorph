using System;
using Amazon.Runtime;
using CloudAbstractions;
using CloudAbstractions.Messaging;

namespace AwsAbstractions
{
    public class AwsRealm : IRealm
    {
        private readonly BasicAWSCredentials _awsCredentials = new BasicAWSCredentials("AKIAJWLHA6MRH2H4LHDQ", "usQf8vof9eN4zDnsv7ME7JWSoy+ZNRN6ZUI3ojS7");

        public AwsRealm()
        {
            //ec2.setEndpoint("ec2.eu-west-1.amazonaws.com");
        }

        public IRealmConfigurator Configure()
        {
            throw new NotImplementedException();
        }

        public IStorageProvider StorageProvider
        {
            get { return new AwsStorageProvider(_awsCredentials); }
        }

        public IComputeInfrastructureProvider ComputeInfrastructureProvider
        {
            get { return new AwsComputeInfrastructureProvider(_awsCredentials); }
        }

        public IQueueProvider QueueProvider
        {
            get { return new AwsQueueProvider(_awsCredentials); }
        }

        public IKvStorageProvider KvStorageProvider
        {
            get { return new AwsSimpleDbProvider(_awsCredentials);  }
        }

        public IEnvironment Current
        {
            get { return new AwsEnvironment(); }
        }
    }
}