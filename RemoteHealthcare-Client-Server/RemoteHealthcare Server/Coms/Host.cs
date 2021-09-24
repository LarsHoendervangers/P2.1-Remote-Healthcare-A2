using CommClass;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RemoteHealthcare_Server.Data;
using RemoteHealthcare_Server.Data.User;
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
        //Needed for assigment
        private readonly TcpClient tcpclient;
        private readonly EncryptedSender sender;
        private readonly Usermanagement usermanagement;


        //Assigable after login
        private Patient p; 
        private Doctor d;
        private Admin a;



        public Host(TcpClient client, Usermanagement management)
        {
            //Setting up attributes
            this.sender = new EncryptedSender(client.GetStream());
            this.usermanagement = management;
            this.tcpclient = client;

            //Starting reading thread
            new Thread(ReadData).Start();
        }

        public void ReadData()
        {
            while (true)
            {
                //Getting json object
                string data  = sender.ReadMessage();
                JObject json = (JObject) JsonConvert.DeserializeObject(data);

                //Getting type Object
                JSONReader.DecodeJsonObject(json, this.sender);
                Trace.WriteLine(data);
                

            }
        }

        public void WriteData(String message)
        {
            sender.SendMessage(message);
        }

        public void Stop()
        {
            this.tcpclient.Close();
        }
    }

   
}