using CommClass;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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

        public IUSer client { get; set; }

        public FileProcessing Database { get; set; }

        public JSONReader JSONReader { get; set; }

        public TcpClient client { get; set; }

        EncryptedSender Sender { get; set; }

        

        public Host(string ID, Patient clientPatient, FileProcessing database, JSONReader jSONReader, TcpClient client)
        {

            //Patient settings
            this.ID = ID;
            this.ClientPatient = clientPatient;
            this.Database = database;
            this.JSONReader = jSONReader;
     

            //Connection settings
            new Thread(ReadData).Start();
            this.client = client;
            this.Sender = new EncryptedSender(client.GetStream());

        }

        public void ReadData()
        {
            while (true)
            {
                string data  = Sender.ReadMessage();
                JObject json = (JObject) JsonConvert.DeserializeObject(data);
                JSONReader.DecodeJsonObject(json, this.Sender);
                Trace.WriteLine(data);
                

            }
        }

        public void WriteData(String message)
        {
            Sender.SendMessage(message, this.client.GetStream());
        }

        public void Stop()
        {
            this.client.Close();
        }
    }
}