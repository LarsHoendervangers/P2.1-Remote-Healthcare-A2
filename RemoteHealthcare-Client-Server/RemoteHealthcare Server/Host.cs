using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RemoteHealthcare_Shared;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace RemoteHealthcare_Server
{
    public class Host
    {
        public string ID { get; set; }

        public Patient ClientPatient { get; set; }

        public FileProcessing Database { get; set; }

        public JSONReader JSONReader { get; set; }

        public TcpClient TcpClient { get; set; }

        public Host(string ID, Patient clientPatient, FileProcessing database, JSONReader jSONReader, TcpClient client)
        {
            this.ID = ID;
            this.ClientPatient = clientPatient;
            this.Database = database;
            this.JSONReader = jSONReader;
            this.TcpClient = client;
            new Thread(ReadData).Start();
        }

        public void ReadData()
        {
            while (true)
            {
                string data  = ComClass.ReadMessage(this.TcpClient.GetStream());
                //JObject json = (JObject) JsonConvert.DeserializeObject(data);
                Trace.WriteLine(data);
                

            }
        }

        public void WriteData(String message)
        {
            ComClass.WriteMessage(message, this.TcpClient.GetStream());
        }

        public void Stop()
        {
            this.TcpClient.Close();
        }
    }
}