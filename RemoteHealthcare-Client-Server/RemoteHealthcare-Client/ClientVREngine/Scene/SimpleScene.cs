using System;
using System.Diagnostics;
using System.Threading;
using Newtonsoft.Json.Linq;
using RemoteHealthcare.ClientVREngine.Util;
using RemoteHealthcare.ClientVREngine.Util.Structs;
using RemoteHealthcare_Client.ClientVREngine.Tunnel;

namespace RemoteHealthcare_Client.ClientVREngine.Scene
{
    public class SimpleScene : GeneralScene
    {
        private string uuidRoute;
        private string uuidModel;
        private string uuidPanel;
        private string uuidCamera;

        public SimpleScene(TunnelHandler handler) : base(handler)
        {
        }

        public override void InitScene()
        {
            Handler.SendToTunnel(JSONCommandHelper.WrapReset());
            float[] height = new float[256 * 256];
            height = VRUTil.GenerateTerrain(256, 256, 3, 0.01f);
            Handler.SendToTunnel(JSONCommandHelper.WrapTerrain(new int[] {256, 256}, height));
            Handler.SendToTunnel(
                JSONCommandHelper.WrapShowTerrain("ground",
                    new Transform(1, new int[3] {-128, 0, -128}, new int[3] {0, 0, 0})),
                new Action<string>(Textureplacer));
            Handler.SendToTunnel(JSONCommandHelper.GetAllNodes(), new Action<string>(RemoveGroundPlaneCallback));
            
            Handler.SendToTunnel(
                JSONCommandHelper.Wrap3DObject("bike", "data/NetworkEngine/models/bike/bike.blend",
                    new Transform(1, new int[3] {0, 5, 0}, new int[3] {270, 270, 0})), new Action<string>(getIdModel));
            Thread.Sleep(2000);
            Handler.SendToTunnel(JSONCommandHelper.GetAllNodes(), new Action<string>(AttachCameraToBike));

            PosVector[] posVectors = new PosVector[]
            {
                new PosVector(new int[] {-22, 0, 40}, new int[] {5, 0, 5}),
                new PosVector(new int[] {0, 0, 62}, new int[] {5, 0, 5}),
                new PosVector(new int[] {42, 0, 63}, new int[] {5, 0, -5}),
                new PosVector(new int[] {65, 0, 42}, new int[] {5, 0, -5}),
                new PosVector(new int[] {75, 0, 10}, new int[] {5, 0, -5}),
                new PosVector(new int[] {63, 0, -30}, new int[] {-5, 0, -5}),
                new PosVector(new int[] {20, 0, -40}, new int[] {5, 0, 5}),
                new PosVector(new int[] {-10, 0, -30}, new int[] {-5, 0, 5}),
                new PosVector(new int[] {-25, 0, -5}, new int[] {-5, 0, 5})
            };
            Handler.SendToTunnel(JSONCommandHelper.WrapAddRoute(posVectors),
                (string message) => this.uuidRoute = VRUTil.GetId(message));

            Handler.SendToTunnel(JSONCommandHelper.WrapAddRouteTerrain(uuidRoute),
                new Action<string>(TextureplacerRoad));
            Thread.Sleep(2000);
            Handler.SendToTunnel(
                JSONCommandHelper.WrapPanel("panel", uuidModel, new Transform(1, new[] {-1, 0, 1}, new[] {90, 0, 90}),
                    1, 1, 512, 512, true), new Action<string>(getIdPanel));
            Thread.Sleep(2000);
            Handler.SendToTunnel(JSONCommandHelper.WrapFollow(uuidRoute, uuidModel));
           

        }

        public override void LoadScene()
        {

        }

        private void Textureplacer(string json)
        {
            Handler.SendToTunnel(JSONCommandHelper.WrapAddTexture(VRUTil.GetId(json),
                "data/NetworkEngine/textures/terrain/oilpt2_2K_Normal.jpg",
                "data/NetworkEngine/textures/terrain/oilpt2_2K_Albedo.jpg", 0, 3, 1));
        }

        private void TextureplacerRoad(string json)
        {
            Handler.SendToTunnel(JSONCommandHelper.WrapAddRouteTerrain(uuidRoute,
                "data/NetworkEngine/textures/terrain/vhwmdias_2K_Albedo.jpg",
                "data/NetworkEngine/textures/terrain/vhwmdias_2K_Normal.jpg",
                "data/NetworkEngine/textures/terrain/vhwmdias_2K_Roughness.jpg "));
        }

        private void Response(string json)
        {
            Trace.Write(json);
        }

        private void getIdModel(string json)
        {
            uuidModel = VRUTil.GetId(json);
        }

        private void getIdPanel(string json)
        {
            uuidPanel = VRUTil.GetId(json);
            Console.WriteLine("Panel" + uuidPanel);
        }

        public string getOrDefaultPanelUuid()
        {
            return this?.uuidPanel;
        }



        public void RemoveGroundPlaneCallback(string jsonString)
        {
            JObject jObject = JObject.Parse(jsonString);
            JArray array = (JArray) jObject.SelectToken("data.data.data.children");

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
                    Handler.SendToTunnel(JSONCommandHelper.UpdateNodeCamera(o.GetValue("uuid").ToString(),uuidModel,new Transform(1, new int[]{-3,0,0},new int[]{ 90, 0, 90 })));
                    

                }
                if(o.GetValue("name").ToString() == "Head")
                {
                    Handler.SendToTunnel(JSONCommandHelper.RemoveNode(o.GetValue("uuid").ToString()));
                }
            }
        }

        public void WriteTextToPanel(JObject BikeData)
        {
            if (uuidPanel != null)
            {
                
                Handler.SendToTunnel(JSONCommandHelper.WrapPanelClear(uuidPanel));
                Handler.SendToTunnel(
                    JSONCommandHelper.WrapPanelText(uuidPanel, $"Speed: {BikeData.SelectToken("data.speed")}\\nRPM: {BikeData.SelectToken("data.rpm")}\\nDistance: {BikeData.SelectToken("data.dist")}\\nPower: {BikeData.SelectToken("data.pow")}\\nTotal Power: {BikeData.SelectToken("data.accpow")}\\nBPM: {BikeData.SelectToken("data.bpm")}\\n",
                        new double[] {25.0, 25.0}, 25.0, "Arial")
                    );
                Handler.SendToTunnel(JSONCommandHelper.WrapPanelSwap(uuidPanel));
                
            }

        }
    }
}