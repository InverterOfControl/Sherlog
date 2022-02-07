using Sherlog.Gui.Connection;
using Sherlog.Gui.Viewmodels;
using Sherlog.Shared.Models;
using System.Collections.Generic;
using System.Windows;

namespace Sherlog.Gui
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    public MainWindow()
    {
      InitializeComponent();

      DataContext = new SherlogViewModel
      {
        ServiceConfigurations = new System.Collections.Generic.List<Shared.Models.ServiceConfiguration>{
                new Shared.Models.ServiceConfiguration { ServiceName = "Test", BackupPath = "None" }
                }
      };

      string isConnected = Connector.IsConnected ? "Connected" : "Not connected";

      lblConnectionStatus.Text = $"{isConnected} to service!";
    }

    public void UpdateList(List<ServiceConfiguration> list)
    {
      DataContext =
      new SherlogViewModel
      {
        ServiceConfigurations = list
      };
    }

    private void txtEditor_SelectionChanged(object sender, RoutedEventArgs e)
    {
      string isConnected = Connector.IsConnected ? "Connected" : "Not connected";
      lblConnectionStatus.Text = $"{isConnected} to service!";
      //int row = txtEditor.GetLineIndexFromCharacterIndex(txtEditor.CaretIndex);
      //int col = txtEditor.CaretIndex - txtEditor.GetCharacterIndexFromLineIndex(row);
      //lblConnectionStatus.Text = "Line " + (row + 1) + ", Char " + (col + 1);
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
      Connector.RequestConfigurations();
    }
  }
}
