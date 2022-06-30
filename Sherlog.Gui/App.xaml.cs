using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sherlog.Gui.Configuration;
using System.IO;
using System.Windows;

namespace Sherlog.Gui
{
  /// <summary>
  /// Interaction logic for App.xaml
  /// </summary>
  public partial class App : Application
  {

    private readonly ServiceProvider _serviceProvider;

    public IConfiguration Configuration { get; private set; }

    public App()
    {
      Configuration = new ConfigurationBuilder()
       .SetBasePath(Directory.GetCurrentDirectory())
       .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
       .Build();

      var serviceCollection = new ServiceCollection();
      ConfigureServices(serviceCollection);
      _serviceProvider = serviceCollection.BuildServiceProvider();
    }

    private void ConfigureServices(IServiceCollection services)
    {
      var config = Configuration.Get<AppConfig>();


      //services.AddSingleton<ILogBase>(new LogBase(new FileInfo($@"C:\temp\log.txt")));
      services.AddSingleton(_ => new HubConnectionBuilder()
                .WithUrl(config.ServerAddress)
                .WithAutomaticReconnect()
                .Build());

      services.AddSingleton<MainWindow>();
    }

    private async void OnStartup(object sender, StartupEventArgs e)
    {
      var mainWindow = _serviceProvider.GetService<MainWindow>();
      var signalRConnection = _serviceProvider.GetService<HubConnection>();
      try
      {
        await signalRConnection.StartAsync();
      }
      catch { }
      
      mainWindow.Show();
    }
  }
}
