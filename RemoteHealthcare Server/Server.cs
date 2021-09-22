using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RemoteHealthcare_Server
{
    public class Server
    {
        /// <summary>
        /// List of all the clients connected to the server
        /// </summary>
        public List<Host> Clients
        {
            get; set;
        }

        public Host Host
        {
            get => default;
            set
            {
            }
        }

        public Host Host1
        {
            get => default;
            set
            {
            }
        }

        /// <summary>
        /// Metod which starts the server
        /// </summary>
        public void StartServer()
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Method which end the server
        /// </summary>
        public void StopServer()
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Method which is fired when client is connected
        /// </summary>
        public void OnConnect()
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Method which is fired when a client disconnects
        /// </summary>
        public void OnDisconnect()
        {
            throw new System.NotImplementedException();
        }
    }
}