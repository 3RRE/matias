using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Threading.Tasks;
using System.Web.Http;
using IASServiceServer.Jobs;
using Owin;
using Quartz;
using Quartz.Impl;

namespace IASServiceServer
{
    public class Startup
    {
        public void Configuration(IAppBuilder appBuilder)
        {
            var config = new HttpConfiguration();
            config.EnableSystemDiagnosticsTracing();
            config.MapHttpAttributeRoutes();
            config.Routes.MapHttpRoute(
                "DefaultApi",
                "api/{controller}/{id}",
                new { id = RouteParameter.Optional }
            );

            appBuilder.UseWebApi(config);

            Task.Run(async () =>
            {
                await StartSchedulers();
            });
          
        }

        private async Task StartSchedulers()
        {
            // construct a scheduler factory
            NameValueCollection props = new NameValueCollection
            {
                { "quartz.serializer.type", "binary" }
            };

            StdSchedulerFactory factory = new StdSchedulerFactory(props);

            // get a scheduler
            IScheduler sched = await factory.GetScheduler();

            await sched.Start();

            IJobDetail job = JobBuilder.Create<SincronizarSalasJob>().Build();
            ITrigger trigger = null;
            var builder = TriggerBuilder.Create().WithIdentity("SincronizarSalasTrigger", "MainSchedulersGroup");
            var dailyInterval = ConfigurationManager.AppSettings["DailyInterval"];
            if (!string.IsNullOrEmpty(dailyInterval))
            {
                var interval = dailyInterval.Split(':');
                if (interval.Length > 1)
                {
                    try
                    {
                        int hour = int.Parse(interval[0]);
                        int minute = int.Parse(interval[1]);
                        builder.WithDailyTimeIntervalSchedule(s => s
                            .OnEveryDay()
                            .StartingDailyAt(TimeOfDay.HourAndMinuteOfDay(hour, minute)).EndingDailyAfterCount(1));
                        trigger = builder.Build();
                    }
                    catch (Exception ex)
                    {
                        Logger.Error(ex);
                    }
                }
            }

            if (trigger == null)
            {
                builder.StartNow().WithSimpleSchedule(x => x.RepeatForever().WithIntervalInHours(24));
                trigger = builder.Build();
            }

            await sched.ScheduleJob(job, trigger);
        }
    }
}
