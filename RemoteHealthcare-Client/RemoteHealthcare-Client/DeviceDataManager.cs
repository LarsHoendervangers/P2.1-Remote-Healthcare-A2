using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RemoteHealthcare_Client
{
    public class DeviceDataManager : DataManager
    {
        public DataManager ServerDataManager { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public RemoteHealthcare_Client.DataManager VRDataManager
        {
            get => default;
            set
            {
            }
        }

        internal Ergometer.Software.Device Device
        {
            get => default;
            set
            {
            }
        }

        public void HandleIncoming(JObject data)
        {
            throw new NotImplementedException();
        }

        public void PrepareVRData(JObject data)
        {
            throw new NotImplementedException();
        }

        private void PrepareDeviceData(object sender, var data)
        {
            throw new System.NotImplementedException();
        }
    }
}