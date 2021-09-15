using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestVREnginge.TCP;

namespace TestVREnginge
{
    class TunnelHandler
    {
        private Dictionary<string, Action> SerialMap;
        private TCPClientHandler tcpHandler;

        public TunnelHandler()
        {
            this.SerialMap = new Dictionary<string, Action>();
            this.tcpHandler = new TCPClientHandler();

        }

        public List<ClientData> GetAvailableClients()
        {
            List<ClientData> clients = new List<ClientData>();

            //Writing for connection
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



        public void SetUpConnection(string id)
        {
            string requestingCode = "{\r\n\"id\" : \"tunnel/create\",\r\n\"data\" :\r\n	{\r\n\"session\" : \"" + id + "\"\r\n}\r\n}";
            Console.WriteLine(requestingCode);
        }



    }

    

}
