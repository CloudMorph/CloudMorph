using System;
using Magnum.Extensions;
using Nancy;
using Stact;
using Stact.MessageHeaders;
using TinyIoC;
using Topshelf.Messages;
using Topshelf.Model;

namespace CloudMorphDeploymentService.Dashboard
{
	public class DashboardModule : NancyModule
	{
		public DashboardModule()
		{
			Get["/jobs"] = x =>
							   {
								   var serviceChannel = TinyIoCContainer.Current.Resolve<IServiceChannel>();

							       AnonymousActor.New(inbox =>
							                              {
							                                  serviceChannel.Send
							                                      <Request<Topshelf.Messages.ServiceStatus>>(
							                                          new RequestImpl<ServiceStatus>(inbox,
							                                                                         new ServiceStatus()));

							                                  inbox.Receive<Response<ServiceStatus>>(response =>
							                                                                             {
							                                                                                 //var view = new DashboardView(response.Body.Services);

							                                                                                 Console.WriteLine(response.Body.ToString());

							                                                                                 //context.Response.RenderSparkView(view, "dashboard.html");
							                                                                                 //context.Complete();
							                                                                             }, 30.Seconds(),
							                                                                         () =>
							                                                                             {
							                                                                                 //context.Response.StatusCode = (int)HttpStatusCode.ServiceUnavailable;
							                                                                                 //context.Complete();
							                                                                             });
							                              });
								   return "Hello";
                    };
		}
	}
}