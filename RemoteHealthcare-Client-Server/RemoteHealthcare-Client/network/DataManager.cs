using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RemoteHealthcare_Client
{
    public abstract class DataManager
    {
        public IList<DataManager> NetworkManagers { get; set; }

        protected DataManager()
        {
            NetworkManagers = new List<DataManager>();
        }

        public abstract void ReceivedData(JObject data);

        protected void SendToManagers(JObject data)
        {
            foreach(DataManager manager in NetworkManagers)
            {
                manager.ReceivedData(data);
            }
        }
    }
}