using RemoteHealthcare.ClientVREngine.Util;
using RemoteHealthcare_Client.ClientVREngine.Tunnel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using RemoteHealthcare.ClientVREngine.Util.Structs;

namespace RemoteHealthcare_Client.ClientVREngine.Scene
{
    /// <summary>
    /// Abstract class the represents a Scene
    /// Scenes can:
    /// <ul>
    ///     <li>Be inititalized</li>
    ///     <li>Be loaded to the server</li>
    ///     <li>Be saved to the server</li>
    /// </ul>
    /// </summary>
    public abstract class GeneralScene
    {
        protected TunnelHandler Handler;
        protected string uuidRoute;
        protected string uuidCamera;
        protected string uuidBike;
        protected string uuidTextPanel;
        protected string uuidDataPanel;
        protected string uuidSusan;
        protected string uuidTerrain;
        private string text;
        private List<string> messagetext = new List<string>();

        /// <summary>
        /// Constructor for GeneralScene
        /// </summary>
        /// <param name="handler">The TunnelHandler that the scene needs to communicate to the server with</param>
        protected GeneralScene(TunnelHandler handler)
        {
            Handler = handler;
        }

        /// <summary>
        /// Inits the scene, all action needed to start are preformed
        /// </summary>
        public abstract void InitScene();

        /// <summary>
        /// Loads the scene in the server
        /// </summary>
        public abstract void LoadScene();

        /// <summary>
        /// Save the current scene on the server, given the filename
        /// </summary>
        /// <param name="fileName">The filename to store the scene to</param>
        public virtual void SaveScene(string fileName)
        {
            Handler.SendToTunnel(JSONCommandHelper.WrapSaveScene(fileName), OnSaveCallback);
        }

        /// <summary>
        /// Callback fucntion called to handle the save response from the server
        /// </summary>
        /// <param name="message">The message received from the server</param>
        private void OnSaveCallback(string message)
        {
            //TODO handle error from server to user
            Trace.WriteLine($"Scene: save command returned from server: {message} \n");
            Console.WriteLine("Scene save command returned from server");
        }


        public void RemoveGroundPlaneCallback(string jsonString)
        {
            JObject jObject = JObject.Parse(jsonString);
            JArray array = (JArray)jObject.SelectToken("data.data.data.children");

            foreach (JObject o in array)
            {
                Trace.WriteLine($"DemoScene: object name = {o.GetValue("name")} \n");
                if (o.GetValue("name").ToString() == "GroundPlane")
                {
                    Handler.SendToTunnel(JSONCommandHelper.RemoveNode(o.GetValue("uuid").ToString()));

                    return;

                }
            }
        }


        //Attaches the camera and susan to the bike by getting there id and attaching it to the parent bike
        public void AttachCameraToBike(string jsonString)
        {
            
            JObject jObject = JObject.Parse(jsonString);
            JArray array = (JArray)jObject.SelectToken("data.data.data.children");

            foreach (JObject o in array)
            {
                Trace.WriteLine($"DemoScene: object name = {o.GetValue("name")} \n");
                if (o.GetValue("name").ToString() == "Camera")
                {
                    uuidCamera = o.GetValue("uuid").ToString();
                    Handler.SendToTunnel(JSONCommandHelper.UpdateNodeCamera(o.GetValue("uuid").ToString(), uuidBike, new Transform(1, new double[] { 0, 0, 0 }, new double[] { 90, 0, 90 })));

                } else if (o.GetValue("name").ToString() == "Head")
                {
                    uuidSusan = o.GetValue("uuid").ToString();
                }

            }
        }

        //Writes bike data to a panel
        public void WriteDataToPanel(JObject BikeData)
        {
            if (uuidDataPanel != null)
            {

                Handler.SendToTunnel(JSONCommandHelper.WrapPanelClear(uuidDataPanel));
                Handler.SendToTunnel(
                    JSONCommandHelper.WrapPanelText(uuidDataPanel, $"Speed: {BikeData.SelectToken("data.speed")}\\nRPM: {BikeData.SelectToken("data.rpm")}\\nDistance: {BikeData.SelectToken("data.dist")}\\nPower: {BikeData.SelectToken("data.pow")}\\nTotal Power: {BikeData.SelectToken("data.accpow")}\\nBPM: {BikeData.SelectToken("data.bpm")}\\n",
                        new double[] { 25.0, 25.0 }, 25.0, "Arial")
                );
                Handler.SendToTunnel(JSONCommandHelper.WrapPanelSwap(uuidDataPanel));
                UpdateBikeSpeed(BikeData);
            }

        }

        //updates the speed of the bike in vr to the speed of the bike IRL 
        public void UpdateBikeSpeed(JObject speedData)
        {
            string speed = $"{speedData.SelectToken("data.speed")}";
            if (speed != "")
            {
                double speedDouble = Convert.ToDouble(speed) / 3.6;
                Handler.SendToTunnel(JSONCommandHelper.WrapUpdateFollow(uuidBike, speedDouble));
            }
        }

        //Writes Text messages from the doctor to a panel
        public void WriteTextToPanel(string message)
        {
            if (uuidTextPanel != null)
            {

                Handler.SendToTunnel(JSONCommandHelper.WrapPanelClear(uuidTextPanel));
                Handler.SendToTunnel(
                    JSONCommandHelper.WrapPanelText(uuidTextPanel, message,
                        new double[] { 25.0, 25.0 }, 25.0, "Arial")
                );
                Handler.SendToTunnel(JSONCommandHelper.WrapPanelSwap(uuidTextPanel));

            }

        }

