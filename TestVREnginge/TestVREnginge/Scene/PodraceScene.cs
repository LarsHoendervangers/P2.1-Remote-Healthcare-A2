using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics;
using TestVREngine.Tunnel;
using TestVREngine.Util;
using TestVREngine.Util.Structs;


namespace TestVREngine.Scene
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

        }

        /// <summary>
        /// Loading 3dObjects for the PodraceScene
        /// </summary>
        public override void LoadScene()
        {
            //Spawning podracer
            Handler.SendToTunnel(JSONCommandHelper.Wrap3DObject("podracer", "data/NetworkEngine/models/podracer/podracer.obj"), (string message) => this.uuidModel = VRUTil.GetId(message));

            //Spawning map
            //Handler.SendToTunnel(JSONCommandHelper.Wrap3DObject("raceterrain", "data/NetworkEngine/models/podracemap1/podracermap.obj"));
            Handler.SendToTunnel(JSONCommandHelper.Wrap3DObject("raceterrain", "data/NetworkEngine/models/podracemap1/podracemap1.obj", new Transform(1, new int[3] { 200, -2, 50 }, new int[3] { 0, 0, 0 })));

            //Creating terrain
            Debug.WriteLine(CreateTerrain());

            //Adding route
            AddRoute();

            //Adding road
            AddRoad();

            //Letting podracer follow the route
            MoveModelOverRoad();
        }

        /// <summary>
        /// Creating a terrain using simplex noise
        /// </summary>
        /// <returns></returns>
        private string CreateTerrain()
        {
            float[] height = VRUTil.GenerateTerrain(256, 256, 3, 0.01f);

            this.Handler.SendToTunnel(JSONCommandHelper.WrapTerrain(new int[] { 256, 256 }, height));
            this.Handler.SendToTunnel(JSONCommandHelper.WrapShowTerrain("ground", new Transform(1, new int[3] { -128, 0, -128 }, new int[3] { 0, 0, 0 })));

            return "Created a new terrain with size: 256 x 256.";
        }

        private string AddRoute()
        {
            PosVector[] posVectors = new PosVector[]
            {
            new PosVector(new int[]{-22,0,40 }, new int[]{5,0,5}),
            new PosVector(new int[]{0,0,62}, new int[]{5,0,5}),
            new PosVector(new int[]{42,0, 63}, new int[]{5,0,-5}),
            new PosVector(new int[]{65,0,42 }, new int[]{5,0,-5}),
            new PosVector(new int[]{75,0,10 }, new int[]{5,0,-5}),
            new PosVector(new int[]{63,0,-30 }, new int[]{-5,0,5}),
            new PosVector(new int[]{20,0,-40 }, new int[]{-5,0,-5}),
            new PosVector(new int[]{-10,0,-30 }, new int[]{-5,0,5}),
            new PosVector(new int[]{-25,0,-5 }, new int[]{-5,0,5})
        };

            Handler.SendToTunnel(JSONCommandHelper.WrapAddRoute(posVectors),(string message) => this.uuidRoute = VRUTil.GetId(message));
            return "Added a route.";
        }
        
        private string MoveModelOverRoad()
        {
            Handler.SendToTunnel(JSONCommandHelper.WrapFollow(uuidRoute, uuidModel));
            return "The bike is now moving over the route.";
        }

        private string AddRoad()
        {
            Handler.SendToTunnel(JSONCommandHelper.WrapAddRouteTerrain(uuidRoute));
            return "Added a road to the previous route.";
        }
    }
}
