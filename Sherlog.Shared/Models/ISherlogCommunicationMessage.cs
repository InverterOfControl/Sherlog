using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sherlog.Shared.Models
{
  public interface ISherlogCommunicationMessage
  {
    Task ReceiveConfigs(IList<ServiceConfiguration> test);
  }
}
