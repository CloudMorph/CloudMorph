using System;
using System.Text;
using Amazon.EC2;
using Amazon.EC2.Model;
using Amazon.Runtime;
using CloudAbstractions;

namespace AwsAbstractions
{
    public class AwsComputeInfrastructureProvider : IComputeInfrastructureProvider
    {
        private readonly AWSCredentials _awsCredentials;

        public AwsComputeInfrastructureProvider(AWSCredentials awsCredentials)
        {
            _awsCredentials = awsCredentials;
        }

        public void LaunchInstance(string instanceName, InstanceSize size, string metadata)
        {
            var ec2Client = new AmazonEC2Client(_awsCredentials);

            string instanceType = "t1.micro";
            // WEB: string imageId = "ami-a9da0ec0";
            // Compute: string imageId = "ami-443fe32d";
            string imageId = "ami-09d96e60";

            switch (size)
            {
                case InstanceSize.Medium:
                    instanceType = "t1.small";
                    break;

                case InstanceSize.Small:
                default:
                    instanceType = "t1.micro";
                    break;
            }

            var request = new RunInstancesRequest()
                .WithInstanceType(instanceType)
                .WithPlacement(new Placement().WithAvailabilityZone("us-east-1d"))
                .WithImageId(imageId)
                .WithMinCount(1)
                .WithMaxCount(1)
                .WithSecurityGroup("default")
                .WithKeyName("IgorTest3");

            if (!string.IsNullOrEmpty(metadata))
                request = request.WithUserData(Convert.ToBase64String(Encoding.UTF8.GetBytes(metadata)));

            var runInstancesResponse = ec2Client.RunInstances(request);

            var instances = runInstancesResponse.RunInstancesResult.Reservation.RunningInstance;
            var index = 0;
            foreach (var instance in instances)
            {
                var name = instanceName;

                if (instances.Count > 0)
                    instanceName = instanceName + index;

                var createTagsRequest = new CreateTagsRequest();
                createTagsRequest
                    .WithResourceId(instance.InstanceId)
                    .WithTag(new Tag().WithKey("Name")
                    .WithValue(name));
                ec2Client.CreateTags(createTagsRequest);

                index++;
            }

/*
            string rsaPrivateKey;
            using (var reader = new StreamReader(@"C:\Dev\BlueMetal\CloudAbstractions\SecurityStorage\IgorKeyPair.pem"))
            {
                rsaPrivateKey = reader.ReadToEnd();
            }
            var result = ec2Client.GetPasswordData(
                        new GetPasswordDataRequest().WithInstanceId(instanceId))
                .GetPasswordDataResult;

            Console.WriteLine(result.GetDecryptedPassword(rsaPrivateKey));
*/

            /*
                        AmazonEC2 ec2 = AWSClientFactory.CreateAmazonEC2Client(
                          appConfig["AWSAccessKey"],
                          appConfig["AWSSecretKey"],
                          new AmazonEC2Config().WithServiceURL("https://eu-west-1.ec2.amazonaws.com")
                          );

                        DescribeInstancesRequest ec2Request = new DescribeInstancesRequest();
            */
        }
    }
}