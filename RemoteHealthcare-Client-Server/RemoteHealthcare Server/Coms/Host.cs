using CommClass;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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
        private readonly JSONReader reader;

        //Only assign 
        IUser user;

        /// <summary>
        /// Constructor for the session
        /// </summary>
        /// <param name="client">Is the client or doctor app</param>
        /// <param name="management">Is the list that is used</param>
        public Host(TcpClient client, Usermanagement management)
        {
            //Objects needed
            this.sender = new EncryptedServer(client.GetStream());
            this.usermanagement = management;
            this.tcpclient = client;
            this.reader = new JSONReader();

            //Starting reading thread
            new Thread(ReadData).Start();
        }

        /// <summary>
        /// Reading loop for the session
        /// </summary>
        public void ReadData()
        {
            while (true)
            {
                //Getting json object
                string data = sender.ReadMessage();
                JObject json = (JObject)JsonConvert.DeserializeObject(data);

                //Reading json object
                this.reader.DecodeJsonObject(json, this.sender, this.user, this.usermanagement);
            }
        }

        public void Stop()
        {
            this.tcpclient.Close();
        }
    }
       

   
}