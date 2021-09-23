using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RemoteHealthcare_Client
{
    public class VRDataManager : IDataManager
    {
        internal ClientVREngine.Tunnel.TunnelHandler TunnelHandler
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

        public void ReceivedData(JObject data)
        {
            throw new NotImplementedException();
        }
    }
}