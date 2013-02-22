using System;
using AwsAbstractions;
using AzureAbstractions;
using CloudAbstractions;
using LocalAbstractions;

namespace Commons
{
    public static class CloudMorphFactory
    {
        public static IRealm GetRealm(string realmId = "local")
        {
            if (string.IsNullOrEmpty(realmId))
                return null;

            switch (realmId.ToLower())
            {
                case "azure":
                    return new AzureRealm();

                case "aws":
                    return new AwsRealm();

                case "local":
                    return new LocalRealm();
            }

            throw new NotSupportedException("Specified Realm is unknown");
        }
    }
}