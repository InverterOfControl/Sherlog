using Sherlog.Shared.Models;
using System.Collections.Generic;
using System.ComponentModel;

namespace Sherlog.Gui.Viewmodels
{
  public class SherlogViewModel : INotifyPropertyChanged
  {
    public event PropertyChangedEventHandler PropertyChanged;

    public void NotifyPropertyChanged(string propertyName)
    {
      if (PropertyChanged != null)
        PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
    }

    private List<ServiceConfiguration> _serviceConfigurations;

    private ServiceConfiguration _selectedConfiguration;

    public List<ServiceConfiguration> ServiceConfigurations
    {
      get
      {
        return _serviceConfigurations;
      }

      set
      {
        _serviceConfigurations = value;
        NotifyPropertyChanged(nameof(ServiceConfigurations));
      }
    }

    public ServiceConfiguration SelectedConfiguration
    {
      get { return _selectedConfiguration; }
      set
      {
        _selectedConfiguration = value;
        NotifyPropertyChanged(nameof(SelectedConfiguration));
      }
    }
  }
}
