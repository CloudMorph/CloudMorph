using Nancy;

namespace CloudMorphWebPortal.nancy
{
    public class Module : NancyModule
    {
        public Module() : base("/nancy")
        {
            Get["/test"] = x =>
                               {
                                   return "Hello";
                               };
        }
    }
}