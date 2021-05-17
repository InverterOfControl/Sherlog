using NetCoreServer;
using Serilog;
using System.Net;
using System.Net.Sockets;

namespace Sherlog.Service.Communication
{
    public class SherlogServer : TcpServer
    {
        public SherlogServer(IPAddress address, int port) : base(address, port){ }

        public SherlogServer(string ipaddress, int port) : this(IPAddress.Parse(ipaddress), port)
        {
            
        }

        protected override TcpSession CreateSession() { return new SherlogSession(this); }

        protected override void OnError(SocketError error)
        {
            Log.Error($"Chat TCP server caught an error with code {error}");
        }
    }
}
