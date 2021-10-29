using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RemoteHealthcare_Client;
using RemoteHealthcare_Client.TCP;
using RemoteHealthcare_Shared.Settings;
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
            this.tcpClientHandler = new TCPClientHandler(ServerSettings.IP, ServerSettings.Port, true);

            this.tcpClientHandler.SetRunning(true);

            this.tcpClientHandler.OnMessageReceived += OnServerMessageReceived;
        }

        /// <summary>
        /// Method which send an JObject to the server
        /// </summary>
        /// <param name="data"></param>
        public override void ReceivedData(JObject data)
        {
            this.tcpClientHandler.WriteMessage(data.ToString());
        }

        /// <summary>
        /// Method which is used when a message from the server was received. Sends the JObject from the params
        /// to all the handlers
        /// </summary>
        /// <param name="data"></param>
        private void HandleServerMessage(JObject data)
        {
            this.SendToManagers(data);
        }

        /// <summary>
        /// Method which checks whether the message is valid and calls the HandleServerMessage method
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="message"></param>
        private void OnServerMessageReceived(object sender, string message)
        {
            //Empty message == error in the connection
            if (message == "") onNetworkError();

            JObject jObject = JsonConvert.DeserializeObject(message) as JObject;

            if (jObject == null) return;

            HandleServerMessage(jObject);
        }

        /// <summary>
        /// Method which stops the TCPClientHandler when the connection is lost
        /// </summary>
        private void onNetworkError()
        {
            // Closing the tcp handler to prevent data from going there
            this.tcpClientHandler.SetRunning(false);

            // Notifying the user of the connection error
        }
    }
}
