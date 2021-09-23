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

        public TcpClient TcpClient { get; set; }

        public Host(string ID, Patient clientPatient, FileProcessing database, JSONReader jSONReader, TcpClient client)
        {
            this.ID = ID;
            this.ClientPatient = clientPatient;
            this.Database = database;
            this.JSONReader = jSONReader;
            this.TcpClient = client;
        }

        public void ReadData()
        {
            throw new System.NotImplementedException();
        }

        public void Stop()
        {
            this.TcpClient.Close();
        }
    }
}