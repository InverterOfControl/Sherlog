using Microsoft.AspNetCore.SignalR.Client;
using Sherlog.Gui.Viewmodels;
using Sherlog.Shared.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Sherlog.Gui
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    HubConnection connection;

    public MainWindow(HubConnection conn)
    {
      InitializeComponent();

      connection = conn;

      connection.Closed += async (error) =>
      {
        lblConnectionStatus.Text = "not connected to service!";
        await Task.Delay(new Random().Next(0, 5) * 1000);
        await connection.StartAsync();
      };

      connection.On<IList<ServiceConfiguration>>("ReceiveConfigs", (configs) =>
      {
        UpdateList(configs);
      });
    } 

    public void UpdateList(IList<ServiceConfiguration> list)
    {
      DataContext =
      new SherlogViewModel
      {
        ServiceConfigurations = (List<ServiceConfiguration>)list
      };
    }

    private void txtEditor_SelectionChanged(object sender, RoutedEventArgs e)
    {
      string isConnected = connection.State == HubConnectionState.Connected ? "Connected" : "Not connected";

      lblConnectionStatus.Text = $"{isConnected} to service!";
      //int row = txtEditor.GetLineIndexFromCharacterIndex(txtEditor.CaretIndex);
      //int col = txtEditor.CaretIndex - txtEditor.GetCharacterIndexFromLineIndex(row);
      //lblConnectionStatus.Text = "Line " + (row + 1) + ", Char " + (col + 1);
    }

    private async void Button_Click(object sender, RoutedEventArgs e)
    {
      await connection.InvokeAsync("RequestAllConfigs");
    }

    private void ListViewItem_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      var item = sender as ListViewItem;
      if (item != null && item.IsSelected)
      {
        ((SherlogViewModel)DataContext).SelectedConfiguration = (ServiceConfiguration)item.Content;
      }
    }
  }
}
