using System;
using Microsoft.Owin.Hosting;
using Topshelf;

namespace IASServiceServer
{
    public class MainService : ServiceControl
    {
       
        private IDisposable WebServer { get; set; }

        public bool Start(HostControl hostControl)
        {
            string baseAddress = AppConfig.BaseAddress;
            WebServer = WebApp.Start<Startup>(baseAddress);
            Logger.Information(string.Format("Service is Started at {0}", baseAddress));
            return true;
        }

        public bool Stop(HostControl hostControl)
        {
            if (WebServer != null)
            {
                WebServer.Dispose();
            }
            return true;
        }
    }
}
