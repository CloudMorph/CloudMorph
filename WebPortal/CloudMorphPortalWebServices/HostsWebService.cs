using System;
using System.Threading;
using CloudMorphPortalWebServices.Models;
using Nancy;

namespace CloudMorphPortalWebServices
{
    public class HostsWebService : NancyModule
    {
        public HostsWebService() : base("/svc/hosts")
        {
            Get["/"] = x =>
                           {
                               var hosts = new[]
                                               {
                                                   new Host { Guid = Guid.NewGuid(), IP = "192.168.10.5", Name = "Service1" },
                                                   new Host { Guid = Guid.NewGuid(), IP = "192.168.10.6", Name = "Service2" },
                                                   new Host { Guid = Guid.NewGuid(), IP = "192.168.10.7", Name = "Service3" }
                                                };

                               return Response.AsJson(hosts);
                           };

            Get["{id}/packages"] = @params =>
            {
                string id = @params["id"];
                var packages = new[]
                                    {
                                        new Package { Guid = Guid.NewGuid(), Name = "Package 1 -> " + id, Size = 1234567890 },
                                        new Package { Guid = Guid.NewGuid(), Name = "Package 2 -> " + id, Size = 1234567890 },
                                        new Package { Guid = Guid.NewGuid(), Name = "Package 3 -> " + id, Size = 1234567890 },
                                        new Package { Guid = Guid.NewGuid(), Name = "Package 4 -> " + id, Size = 1234567890 }
                                };

                //Thread.Sleep(10000);

                return Response.AsJson(packages);
            };
        }
    }
}