using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RemoteHealthcare_Client
{
    public abstract class DataManager
    {

        public DataManager DeviceDataManager { get; set; }
        public DataManager VRDataManager { get; set; }
        public DataManager ServerDataManager { get; set; }

        public abstract void ReceivedData(JObject data);
    }
}