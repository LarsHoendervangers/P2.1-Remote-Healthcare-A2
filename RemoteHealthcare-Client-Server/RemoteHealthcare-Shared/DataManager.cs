using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace RemoteHealthcare_Client
{
    /// <summary>
    /// Abstract class that handles the abstraction layer above all the different data sources
    /// </summary>
    public abstract class DataManager
    {
        protected static readonly IList<DataManager> NetworkManagers = new List<DataManager>();

        /// <summary>
        /// Constructor for the DataManager
        /// </summary>
        protected DataManager()
        {
            DataManager.NetworkManagers.Add(this);
        }

        /// <summary>
        /// Method that handles the receiving of data from one DataManager to another.
        /// </summary>
        /// <param name="data">The data to send to the manager</param>
        public abstract void ReceivedData(JObject data);

        /// <summary>
        /// Sends the given data to all the other dataManagers
        /// </summary>
        /// <param name="data">The data to send</param>
        protected void SendToManagers(JObject data)
        {
            for (int i = 0; i < DataManager.NetworkManagers.Count; i++)
            {
                if (DataManager.NetworkManagers[i].Equals(this))
                    continue;
                
                DataManager.NetworkManagers[i].ReceivedData(data);
            }
        }
    }
}