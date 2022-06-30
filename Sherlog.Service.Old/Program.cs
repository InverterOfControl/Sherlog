using Serilog;
using System;
using Topshelf;

namespace Sherlog.Service
{
    class Program
    {
        static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
            .WriteTo.File(
                AppDomain.CurrentDomain.BaseDirectory + "\\logs\\log.txt",
                rollingInterval: RollingInterval.Day)
            .WriteTo.Console()
            .MinimumLevel.Debug()
            .CreateLogger();

            HostFactory.Run(
                x => x.Service<SherlogService>()
                .UseSerilog()
                );

        }
    }
}
