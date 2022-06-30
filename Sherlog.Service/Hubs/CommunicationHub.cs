using Microsoft.AspNetCore.SignalR;
using Sherlog.Shared.Models;
using System.Text.Json;

namespace Sherlog.Service.Hubs
{
  public class CommunicationHub : Hub<ISherlogCommunicationMessage>
  {
    private readonly IList<ServiceConfiguration> configs;

    public CommunicationHub(IList<ServiceConfiguration> configs)
    {
      this.configs = configs;
    }
    public async Task RequestAllConfigs()
    {
      await Clients.Caller.ReceiveConfigs(configs);
    }
  }
}
