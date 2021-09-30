using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RemoteHealthcare_Client.TCP;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace RemoteHealthcare_Client
{
    public class ServerDataManager : DataManager
    {
       
        private TCPClientHandler TCPClientHandler { get; set; }

        public ServerDataManager(string ip, int port)
        {
            this.TCPClientHandler = new TCPClientHandler(ip, port);
            this.TCPClientHandler.SetRunning(true);

            this.TCPClientHandler.OnMessageReceived += OnMessageReceived;
        }

        private void OnMessageReceived(object sender, string message)
        {
            // REMOVE
            Trace.WriteLine("---------------------" + message);
            //Reading input
            JObject jobject = JsonConvert.DeserializeObject(message) as JObject;

            if (jobject != null) HandleIncoming(jobject); else
            {
                Debug.WriteLine("JObject is null");
            };
        }

        private void HandleIncoming(JObject jobject)
        {
            // command value always gives the action 
            JToken value;

            bool correctCommand = jobject.TryGetValue("command", StringComparison.InvariantCulture, out value);

            if (!correctCommand)
            {
                // todo, log error and handle correctly
                return;
            }

            // Looking at the command and switching what behaviour is required
            switch(value.ToString())
            {

                case "message":
                    HandleMessageCommand(jobject);
                    break;
                default:
                    // DataManager does not need the command, sending to all others
                    this.SendToManagers(jobject);
                    break;


            }
        }

        private void HandleMessageCommand(JObject jobject)
        {
            //TODO try get value instead of getvalue
            // all message object are required to have flag attribute.
            int flag = (int)jobject.GetValue("flag");

            switch(flag)
            {
                case 0:
                case 1:
                    // TODO flags needed for login, net yet needed
                    break;
                case 2:
                    this.SendToManagers(jobject);
                    // Sending the data to the vrmanager, since flag 2 needs to be show in vr
                    break;
                case 3:
                default:
                    Trace.WriteLine($"Error received from server{jobject.GetValue("data")}");
                    break;

            }
        }

        public override void ReceivedData(JObject data)
        {
            // The server will only get messages to login, all other is not defined in the data protocol
            Trace.WriteLine($"received data from server: {data}");
            this.TCPClientHandler.WriteMessage(data.ToString());
        }
    }
}