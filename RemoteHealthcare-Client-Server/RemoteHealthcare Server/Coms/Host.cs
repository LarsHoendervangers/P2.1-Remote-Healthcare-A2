using CommClass;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RemoteHealthcare_Server.Data;
using RemoteHealthcare_Server.Data.User;
using RemoteHealthcare_Shared;
using System;
using System.Net.Sockets;
using System.Threading;

namespace RemoteHealthcare_Server
{
    public delegate void Notify(Host host);

    public class Host
    {
        //Needed for assigment
        public TcpClient tcpclient;
        private readonly ISender sender;
        private readonly UserManagement usermanagement;
        private readonly JSONReader reader;

        //Only assign 
        IUser user;

        private bool stop = false;
        public event Notify Disconnecting;

        /// <summary>
        /// Constructor for the session
        /// </summary>
        /// <param name="client">Is the client or doctor app</param>
        /// <param name="management">Is the list that is used</param>
        public Host(TcpClient client, UserManagement management)
        {
            //Objects needed
            this.sender = new EncryptedServer(client.GetStream());
            this.usermanagement = management;
            this.tcpclient = client;
            this.reader = new JSONReader();
            this.reader.CallBack += ChangeUser;

            this.Disconnecting += Stop;

            //Starting reading thread
            new Thread(ReadData).Start();
        }

        /// <summary>
        /// Reading loop for the session
        /// </summary>
        public void ReadData()
        {
            while (!this.stop)
            {
                //Getting json object
                try
                {
                    string data = sender.ReadMessage();
                    if (data.Length == 0) break;
                    JObject json = (JObject)JsonConvert.DeserializeObject(data);

                    //Reading json object
                    this.reader.DecodeJsonObject(json, this.sender, this.user, this.usermanagement);
                } catch (Exception)
                {
                    break;
                }
            }
            FireDisconnectingEvent();
        }

        /// <summary>
        /// Shutting down for user
        /// </summary>
        public void Stop(Host host)
        {
            this.stop = true;
            this.usermanagement.SessionEnd(user);
            this.usermanagement.activeHosts.Remove(this);
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

        /// <summary>
        /// Fire the disconnecting event.
        /// </summary>
        public void FireDisconnectingEvent()
        {
            this.Disconnecting?.Invoke(this);
        }
    }
}