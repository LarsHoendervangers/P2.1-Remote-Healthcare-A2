using Newtonsoft.Json.Linq;
using RemoteHealthcare.ClientVREngine.Util;
using RemoteHealthcare.ClientVREngine.Util.Structs;
using RemoteHealthcare_Client.ClientVREngine.Tunnel;
using System;
using System.Diagnostics;
using System.Threading;

namespace RemoteHealthcare_Client.ClientVREngine.Scene
{
    class PodraceScene : GeneralScene
    {
        private string uuidRoute;
        private string uuidModel;

        public PodraceScene(TunnelHandler handler) : base(handler)
        {
        }

        public override void InitScene()
        {
            Handler.SendToTunnel(JSONCommandHelper.WrapReset());
            CreateTerrain("data/NetworkEngine/textures/terrain/uc0lbi0ew_4K_Normal.jpg", "data/NetworkEngine/textures/terrain/uc0lbi0ew_4K_Normal.jpg");
           
            Handler.SendToTunnel(JSONCommandHelper.Wrap3DObject("mountain", "data/NetworkEngine/models/podracemap1/podracemap1.obj",new Transform(1,new double[]{ 200, -2, 50 },new double[]{0,0,0})));
            CreateRoute(new PosVector[]
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
                });
            CreateVechile("data/NetworkEngine/models/podracer/podracer.obj", new Transform(1, new double[] { 0, 15, 0 }, new double[] { 0, 0, 0 }));
            CreatePanels(uuidSusan, uuidSusan, new Transform(1, new double[] { 0.25, -0.25, -0.5 }, new double[] { 0, 0, 0 }), new Transform(1, new double[] { 0.25, 0.1, -0.5 }, new double[] { 0, 0, 0 }));
            Handler.SendToTunnel(JSONCommandHelper.WrapFollow(uuidRoute, uuidBike));
        }


        public override void LoadScene()
        {
           
        }

       
    }
}
