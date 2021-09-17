using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestVREngine.Structs;
using TestVREngine.Tunnel.TCP;
using TestVREngine.Util;

namespace TestVREngine.Tunnel
{
    class TunnelHandler
    {
        public string destinationID;
        private Dictionary<string, Action<string>> SerialMap;
        private TCPClientHandler tcpHandler;
        private int serialNumber;

        public TunnelHandler()
        {
            SerialMap = new Dictionary<string, Action<string>>();
            tcpHandler = new TCPClientHandler();
            serialNumber = 0;

            tcpHandler.OnMessageReceived += OnMessageReceived;

        }

        /// <summary>
        /// gives a list of all the available clients.
        /// </summary>
        /// <returns></returns>
        public List<ClientData> GetAvailableClients()
        {
            List<ClientData> clients = new List<ClientData>();

            //Writing for connection
            string startingCode = JsonConvert.SerializeObject(JSONCommandHelper.WrapRequest());
            tcpHandler.WriteMessage(startingCode);

            //Reading for clients
            string jsonString = tcpHandler.ReadMessage();
            JObject jsonData = (JObject)JsonConvert.DeserializeObject(jsonString);

            //Adding it to the list
            JArray computers = (JArray)jsonData.GetValue("data");
            foreach (JObject objects in computers)
            {
                string id = objects.GetValue("id").ToString();
                JObject clientinfo = (JObject)objects.GetValue("clientinfo");
                string pcName = clientinfo.GetValue("host").ToString();
                string pcUser = clientinfo.GetValue("user").ToString();
                string pcGPU = clientinfo.GetValue("renderer").ToString();

                clients.Add(new ClientData(id, pcName, pcUser, pcGPU));
            }


            //Returning it.
            return clients;
        }


        /// <summary>
        /// setup up the connection and returns the id.
        /// </summary>
        /// <param name="connection"></param>
        /// <returns></returns>
        public bool SetUpConnection(string connection)
        {
            //Sending tunneling request to vps
            string requestingCode = JsonConvert.SerializeObject(JSONCommandHelper.WrapTunnel(connection));
            Console.WriteLine(requestingCode);
            tcpHandler.WriteMessage(requestingCode);

            //Receiving ok or error
            string jsonString = tcpHandler.ReadMessage();
            JObject jsonData = (JObject)JsonConvert.DeserializeObject(jsonString);

            //verifying connection.
            JObject jsonFile = (JObject)jsonData.GetValue("data");
            if (jsonFile.GetValue("status").ToString() != "ok") return false;
            else
            {
                //Getting destination
                string id = jsonFile.GetValue("id").ToString();
                destinationID = id;

                //Setting reader on.
                tcpHandler.SetRunning(true);

                return true;
            }
        }


        /// <summary>
        /// Sends a message to the server with a serial id.
        /// </summary>
        /// <param name="message"></param> object as json that is send to the server
        /// <param name="action"></param> the method that is used as action.
        public void SendToTunnel(object message, Action<string> action)
        {

            //Number for serialID
            serialNumber += 1;
            string serial = serialNumber.ToString();

            //Magic thing for adding a object
            JObject decode = JObject.FromObject(message);
            decode.Add("serial", serialNumber);
            object encoded = decode.ToObject<object>();

            //Putting it in the hashmap
            SerialMap.Add(serial, action);

            //Sending the message.
            SendToTunnel(encoded);
        }

        internal void SendToTunnel(object v, Action<string> action, object onRouteReceived)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Sends a message to the server without a id.
        /// </summary>
        /// <param name="message"></param> object as json that is send to the server
        public void SendToTunnel(object message)
        {
            object totalStream = JSONCommandHelper.WrapHeader(destinationID, message);
            Console.WriteLine(JsonConvert.SerializeObject(totalStream));
            tcpHandler.WriteMessage(JsonConvert.SerializeObject(totalStream));
        }

        /// <summary>
        /// If a message is received than it will try to check what message serialnumber is and return it contents to a action delegate
        /// </summary>
        /// <param name="sender"></param> is the class that send the object
        /// <param name="input"></param> is the content that is send.
        private void OnMessageReceived(object sender, string input)
        {
            //Reading input
            JObject message = JsonConvert.DeserializeObject(input) as JObject;

            //Check if serial exist if so then...
            JToken token = message.SelectToken("data.data.serial");
            if (token is null)
                return;

            //Sending to the action delegate if found and deleting it for memory usage..
            if (SerialMap.ContainsKey(token.ToString()))
            {
                SerialMap[token.ToString()].Invoke(input);
                SerialMap.Remove(token.ToString());
            }

        }
    }
}
