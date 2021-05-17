using Serilog;
using Sherlog.Gui.Viewmodels;
using Sherlog.Shared.Models;
using System;
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
    public class SherlogMessageClient : NetCoreServer.TcpClient
    {
        public SherlogMessageClient(IPAddress address, int port) : base(address, port)
        {
        }

        public SherlogMessageClient(string ipaddress, int port) : this(IPAddress.Parse(ipaddress), port)
        {

        }

        public void DisconnectAndStop()
        {
            _stop = true;
            DisconnectAsync();
            while (IsConnected)
            {
                Thread.Yield();
            }

        }

        protected override void OnConnected()
        {

        }

        protected override void OnDisconnected()
        {
            Log.Debug($"Chat TCP client disconnected a session with Id {Id}");

            // Wait for a while...
            Thread.Sleep(1000);

            // Try to connect again
            if (!_stop)
                ConnectAsync();
        }

        protected override void OnReceived(byte[] buffer, long offset, long size)
        {
            var msg = Encoding.UTF8.GetString(buffer, (int)offset, (int)size);

            (string command, string payload) = ReadMessage(msg);

            if (string.IsNullOrWhiteSpace(command))
            {
                return;
            }

            switch (command)
            {
                case Message.RETRIEVE_CONFIGS:
                    GetConfigs(payload);
                    break;
            }

        }

        private void GetConfigs(string payload)
        {
            var configs = JsonSerializer.Deserialize<List<ServiceConfiguration>>(payload);

            Application.Current.Dispatcher.Invoke(() => {
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


        protected override void OnError(SocketError error)
        {
            Log.Debug($"Chat TCP client caught an error with code {error}");
        }

        private bool _stop;
    }
}
