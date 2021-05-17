using Microsoft.Extensions.Configuration;
using Sherlog.Gui.Configuration;
using Sherlog.Gui.Connection;
using Sherlog.Shared.Models;
using System.Collections.Generic;
using System.IO;
using System.Windows;

namespace Sherlog.Gui
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public IConfiguration Configuration { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            Configuration = new ConfigurationBuilder()
             .SetBasePath(Directory.GetCurrentDirectory())
             .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
             .Build();

            var config = Configuration.Get<AppConfig>();

            Connector.Connect(config);
        }
    }
}
