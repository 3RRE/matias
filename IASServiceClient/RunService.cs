using Microsoft.Owin.Hosting;
using System;
using System.Configuration;

namespace IASServiceClient
{
    public class RunService
    {
        private IDisposable _app;
        public void Start()
        {
            var baseUri = ConfigurationManager.AppSettings["BaseUri"];
            StartOptions options = new StartOptions();
            _app = WebApp.Start<Startup>(baseUri);
            //Console.WriteLine("Server is running at {0}", baseUri);
        }
        public void Stop()
        {
            _app.Dispose();
        }
    }
}