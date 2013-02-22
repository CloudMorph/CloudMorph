using System;
using System.Web;
using Microsoft.WindowsAzure.ServiceRuntime;

namespace GuestBook2_WebRole
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            Microsoft.WindowsAzure.CloudStorageAccount.SetConfigurationSettingPublisher((configName, configSetter) =>
            {
                configSetter(RoleEnvironment.GetConfigurationSettingValue(configName));
            });
        }

        void Application_End(object sender, EventArgs e)
        {
            //  Code that runs on application shutdown

        }

        void Application_Error(object sender, EventArgs e)
        {
            // Code that runs when an unhandled error occurs

        }
    }
}
