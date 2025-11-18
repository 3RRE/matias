using System.ServiceProcess;

namespace IASServiceClient
{
    public partial class Service1 : ServiceBase
    {
        public Service1()
        {
            InitializeComponent();
        }
        readonly RunService _serviceClient = new RunService();
        protected override void OnStart(string[] args)
        {
            _serviceClient.Start();
        }

        protected override void OnStop()
        {
            _serviceClient.Stop();
        }
    }
}
