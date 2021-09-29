using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RemoteHealthcare_Client
{
    public class ServerDataManager : DataManager
    {
        internal ClientVREngine.Tunnel.TCPClientHandler TCPClientHandler
        {
            get => default;
            set
            {
            }
        }

        public RemoteHealthcare_Client.DataManager DeviceDataManager
        {
            get => default;
            set
            {
            }
        }

        public RemoteHealthcare_Client.DataManager VRDataManager
        {
            get => default;
            set
            {
            }
        }

        public void PrepareVRData(JObject data)
        {
            throw new NotImplementedException();
        }

        public void HandleIncoming(JObject data)
        {
            throw new NotImplementedException();
        }

        public JObject WrapMessage(object message)
        {
            throw new System.NotImplementedException();
        }
    }
}