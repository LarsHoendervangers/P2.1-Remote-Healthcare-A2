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
        private readonly UserManagement usermanagement;
        private readonly JSONReader reader;

        //Only assign 
        IUser user;

        /// <summary>
        /// Constructor for the session
        /// </summary>
        /// <param name="client">Is the client or doctor app</param>
        /// <param name="management">Is the list that is used</param>
        public Host(TcpClient client, UserManagement management)
        {
            //Objects needed
            this.sender = new PlaneTextSender(client.GetStream());
            this.usermanagement = management;
            this.tcpclient = client;
            this.reader = new JSONReader();
            this.reader.CallBack += ChangeUser;

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

        /// <summary>
        /// Shutting down for user
        /// </summary>
        public void Stop()
        {
            this.tcpclient.Close();
        }


        /// <summary>
        /// Callback for IUSer object
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChangeUser(object sender, IUser e)
        {
            this.user = e;
        }

        public IUser GetUser()
        {
            return this.user;
        }

        public ISender GetSender()
        {
            return this.sender;
        }
    }
       

   
}