        //structures the text send by the doctor. It deletes the earliest messages after the max lines are reached and puts long messages on multiple lines.
        public string HandelTextMessages(int maxTotalLines, int maxCharPerLine, JObject data)
        {
            text = "";
            string message = data.GetValue("data").ToString();
            if (message != "")
            {
                if (messagetext.Count < maxTotalLines && message.Length < maxCharPerLine && message.Length < (maxTotalLines * maxCharPerLine))
                {
                    messagetext.Add(message + "\\n");
                }
                else if (message.Length >= maxCharPerLine && message.Length <= ((maxTotalLines * maxCharPerLine) * 2))
                {
                    for (int i = 0; i < message.Length; i += maxCharPerLine)
                    {

                        if ((((message.Length + maxCharPerLine) / maxCharPerLine) + messagetext.Count) > (maxTotalLines * 2))
                        {
                            for (int j = 0; j < messagetext.Count; j++)
                            {
                                if (messagetext[0].EndsWith("\\n"))
                                {
                                    messagetext.RemoveAt(0);
                                    break;
                                }
                                else
                                {
                                    messagetext.RemoveAt(0);
                                }
                            }

                        }
                        if (((i + maxCharPerLine) / maxCharPerLine) != (message.Length / maxCharPerLine) + 1)
                        {

                            messagetext.Add(message.Substring(i, maxCharPerLine) + "-");
                        }
                        else
                        {

                            messagetext.Add(message.Substring(i, (message.Length % maxCharPerLine)) + "\\n");
                        }
                    }
                }
                else if (messagetext.Count >= maxTotalLines && message.Length < maxCharPerLine && message.Length < (maxTotalLines * maxCharPerLine))
                {
                    for (int j = 0; j < messagetext.Count; j++)
                    {
                        if (messagetext[0].EndsWith("\\n"))
                        {
                            messagetext.RemoveAt(0);
                            break;
                        }
                        else
                        {
                            messagetext.RemoveAt(0);
                        }
                    }
                    messagetext.Add(message + "\\n");
                }

            }

            foreach (var Text in messagetext)
            {
                text += Text + "\\n";
            }

            return text;
        }

        public void CreateTerrain(string pathNormal, string pathDiffuse)
        {
            float[] height = new float[256 * 256];
            height = VRUTil.GenerateTerrain(256, 256, 3, 0.01f);
            Handler.SendToTunnel(JSONCommandHelper.WrapTerrain(new int[] { 256, 256 }, height));
            Handler.SendToTunnel(
                JSONCommandHelper.WrapShowTerrain("ground",
                    new Transform(1, new double[3] { -128, 0, -128 }, new double[3] { 0, 0, 0 })), (string message) => this.uuidTerrain = VRUTil.GetId(message));
            Thread.Sleep(2000);
            Handler.SendToTunnel(JSONCommandHelper.WrapAddTexture(uuidTerrain,
                pathNormal,
                pathDiffuse, 0, 3, 1));
            Handler.SendToTunnel(JSONCommandHelper.GetAllNodes(), new Action<string>(RemoveGroundPlaneCallback));

        }

        public void CreateRoute(PosVector[] posVectors , string pathDiffuse, string pathNormal, string pathSpecular)
        {
            Handler.SendToTunnel(JSONCommandHelper.WrapAddRoute(posVectors),
                (string message) => this.uuidRoute = VRUTil.GetId(message));
            Thread.Sleep(2000);
            Handler.SendToTunnel(JSONCommandHelper.WrapAddRouteTerrain(uuidRoute));
            Handler.SendToTunnel(JSONCommandHelper.WrapAddRouteTerrain(uuidRoute,pathDiffuse,pathDiffuse,pathSpecular));
        }

        public void CreateRoute(PosVector[] posVectors)
        {
            Handler.SendToTunnel(JSONCommandHelper.WrapAddRoute(posVectors),
                (string message) => this.uuidRoute = VRUTil.GetId(message));
            Thread.Sleep(2000);
            Handler.SendToTunnel(JSONCommandHelper.WrapAddRouteTerrain(uuidRoute));
        }

        public void CreateVechile(string modelPath , Transform transform)
        {
            Handler.SendToTunnel(
                JSONCommandHelper.Wrap3DObject("bike", modelPath,
                    transform), (string message) => this.uuidBike = VRUTil.GetId(message));
            Thread.Sleep(2000);
            Handler.SendToTunnel(JSONCommandHelper.GetAllNodes(), new Action<string>(AttachCameraToBike));
            Thread.Sleep(2000);
        }

        public void CreatePanels(string dataPanelParent, string textPanelParent, Transform transformDataPanel, Transform transformTextPanel)
        {
            Handler.SendToTunnel(
                JSONCommandHelper.WrapPanel("panelData", dataPanelParent, transformDataPanel,
                    0.5, 0.5, 512, 512, true), (string message) => this.uuidDataPanel = VRUTil.GetId(message));
            Handler.SendToTunnel(
                JSONCommandHelper.WrapPanel("panelText", textPanelParent, transformTextPanel,
                    0.5, 0.5, 512, 512, true), (string message) => this.uuidTextPanel = VRUTil.GetId(message));
            Thread.Sleep(2000);
            WriteTextToPanel("");
        }
    }

}
