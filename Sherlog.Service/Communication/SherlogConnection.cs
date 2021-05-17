using NetCoreServer;
using Serilog;
using Sherlog.Shared.Models;
using System;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;

namespace Sherlog.Service.Communication
{
    public class SherlogSession : TcpSession
    {
        public SherlogSession(TcpServer server) : base(server){ }

        protected override void OnConnected()
        {
            Log.Information($"Sherlog TCP session with Id {Id} connected!");

            // Send invite message
            string message = "Hi Gui!";
            SendAsync(message);
        }

        protected override void OnDisconnected()
        {
            Log.Debug($"Sherlog TCP session with Id {Id} disconnected!");
        }

        protected override void OnReceived(byte[] buffer, long offset, long size)
        {
            string message = Encoding.UTF8.GetString(buffer, (int)offset, (int)size);
            Log.Debug("Incoming: " + message);


            switch (message)
            {
                case Message.DISCONNECT:
                    Disconnect();
                    break;
                case Message.SEND_CONFIGS:
                    SendServiceConfigurations();
                    break;
                case Message.UPDATE_CONFIGS:
                    UpdateConfigurations();
                    break;
            }                
        }

        private void UpdateConfigurations()
        {
            throw new NotImplementedException();
        }

        private void SendServiceConfigurations()
        {
            var configs = SherlogService.serviceConfigurations.ToList();

            var message = JsonSerializer.Serialize(configs);

            SendAsync(string.Join('|',Message.RETRIEVE_CONFIGS, message));
        }

        protected override void OnError(SocketError error)
        {
            Log.Error($"Sherlog TCP session caught an error with code {error}");
        }
    }
}
