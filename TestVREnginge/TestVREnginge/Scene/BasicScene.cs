using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestVREngine.Tunnel;
using TestVREngine.Util;
using TestVREngine.Util.Structs;

namespace TestVREngine.Scene
{
    class BasicScene
    {
        private List<Func<string>> CommandList;
        private static TunnelHandler Handler;

        private string uuidRoute;
        private string uuidModel;

        /// <summary>
        /// Constructor for BasicScene
        /// </summary>
        /// <param name="HandlerIncoming">The TunnelHandler needed to send data to the server</param>
        public BasicScene(TunnelHandler HandlerIncoming)
        {
            CommandList = new List<Func<string>>();
            Handler = HandlerIncoming;

            // Add methods to queue.
            CommandList.Add(CreateTerrain);
            CommandList.Add(RemoveGroundPlane);
            CommandList.Add(ChangeTime);
            CommandList.Add(AddModels);
            CommandList.Add(AddRoute);
            CommandList.Add(AddRoad);
            CommandList.Add(MoveModelOverRoad);
        }

        /// <summary>
        ///  This method is called and will execute the next step in the exercise.
        /// </summary>
        /// <param name="index">The index of the command needed to be performed</param>
        /// <returns></returns>
        public string ExecuteNext(int index)
        {
            
            // check if index is available
            if (index < CommandList.Count)
            {
                return CommandList[index].Invoke();
            }
            else
            {
                return "There is nothing left to do.";
            }
        }

        /// <summary>
        /// Step 1. Create a new terrain with size: 256 x 256.
        /// </summary>
        /// <returns>Returns a string of the status of the step</returns>
        private string CreateTerrain()
        {
            Handler.SendToTunnel(JSONCommandHelper.WrapReset());
            float[] height = new float[256 * 256];

            


            Handler.SendToTunnel(JSONCommandHelper.WrapTerrain(new int[] { 256, 256 }, VRUTil.GenerateTerrain(256, 256, 3, 0.01f)));

            Handler.SendToTunnel(JSONCommandHelper.WrapShowTerrain("ground", new Transform(1, new int[3] { -128, 0, -128 }, new int[3] { 0, 0, 0 })), new Action<string>(Textureplacer));

            Handler.SendToTunnel(JSONCommandHelper.Wrap3DObject("raceterrain", "data/NetworkEngine/models/podracemap1/podracemap1.obj", new Transform(1, new int[3] { 200, -2,50 }, new int[3] { 0, 0, 0 })));

            return "Created a new terrain with size: 256 x 256.";
        }

        /// <summary>
        /// Step 2. Remove the terrain.
        /// </summary>
        /// <returns>Returns a string of the status of the step</returns>
        public string RemoveGroundPlane()
        {
            // asks the server for a list of all the commands
            Handler.SendToTunnel(JSONCommandHelper.GetAllNodes(), new Action<string>(RemoveGroundPlaneCallback));

            return "Removed the terrain.";
        }

        /// <summary>
        /// Callback method for step 2, contineus this step
        /// </summary>
        /// <param name="jsonString">The JSON command that is given back from the server</param>
        public void RemoveGroundPlaneCallback(string jsonString)
        {
            JObject jObject = JObject.Parse(jsonString);
            JArray array = (JArray)jObject.SelectToken("data.data.data.children");

            foreach (JObject o in array)
            {
                Console.WriteLine(o.GetValue("name"));
                if (o.GetValue("name").ToString() == "GroundPlane")
                {
                    Handler.SendToTunnel(JSONCommandHelper.RemoveNode(o.GetValue("uuid").ToString()));
                    return;
                }
            }
        }

        /// <summary>
        /// Step 3. Change the time to 5:30.
        /// </summary>
        /// <returns>Returns a string of the status of the step</returns>
        private string ChangeTime()
        {
            Handler.SendToTunnel(JSONCommandHelper.WrapTime(5.5));

            return "Changed the time.";
        }

