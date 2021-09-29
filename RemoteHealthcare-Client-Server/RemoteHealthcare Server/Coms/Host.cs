using CommClass;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RemoteHealthcare_Server.Coms;
using RemoteHealthcare_Server.Data;
using RemoteHealthcare_Server.Data.User;
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
        //Needed for assigment
        public TcpClient tcpclient;
        private readonly ISender sender;
        private readonly Usermanagement usermanagement;
        private readonly JSONLogin login;
        private readonly JSONReader reader;

        //Only assign 
        IUser user;


        public Host(TcpClient client, Usermanagement management)
        {
            //Setting up attributes
            //this.sender = new PlaneTextSender(client.GetStream());
            this.sender = new PlaneTextSender(client.GetStream());

            this.usermanagement = management;
            this.tcpclient = client;
            this.login = new JSONLogin();
            this.reader = new JSONReader();


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
                if (user == null)
                {
                    user = this.login.LoginAction(json, sender, usermanagement);
                }
                else
                {
                    Server.PrintToGUI(json.ToString());
                    this.reader.DecodeJsonObject(json, this.sender, this.user, this.usermanagement);
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