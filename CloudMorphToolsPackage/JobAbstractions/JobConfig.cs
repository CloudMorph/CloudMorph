using System.Configuration;

namespace JobAbstractions
{
    public class JobConfig
    {
        public string GetName()
        {
            return ConfigurationManager.AppSettings["Name"];
        }
    }
}