        /// <summary>
        /// Step 4. Place a new house.
        /// </summary>
        /// <returns>Returns a string of the status of the step</returns>
        private string AddModels()
        {
            //Set time back to mid-day
            Handler.SendToTunnel(JSONCommandHelper.WrapTime(12));

            //Normal bike rotation (270, 270, 0).
            Handler.SendToTunnel(JSONCommandHelper.Wrap3DObject("bike", "data/NetworkEngine/models/podracer/podracer.obj", new Transform(1, new int[3] { 0, 15, 0 }, new int[3] { 0, 0, 0 })), new Action<string>(OnObjectCallback));
            return "Spawned a bike.";
            // this.Handler.SendToTunnel(JSONCommandHelper.Wrap3DObject("podracer", "data/NetworkEngine/models/podracer/podracer.obj", new Transform(1 , new int[3] { 0, 0, 0}, new int[3] { 0, 0, 0 })));
            //  return "Spawned a podracer.";
        }


        /// <summary>
        /// Callback method for when the a message from the server comes back
        /// </summary>
        /// <param name="message">The message send from the server</param>
        private void OnObjectCallback(string message)
        {
            JObject jObject = JObject.Parse(message);
            JObject id = (JObject)jObject.SelectToken("data.data.data");

            string idValue = id.GetValue("uuid").ToString();
            uuidModel = idValue;
        }


        /// <summary>
        /// Step 6. Create a new route.
        /// </summary>
        /// <returns>Returns a string of the status of the step</returns>
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

            Handler.SendToTunnel(JSONCommandHelper.WrapAddRoute(posVectors), new Action<string>(OnRouteReceived));
            return "Added a route.";
        }

        /// <summary>
        /// Callback method for when the a message from the server comes back
        /// </summary>
        /// <param name="message">The message send from the server</param>
        private void OnRouteReceived(string message)
        {
            JObject jObject = JObject.Parse(message);
            JObject id = (JObject)jObject.SelectToken("data.data.data");

            string idValue = id.GetValue("uuid").ToString();
            uuidRoute = idValue;
            Console.WriteLine(idValue);
        }

        /// <summary>
        /// Step 7. Add a road to the previous route.
        /// </summary>
        /// <returns>Returns a string of the status of the step</returns>
        private string AddRoad()
        {
            Handler.SendToTunnel(JSONCommandHelper.WrapAddRouteTerrain(uuidRoute, "data/NetworkEngine/textures/terrain/uc0lbi0ew_4K_Albedo.jpg", "data/NetworkEngine/textures/terrain/uc0lbi0ew_4K_Normal.jpg", "data/NetworkEngine/textures/terrain/uc0lbi0ew_4K_Roughness.jpg"));
            //"data/NetworkEngine/textures/terrain/uc0lbi0ew_4K_Albedo.jpg", "data/NetworkEngine/textures/terrain/uc0lbi0ew_4K_Normal.jpg", "data/NetworkEngine/textures/terrain/uc0lbi0ew_4K_Roughness.jpg"
            return "Added a road to the previous route.";
        }

        /// <summary>
        /// Step 8. Move a model over the route.
        /// </summary>
        /// <returns>Returns a string of the status of the step</returns>
        private string MoveModelOverRoad()
        {
            Handler.SendToTunnel(JSONCommandHelper.WrapFollow(uuidRoute, uuidModel));
            return "The bike is now moving over the route.";
        }

        /// <summary>
        /// Give the groundplane a ground texture
        /// </summary>
        /// <param name="json">The json</param>
        private static void Textureplacer(string json)
        {

            Handler.SendToTunnel(JSONCommandHelper.WrapAddTexture(VRUTil.GetId(json), "data/NetworkEngine/textures/terrain/uc0lbi0ew_4K_Normal.jpg", "data/NetworkEngine/textures/terrain/uc0lbi0ew_4K_Albedo.jpg", 0, 3, 1));
        }
    }


}
