using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RemoteHealthcare.ClientVREngine.Util;
using RemoteHealthcare_Client.ClientVREngine.Scene;
using RemoteHealthcare_Client.ClientVREngine.Tunnel;
using System.Diagnostics;

namespace RemoteHealthcare_Client
{
    public class VRDataManager : DataManager
    {
        private readonly SimpleScene simpleScene;
        private readonly PodraceScene demoScene;

        public TunnelHandler VRTunnelHandler { get; set; }

        public VRDataManager()
        {
            VRTunnelHandler = new TunnelHandler();
            this.simpleScene = new SimpleScene(this.VRTunnelHandler);
            //this.demoScene = new PodraceScene(this.VRTunnelHandler);
        }

        public void Start(string vrServerID)
        {
            this.VRTunnelHandler.SetUpConnection(vrServerID);
            
            this.simpleScene.InitScene();

            this.simpleScene.LoadScene();
            //this.demoScene.InitScene();
            //this.demoScene.LoadScene();
        }

        public override void ReceivedData(JObject data)
        {
            // The data the VR engine will receive is the ergodata from the ergodevice + messagedata, see dataprotocol

            // command value always gives the action 
            JToken value;

            bool correctCommand = data.TryGetValue("command", StringComparison.InvariantCulture, out value);

            if (!correctCommand)
            {
                Trace.WriteLine("No valid JSON was received to VRDataManager");
                return;
            }

            // Looking at the command and switching what behaviour is required
            switch (value.ToString())
            {

                case "message":

                    string message = data.GetValue("data").ToString();
                    this.VRTunnelHandler.SendToTunnel(JSONCommandHelper.WrapPanelText(simpleScene.getOrDefaultPanelUuid(),
                        message, new double[] { 5, 5, 0 }, 10, "arial"));
                    break;
                case "ergodata":
                    Trace.WriteLine($"Ergo data received by vr engine{data.GetValue("data")}");
                    simpleScene.WriteTextToPanel(data);
                    break;
                default:
                    // TODO HANDLE NOT SUPPORTER
                    Trace.WriteLine("Error in VRDataManager, data received does not meet spec");
                    break;

            }
        }
    }
}