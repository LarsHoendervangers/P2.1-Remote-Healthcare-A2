using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RemoteHealthcare_Client
{
    /// <summary>
    /// Abstract class that handles the abstration layer above all the different data sources
    /// </summary>
    public abstract class DataManager
    {
        public IList<DataManager> NetworkManagers { get; set; }

        /// <summary>
        /// Constructor for the DataManager
        /// </summary>
        protected DataManager()
        {
            NetworkManagers = new List<DataManager>();
        }

        /// <summary>
        /// Method that handles the receiving of data from one DataManager to an other
        /// </summary>
        /// <param name="data">The data send to the manager</param>
        public abstract void ReceivedData(JObject data);

        /// <summary>
        /// Sends the given data to all the other dataManagers
        /// </summary>
        /// <param name="data">The data to be send</param>
        protected void SendToManagers(JObject data)
        {
            foreach(DataManager manager in NetworkManagers)
            {
                manager.ReceivedData(data);
            }
        }
    }
}