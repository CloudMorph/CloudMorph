using System;
using System.Threading.Tasks;
using log4net;
using Topshelf;
using Topshelf.Model;

namespace CloudMorphDeploymentService
{
    using Stact;
    using Stact.MessageHeaders;
    using Topshelf.Messages;


    public class DeploymentService
    {
        private readonly ServiceDescription _description;
        private readonly IServiceChannel _coordinatorChannel;
        //readonly Timer _timer;
        readonly log4net.ILog _log = LogManager.GetLogger(typeof(DeploymentService));
        //private NancyHost _host;

/*
        public DeploymentService()
        {
*/
/*
            _timer = new Timer(3000) { AutoReset = true };
            _timer.Elapsed += (sender, eventArgs) => TimedMessageProcessor();
*/
/*
        }
*/

        public DeploymentService(ServiceDescription description, IServiceChannel coordinatorChannel) //: this()
        {
            _description = description;
            _coordinatorChannel = coordinatorChannel;
        }

/*
        public void TimedMessageProcessor()
        {
            //_log.Info(DateTime.Now);

            var message = _queueProvider.ReceiveMessage(_queueDeploymentsCommands);

            if (message != null)
            {
                _log.Info("Received message: " + message.Body);

                var task = new Task(() => new CommandProcessor(_coordinatorChannel).Process(message.Body));

                task.Start();
            }
        }
*/

/*
*/

        public void Start()
        {
//            _timer.Start();

            new Task(DelayedStart).Start();
        }

        private void DelayedStart()
        {
            //TinyIoC.TinyIoCContainer.Current.Register<IServiceChannel>(_coordinatorChannel);

            // initialize an instance of NancyHost (found in the Nancy.Hosting.Self package)
            //_host = new NancyHost(new Uri("http://localhost:8085"));
            //_host.Start(); // start hosting


/*
            AnonymousActor.New(inbox =>
            {
                _coordinatorChannel.Send<Request<ServiceStatus>>(new RequestImpl<ServiceStatus>(inbox, new ServiceStatus()));

                inbox.Receive<Response<ServiceStatus>>(response =>
                {
                    //var view = new DashboardView(response.Body.Services);

                    //context.Response.RenderSparkView(view, "dashboard.html");
                    //context.Complete();

                    Console.WriteLine(response.Body.Services);

                }, TimeSpan.FromSeconds(30), () =>
                {
                    //context.Response.StatusCode = (int)HttpStatusCode.ServiceUnavailable;
                    //context.Complete();
                });
            });
*/
        }

        public void Stop()
        {
//            _timer.Stop();

            //_host.Stop();
        }
    }
}