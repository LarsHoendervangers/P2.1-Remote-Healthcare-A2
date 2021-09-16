using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestVREngine.TCP;

namespace TestVREngine
{
    class TunnelHandler
    {
        public string destinationID;
        private Dictionary<string, Action<string>> SerialMap;
        private TCPClientHandler tcpHandler;

        public TunnelHandler()
        {
            this.SerialMap = new Dictionary<string, Action<string>>();
            this.tcpHandler = new TCPClientHandler();

            this.tcpHandler.OnMessageReceived += OnMessageReceived;

        }


        /// <summary>
        /// gives a list of all the available clients.
        /// </summary>
        /// <returns></returns>
        public List<ClientData> GetAvailableClients()
        {
            List<ClientData> clients = new List<ClientData>();

            //Writing for connection
            //TODO fixing hardcode json.
            string startingCode = "{\r\n\"id\" : \"session/list\"\r\n}";
            tcpHandler.WriteMessage(startingCode);

            //Reading for clients
            string jsonString = this.tcpHandler.ReadMessage();
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
            //TODO fixing hardcode json.
            string requestingCode = "{\r\n\"id\" : \"tunnel/create\",\r\n\"data\" :\r\n	{\r\n\"session\" : \"" + connection + "\"\r\n}\r\n}";
            this.tcpHandler.WriteMessage(requestingCode);

            //Receiving ok or error
            string jsonString = this.tcpHandler.ReadMessage();
            JObject jsonData = (JObject)JsonConvert.DeserializeObject(jsonString);

           
            JObject jsonFile = (JObject)jsonData.GetValue("data");
            if (jsonFile.GetValue("status").ToString() != "ok")
            {
                return false ;
            }
            else { 
                //Getting destination
                string id = jsonFile.GetValue("id").ToString();
                this.destinationID = id;

                //Setting reader on.
                this.tcpHandler.SetRunning(true);

                return true;
            }
        }

        private int serialNumber = 0;

        //TODO discuss if it should be JOBject or else
        public void SendToTunnel(object message, Action<string> action)
        {

            //Number for serialID
            this.serialNumber += 1;
            string serial = this.serialNumber.ToString();

            //Magic thing for adding a object
            JObject decode = JObject.FromObject(message);
            decode.Add("serial", serialNumber);
            object encoded = decode.ToObject<object>();

            //Putting it in the hashmap
            this.SerialMap.Add(serial, action);

            //Sending the message.
            SendToTunnel(encoded);
        }

        public void SendToTunnel(object message)
        {
            object totalStream = JSONCommandHelper.WrapHeader(this.destinationID, message);
            tcpHandler.WriteMessage(JsonConvert.SerializeObject(totalStream));
        }


        //Example function for controlling the appliction
        public void exampleFunction(string json)
        {

            this.tcpHandler.WriteMessage(json);
        }

        private void OnMessageReceived(object sender, string e)
        {
            //Reading serial ID
            JObject message = JsonConvert.DeserializeObject(e) as JObject;

      

            JToken data1;
            bool data1Check = message.TryGetValue("data", out data1);
            if (data1Check)
            {
                JToken data2;
                JObject data1object = data1 as JObject;
                bool data2check = data1object.TryGetValue("data", out data2);

                if (data2check)
                {
                    JToken serial;
                    JObject data2object = data2 as JObject;
                    bool serialCheck = data2object.TryGetValue("serial", out serial);
                    if (serialCheck)
                    {
                        string serialID = serial.ToString();
                        Console.WriteLine(serialID);
                        if (SerialMap.ContainsKey(serialID))
                        {
                            SerialMap[serialID].Invoke(e);
                        }
                    } else
                    {
                        Console.WriteLine("No ID found");

                    }

                }


            }


        }

    }

    

}
