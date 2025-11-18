using System;
using System.Configuration;
using Topshelf;

namespace IASServiceServer
{
    class Program
    {

        static void Main()
        {
            try
            {
                HostFactory.Run(hostConfigurator =>
                {
                    hostConfigurator.SetServiceName(ConfigurationManager.AppSettings["ServiceName"]);
                    hostConfigurator.SetDisplayName(ConfigurationManager.AppSettings["DisplayName"]);
                    hostConfigurator.SetDescription(ConfigurationManager.AppSettings["Description"]);
                    hostConfigurator.Service<MainService>(serviceConfigurator =>
                    {
                        serviceConfigurator.ConstructUsing(() => new MainService());
                        serviceConfigurator.WhenStarted((service, control) => service.Start(control));
                        serviceConfigurator.WhenStopped((service, control) => service.Stop(control));
                    });
                    hostConfigurator.RunAsLocalSystem();
                    hostConfigurator.EnableServiceRecovery(recoveryOptions =>
                    {
                        var x = 0;
                        while (x < 3)
                        {
                            x++;
                            recoveryOptions.RestartService(x);
                        }
                    });
                    hostConfigurator.EnableShutdown();
                    hostConfigurator.StartAutomatically();
                });
            }
            catch (Exception e)
            {
                Logger.Error(e);
            }
        }
    }
}
