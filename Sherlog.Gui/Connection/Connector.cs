using NetCoreServer;
using Sherlog.Gui.Configuration;

namespace Sherlog.Gui.Connection
{
    public static class Connector
    {
        public static bool IsConnected = false;

        private static TcpClient Client;

        public static TcpClient Connect(AppConfig config)
        {
            var client = new SherlogMessageClient(config.ServerAddress, config.ServerPort);

            IsConnected = client.ConnectAsync();

            Client = client;

            return Client;
        }

        public static void RequestConfigurations()
        {
            Client.SendAsync("SC");
        }
    }
}
