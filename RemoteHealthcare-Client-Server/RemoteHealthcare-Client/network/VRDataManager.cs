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
        private string text;
        private List<string> messagetext = new List<string>();
        public TunnelHandler VRTunnelHandler { get; set; }

        public VRDataManager()
        {
            VRTunnelHandler = new TunnelHandler();
            this.simpleScene = new SimpleScene(this.VRTunnelHandler);
        }

        public void Start(string vrServerID)
        {
            this.VRTunnelHandler.SetUpConnection(vrServerID);

            this.simpleScene.InitScene();

            this.simpleScene.LoadScene();
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
                    text = "";
                    string message = data.GetValue("data").ToString();
                    if(message != "")
                    { 
                        if (messagetext.Count < 8 && message.Length < 15 && message.Length < 120)
                        {
                            messagetext.Add(message + "\\n");
                        }
                        else if(message.Length >= 15 && message.Length <= 240)
                        {
                            for (int i = 0; i < message.Length; i += 15)
                            {
                                if((((message.Length + 15) / 15) + messagetext.Count ) > 16)
                                {
                                    messagetext.RemoveAt(0);
                                }
                                if (((i + 15) / 15)  != (message.Length / 15) + 1)
                                {
                                    messagetext.Add(message.Substring(i,  15) + "-");
                                }
                                else
                                {
                                    messagetext.Add(message.Substring(i, (message.Length % 15)) + "-");
                                }
                            }
                        }
                        else if(messagetext.Count >= 8 && message.Length < 15 && message.Length < 120)
                        {
                            messagetext.RemoveAt(0);
                            messagetext.Add(message + "\\n");
                        }
                        else
                        {
                        
                        }
                    }
                   
                    foreach (var Text in messagetext)
                    {
                        text += Text + "\\n";
                    }
                    simpleScene.WriteTextToPanel(text);

                    break;
                case "ergodata":
                    Trace.WriteLine($"Ergo data received by vr engine{data.GetValue("data")}");
                    simpleScene.WriteDataToPanel(data);
                    break;
                default:
                    // TODO HANDLE NOT SUPPORTER
                    Trace.WriteLine("Error in VRDataManager, data received does not meet spec");
                    break;

            }
        }
    }
}