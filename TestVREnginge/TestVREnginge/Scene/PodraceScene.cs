using System;
using System.Diagnostics;
using TestVREngine.Tunnel;
using TestVREngine.Util;
using TestVREngine.Util.Structs;


namespace TestVREngine.Scene
{
    class PodraceScene : GeneralScene
    {
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
            Handler.SendToTunnel(JSONCommandHelper.Wrap3DObject("podracer", "data/NetworkEngine/models/podracer/podracer.obj"));

            //Spawning map
            Handler.SendToTunnel(JSONCommandHelper.Wrap3DObject("raceterrain", "data/NetworkEngine/models/podracemap1/podracermap.obj"));

            //Creating terrain
            Debug.WriteLine(CreateTerrain());
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
    }
}
