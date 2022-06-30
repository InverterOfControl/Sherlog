using Microsoft.Extensions.Hosting.WindowsServices;
using Serilog;
using Serilog.Events;
using Sherlog.Service;
using Sherlog.Service.Actions;
using Sherlog.Service.Configuration;
using Sherlog.Service.Hubs;
using Sherlog.Shared.Models;

var options = new WebApplicationOptions
{
  Args = args,
  ContentRootPath = WindowsServiceHelpers.IsWindowsService() ? AppContext.BaseDirectory : default
};


Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .Enrich.FromLogContext()
            .WriteTo.File(options.ContentRootPath + "/Logs/Sherlog/log.log", rollingInterval: RollingInterval.Day)
            .CreateLogger();


var hostBuilder = WebApplication.CreateBuilder(options);

hostBuilder.Services.AddSignalR();
hostBuilder.Services.AddHostedService<WindowsBackgroundService>();
hostBuilder.Services.Configure<AppConfiguration>(hostBuilder.Configuration.GetSection("AppConfig"));
hostBuilder.Services.AddSingleton<IList<ServiceConfiguration>>(_ => ConfigBuilder.BuildServiceConfigs().ToList());
hostBuilder.Services.AddSingleton<IMainWorker, MainWorker>();
hostBuilder.Services.AddSingleton<IDeleter, Deleter>();

hostBuilder.Host.UseSerilog();

hostBuilder.Host.UseWindowsService(options =>
{
  options.ServiceName = "Sherlog.Service";
});

var host = hostBuilder.Build();

host.MapHub<CommunicationHub>("/hubs/comm");

await host.RunAsync();
