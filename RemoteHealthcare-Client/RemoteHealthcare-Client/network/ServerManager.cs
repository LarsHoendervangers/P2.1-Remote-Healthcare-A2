using Newtonsoft.Json.Linq;
using RemoteHealthcare_Client.TCP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RemoteHealthcare_Client
{
    public class ServerDataManager : IDataManager
    {
       
        private TCPClientHandler TCPClientHandler { get; set; }

        public IDataManager DeviceDataManager { get;  set; }

        public IDataManager VRDataManager { get; set; }

        public ServerDataManager(string ip, int port)
        {
            this.TCPClientHandler = new TCPClientHandler(ip, port);

            this.TCPClientHandler.OnMessageReceived += OnMessagReceived;
        }

        private void OnMessagReceived(object sender, string e)
        {
        }

        public void ReceivedData(JObject data)
        {
            throw new NotImplementedException();
        }
    }
}