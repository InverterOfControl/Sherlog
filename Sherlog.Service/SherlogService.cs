using Serilog;
using Sherlog.Service.Actions;
using Sherlog.Service.Communication;
using Sherlog.Service.Configuration;
using Sherlog.Shared.Helper;
using Sherlog.Shared.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
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

        private SherlogServer server;

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

            server = new SherlogServer(appConfig.ListenAddress, appConfig.ListenPort);

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

                var logs = Directory.GetFiles(service.LogPath);

                List<LogProcessModel> logsToProcess = new List<LogProcessModel>();

                foreach (string log in logs)
                {
                    var date = Parser.TryExtractDateFromFilename(log);

                    if (date == null) continue;

                    if ((DateTime.UtcNow - date.Value).Days < service.DaysToKeepUnprocessed)
                    {
                        continue;
                    }

                    logsToProcess.Add(new LogProcessModel(log, date.Value));
                }

                foreach (var logresult in Grouper.GroupLogs(logsToProcess))
                {
                    if (service.DoBackups)
                    {
                        string newFileName = $"{service.BackupPath}\\{logresult.LogTypeName}.{logresult.Timerange}.zip";

                        Compressor.Compress(logresult.Filepaths.ToArray(), newFileName);
                    }

                    Deleter.Delete(logresult.Filepaths.ToArray());
                }

            }
        }

        public bool Stop(HostControl hostControl)
        {
            mainScheduler.Stop();
            server.Stop();
            Log.Debug("Service stopped!");
            return false;
        }

    }
}
