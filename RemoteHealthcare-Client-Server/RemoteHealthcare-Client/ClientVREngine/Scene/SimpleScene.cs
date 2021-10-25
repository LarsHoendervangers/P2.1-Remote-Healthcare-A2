using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Mime;
using System.Threading;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RemoteHealthcare.ClientVREngine.Util;
using RemoteHealthcare.ClientVREngine.Util.Structs;
using RemoteHealthcare_Client.ClientVREngine.Tunnel;

namespace RemoteHealthcare_Client.ClientVREngine.Scene
{
    public class SimpleScene : GeneralScene
    {
        

        public SimpleScene(TunnelHandler handler) : base(handler)
        {
        }

        public override void InitScene()
        {
            Handler.SendToTunnel(JSONCommandHelper.WrapReset());
            CreateTerrain("data/NetworkEngine/textures/terrain/oilpt2_2K_Normal.jpg", "data/NetworkEngine/textures/terrain/oilpt2_2K_Albedo.jpg");
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
            }, "data/NetworkEngine/textures/terrain/vhwmdias_2K_Albedo.jpg",
                "data/NetworkEngine/textures/terrain/vhwmdias_2K_Normal.jpg",
                "data/NetworkEngine/textures/terrain/vhwmdias_2K_Roughness.jpg");

            CreateVechile("data/NetworkEngine/models/bike/bike.blend", new Transform(1, new double[3] { 0, 5, 0 }, new double[3] { 270, 270, 0 }), new Transform(1, new double[] { 0, 0, 0 }, new double[] { 90, 0, 90 }));
            
            CreatePanels(uuidSusan,uuidSusan, new Transform(1, new double[] { 0.1, -0.4, -0.25 }, new double[] { -45, 0, 0 }), new Transform(1, new double[] { -0.15, -0.4, -0.25 }, new double[] { -20, 45, 0 }));
            Handler.SendToTunnel(JSONCommandHelper.WrapFollow(uuidRoute, uuidBike, new double[] { 80, 0, 0 }));


        }

        public override void LoadScene()
        {
        }        
    }
}