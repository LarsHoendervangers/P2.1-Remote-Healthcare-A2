using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RemoteHealthcare_Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace RemoteHealthcare_Server
{
    public class Host
    {
        public string ID { get; set; }

        public Patient ClientPatient { get; set; }

        public FileProcessing Database { get; set; }

        public JSONReader JSONReader { get; set; }

        public TcpClient tcpClient { get; set; }

        public Host(string ID, Patient clientPatient, FileProcessing database, JSONReader jSONReader, TcpClient client)
        {
            this.ID = ID;
            this.ClientPatient = clientPatient;
            this.Database = database;
            this.JSONReader = jSONReader;
            this.tcpClient = client;
        }

        public void ReadData()
        {
            while (true)
            {
                string data  = ComClass.ReadMessage(this.tcpClient.GetStream());
                JObject json = (JObject) JsonConvert.DeserializeObject(data);

                

            }
            
        }

        public void Stop()
        {

        }

    }
}