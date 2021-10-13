using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RemoteHealthcare.ClientVREngine.Util;
using RemoteHealthcare.ClientVREngine.Util.Structs;
using RemoteHealthcare_Client.TCP;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RemoteHealthcare_Client.ClientVREngine.Tunnel
{

    /// <summary>
    /// Class that handles the communication between the vr-server <br>
    /// <ul>
    ///     <li>Sets up connection to the vr-tunnel</li>
    ///     <li>Sends given data</li>
    ///     <li>Class provides callback for return messages from the server</li>
    /// </ul>
    /// </summary>
    public class TunnelHandler
    {
        private string DestinationID;
        private readonly Dictionary<string, Action<string>> SerialMap;
        private readonly TCPClientHandler TcpHandler;
        private int SerialNumber;
        private JObject message;


        /// <summary>
        /// Constructor for Tunnelhandler
        /// </summary>
        public TunnelHandler()
        {
            SerialMap = new Dictionary<string, Action<string>>();
            TcpHandler = new TCPClientHandler("145.48.6.10", 6666, false);
            SerialNumber = 0;

            // Setting the method to be performed when data is received
            TcpHandler.OnMessageReceived += OnMessageReceived;
        }

        /// <summary>
        /// gives a list of all the available clients.
        /// </summary>
        /// <returns>List of all the clients</returns>
        public List<ClientData> GetAvailableClients()
        {
            List<ClientData> clients = new List<ClientData>();

            //Writing for connection 
            string startingCode = JsonConvert.SerializeObject(JSONCommandHelper.WrapRequest());
            TcpHandler.WriteMessage(startingCode);

            //Reading for clients
            string jsonString = TcpHandler.ReadMessage();
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


            //Returning the client list.
            return clients;
        }

        // TODO fix comments, returns no id but a bool
        /// <summary>
        /// sets up up the connection and returns the id.
        /// </summary>
        /// <param name="connectionID">The ID of the client to connect to</param>
        /// <returns>boolean if the connection succeeded</returns>
        public bool SetUpConnection(string connectionID)
        {
            //Sending tunneling request to vps
            string requestingCode = JsonConvert.SerializeObject(JSONCommandHelper.WrapTunnel(connectionID));
            Trace.WriteLine($"TunnelHandler: json to connect is {requestingCode} \n");
            TcpHandler.WriteMessage(requestingCode);

            //Receiving ok or error
            string jsonString = TcpHandler.ReadMessage();
            JObject jsonData = null;
            try
            {
                jsonData = (JObject) JsonConvert.DeserializeObject(jsonString);
            }
            catch (Exception e)
            {
                Debug.WriteLine("Tunnelhandler.SetupConnection: Failed to deserialize JSON object" + e.Message);
            }

            //verifying connection.
            if (jsonData == null)
            {
                //Errors have been thrown above
                return false;
            }

            JToken jsonFile;
            jsonData.TryGetValue("data", out jsonFile);
            JObject jsonFileObject = jsonFile as JObject;
            if (jsonFileObject == null)
            {
                Debug.WriteLine("Tunnelhandler.SetupConnection: Failed to cast JToken to JObject");
                return false;
            }

            JToken statusCode;

            if (jsonFileObject.TryGetValue("status", out statusCode))
            {
                switch (statusCode.ToString())
                {
                    case "ok":
                        //Getting destination
                        string id = jsonFileObject.GetValue("id").ToString();
                        DestinationID = id;

                        //Setting reader on.
                        TcpHandler.SetRunning(true);
                        return true;
                    case "error":
                        MessageBox.Show("Failed to connect to VrEngine", "Error");
                        return false;
                    default:
                        Debug.WriteLine("Tunnelhandler.SetupConnection: Received unknown message");
                        return false;
                }
            }
            Debug.WriteLine("Tunnelhandler.SetupConnection: No status message received");
            return false;

            //if (jsonFileObject.GetValue("status").ToString() != "ok") return false;
            //else
            //{
            //    //Getting destination
            //    string id = jsonFileObject.GetValue("id").ToString();
            //    DestinationID = id;

            //    //Setting reader on.
            //    TcpHandler.SetRunning(true);

            //    return true;
            //}
        }


        /// <summary>
        /// Sends a message to the server with a serial id.
        /// </summary>
        /// <param name="message">object as json that is send to the server</param>
        /// <param name="action">the method that is used as action.</param>
        public void SendToTunnel(object message, Action<string> action)
        {

            //Number for serialID
            SerialNumber += 1;
            string serial = SerialNumber.ToString();

            //Magic thing for adding a object
            JObject decode = JObject.FromObject(message);
            decode.Add("serial", SerialNumber);
            object encoded = decode.ToObject<object>();

            //Putting it in the hashmap
            SerialMap.Add(serial, action);

            //Sending the message.
            SendToTunnel(encoded);
        }

        /// <summary>
        /// Sends a message to the server without a id.
        /// </summary>
        /// <param name="message">Object as json that is send to the server</param> 
        public void SendToTunnel(object message)
        {
            object totalStream = JSONCommandHelper.WrapHeader(DestinationID, message);
            Trace.WriteLine($"TunnelHandler: Sending data to server: {JsonConvert.SerializeObject(totalStream)} \n");
            TcpHandler.WriteMessage(JsonConvert.SerializeObject(totalStream));
        }

        /// <summary>
        /// If a message is received than it will try to check what message serialnumber is and return it contents to a action delegate
        /// </summary>
        /// <param name="sender">The class that send the object</param> 
        /// <param name="input">The content that is send.</param> 
        private void OnMessageReceived(object sender, string input)
        {
            //Reading input
            if(!input.StartsWith("\0\0")){
                message = JsonConvert.DeserializeObject(input) as JObject;
            }

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
