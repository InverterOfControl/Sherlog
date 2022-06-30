using Serilog;
using Sherlog.Gui.Viewmodels;
using Sherlog.Shared.Models;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Windows;
using System.Windows.Threading;

namespace Sherlog.Gui.Connection
{
  public class SherlogMessageClient
  {

  

    private void GetConfigs(string payload)
    {
      var configs = JsonSerializer.Deserialize<List<ServiceConfiguration>>(payload);

      Application.Current.Dispatcher.Invoke(() =>
      {
        Application.Current.MainWindow.DataContext = new SherlogViewModel { ServiceConfigurations = configs };
      }, DispatcherPriority.ContextIdle);

    }

    private (string command, string payload) ReadMessage(string msg)
    {
      string[] strings = msg.Split('|');

      if (strings.Length == 2)
      {
        return (strings[0], strings[1]);
      }

      return (null, null);
    }


    private bool _stop;
  }
}
