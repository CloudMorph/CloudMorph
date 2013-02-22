using Nancy;

namespace CloudMorphPortalWebServices
{
    public class RootWebService : NancyModule
    {
        public RootWebService() : base("/svc")
        {
            //Get["/"] = x => "Hello";

            Get["/Jobs"] = x =>
                               {
                                   var jobs = new[] {new {name = "Test"}, new {name = "Blam"}, new {name = "BaBam"}};

                                   return Response.AsJson(jobs);
                               };
        }
    }
}