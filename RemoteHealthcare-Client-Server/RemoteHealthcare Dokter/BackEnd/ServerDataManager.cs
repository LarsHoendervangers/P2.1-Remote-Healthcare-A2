using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RemoteHealthcare_Client;
using RemoteHealthcare_Client.TCP;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

            this.tcpClientHandler.SetRunning(true);

            this.tcpClientHandler.OnMessageReceived += OnServerMessageReceived;
        }

        public override void ReceivedData(JObject data)
        {
            // The server will only get messages to login, all other is not defined in the data protocol.
            Trace.WriteLine($"received data from server: {data}");
            this.tcpClientHandler.WriteMessage(data.ToString());
        }

        private void HandleServerMessage(JObject data)
        {
            Trace.WriteLine(data);
            this.SendToManagers(data);
        }

        private void OnServerMessageReceived(object sender, string message)
        {
            JObject jObject = JsonConvert.DeserializeObject(message) as JObject;

            if (jObject == null) return;

            HandleServerMessage(jObject);
        }
    }
}
