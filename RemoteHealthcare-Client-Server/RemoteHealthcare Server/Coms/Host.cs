using CommClass;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RemoteHealthcare_Server.Coms;
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
        private readonly JSONLogin login;


        //Only assign 
        int type = -1;
        object o = null;


        public Host(TcpClient client, Usermanagement management)
        {
            Debug.WriteLine("test");
            //Setting up attributes
            this.sender = new EncryptedSender(client.GetStream());
            this.usermanagement = management;
            this.tcpclient = client;
            this.login = new JSONLogin();

            //Starting reading thread
            new Thread(ReadData).Start();
        }

        public void ReadData()
        {
            while (true)
            {
                //Getting json object
                string data = sender.ReadMessage();
                JObject json = (JObject)JsonConvert.DeserializeObject(data);

                //Loging in or trying commanding...
                if (type == -1)
                {
                    this.login.LoginAction(json, sender, usermanagement);
                }
                else
                {
                    JSONReader.DecodeJsonObject(json, this.sender);

                }
            }
        }

        public void WriteData(string message)
        {
            sender.SendMessage(message);
        }

        public void Stop()
        {
            this.tcpclient.Close();
        }
    }
       

   
}