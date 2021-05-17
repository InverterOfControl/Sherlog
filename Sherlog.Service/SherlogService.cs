using Serilog;
using Sherlog.Service.Communication;
using Sherlog.Service.Configuration;
using Sherlog.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using Topshelf;

namespace Sherlog.Service
{
    public class SherlogService : ServiceControl
    {
        private AppConfiguration appConfig;

        public static IEnumerable<ServiceConfiguration> serviceConfigurations;

        private Timer mainScheduler;


        public bool Start(HostControl hostControl)
        {
            appConfig = ConfigBuilder.GetConfig<AppConfiguration>("appsettings.json");
            serviceConfigurations = ConfigBuilder.BuildServiceConfigs();
            
            mainScheduler = new Timer
            {
                Interval = 60000 * appConfig.MinutesBetweenChecks
            };

            mainScheduler.Elapsed += new ElapsedEventHandler(MainCheckLoop);

            mainScheduler.Start();

            Log.Debug("Scheduler started!");

            var server = new SherlogServer(appConfig.ListenAddress, appConfig.ListenPort);

            if (server.Start())
            {
                Log.Debug("Messaging-Server started!");
            }

            return true;
        }

        private void MainCheckLoop(object sender, ElapsedEventArgs e)
        {
            Log.Debug("Execute mainloop");

            foreach (var service in serviceConfigurations)
            {
                Log.Information($"Looking for logs for {service.ServiceName}");
            }
        }

        public bool Stop(HostControl hostControl)
        {
            mainScheduler.Stop();
            Log.Debug("Service stopped!");
            return false;
        }

    }
}
