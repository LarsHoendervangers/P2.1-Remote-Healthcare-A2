using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RemoteHealthcare.ClientVREngine.Util;
using RemoteHealthcare_Client.ClientVREngine.Tunnel;

namespace RemoteHealthcare_Client
{
    public class VRDataManager : IDataManager
    {

        public TunnelHandler VRTunnelHandler { get; set; }

        public void HandleIncoming(JObject data)
        {
            string message = data.GetValue("data").ToString();
            //JSONCommandHelper.WrapPanelText()
        }

        public void PrepareVRData(JObject data)
        {
            throw new NotImplementedException();
        }

        public void ReceivedData(JObject data)
        {
            //Handle incoming data, because Jesse has autism
            HandleIncoming(data);
        }
    }
}