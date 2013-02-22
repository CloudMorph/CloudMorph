using System;
using System.Collections.Generic;
using System.Linq;
using CloudAbstractions;
using Microsoft.WindowsAzure.ServiceRuntime;

namespace AzureAbstractions
{
    // http://msdn.microsoft.com/en-us/library/microsoft.windowsazure.serviceruntime.roleenvironment.aspx
    public class AzureEnvironment : IEnvironment
    {
        public string MyLocalAddress
        {
            get
            {
                var roleInstance = RoleEnvironment.CurrentRoleInstance;

                var endPoint = roleInstance.InstanceEndpoints.Values.First();
                return endPoint.IPEndpoint.Address.ToString();
            }
        }

        public string MyPublicAddress
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public IDictionary<string, string> GetMetadata()
        {
            throw new NotImplementedException();
        }
    }
}