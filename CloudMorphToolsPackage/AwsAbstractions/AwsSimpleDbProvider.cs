using System.Collections.Generic;
using System.Linq;
using Amazon.Runtime;
using Amazon.SimpleDB;
using Amazon.SimpleDB.Model;
using CloudAbstractions;

namespace AwsAbstractions
{
    public class AwsSimpleDbProvider : IKvStorageProvider
    {
        private readonly AWSCredentials _awsCredentials;
        private AmazonSimpleDBClient _simpleDbClient;

        public AwsSimpleDbProvider(AWSCredentials awsCredentials)
        {
            _awsCredentials = awsCredentials;

            _simpleDbClient = new AmazonSimpleDBClient(_awsCredentials);
        }

        public void CreateTable(string domain)
        {
            CreateDomainRequest request = new CreateDomainRequest()
                .WithDomainName(domain);
            _simpleDbClient.CreateDomain(request);
        }

        public void Get(string domain, string id)
        {
            GetAttributesRequest request = new GetAttributesRequest()
                .WithDomainName(domain)
                .WithItemName(id);
            var response = _simpleDbClient.GetAttributes(request);
        }

        public void GetAll()
        {
            SelectRequest request = new SelectRequest()
                .WithConsistentRead(true)
                .WithSelectExpression("SELECT * FROM WHERE");
            var response = _simpleDbClient.Select(request);
        }

        public void Put(string domain, string id, Dictionary<string, string> properties, bool replace = false)
        {
            PutAttributesRequest request = new PutAttributesRequest()
                .WithDomainName(domain)
                .WithItemName(id)
                .WithAttribute(properties.Select(kv =>
                                                 new ReplaceableAttribute().WithName(kv.Key).WithValue(kv.Value).
                                                     WithReplace(replace)).ToArray());
            //    .WithExpected(new UpdateCondition())

            _simpleDbClient.PutAttributes(request);
        }

        public IEnumerable<Dictionary<string, object>> GetAll(string domain)
        {
            throw new System.NotImplementedException();
        }
    }
}