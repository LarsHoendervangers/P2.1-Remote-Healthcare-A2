using Newtonsoft.Json.Linq;
using RemoteHealthcare_Client;
using RemoteHealthcare_Client.TCP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoteHealthcare_Dokter.BackEnd
{
    class ServerDataManager : DataManager
    {
        private TCPClientHandler tcpClientHandler;

        public ServerDataManager()
        {
            this.tcpClientHandler = new TCPClientHandler("127.0.0.1", 6969);

            this.tcpClientHandler.OnMessageReceived += OnServerMessageReceived;
        }

        public override void ReceivedData(JObject data)
        {
            throw new NotImplementedException();
        }

        private void HandleServerMessage(JObject data)
        {

        }

        private void OnServerMessageReceived(object sender, string message)
        {

        }
    }
}
