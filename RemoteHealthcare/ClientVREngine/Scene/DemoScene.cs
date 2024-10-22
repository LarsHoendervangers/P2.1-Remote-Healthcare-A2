﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RemoteHealthcare.ClientVREngine.Tunnel;
using RemoteHealthcare.ClientVREngine.Util;
using RemoteHealthcare.ClientVREngine.Util.Structs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoteHealthcare.ClientVREngine.Scene
{
    /// <summary>
    /// Scene class that demo's all the parts of the school assignment
    /// </summary>
    class DemoScene : GeneralScene
    {
        private List<Func<string>> CommandList;

        private string uuidRoute;
        private string uuidModel;

        /// <summary>
        /// Constructor for BasicScene
        /// </summary>
        /// <param name="HandlerIncoming">The TunnelHandler needed to send data to the server</param>
        public DemoScene(TunnelHandler HandlerIncoming) : base(HandlerIncoming) { }

        public override void InitScene()
        {
            CommandList = new List<Func<string>>();

            // Add methods to queue.
            CommandList.Add(CreateTerrain);
            CommandList.Add(RemoveGroundPlane);
            CommandList.Add(ChangeTime);
            CommandList.Add(AddModels);
            CommandList.Add(AddRoute);
            CommandList.Add(AddRoad);
            CommandList.Add(MoveModelOverRoad);
            CommandList.Add(Reset);
        }

        public override void LoadScene()
        {
            // REMEMBER Method takes in user data and is not on seperated thread
            // Not an issue for this demo

            Console.WriteLine(
                "\t--------------------------------" + "\n" +
                "\t           DEMO SCENE           " + "\n" +
                "\t  press a key to load the next  " + "\n" +
                "\t        part of this demo       " + "\n" +
                "\t--------------------------------" + "\n"
                );

            //Loop which calls a method from the BasicScene class and starts the corresponding activity from teh list
            for (int i = 0; i < CommandList.Count; i++)
            {
                string stepResponse = ExecuteNext(i);
                Console.WriteLine($"step {i + 1}: {stepResponse}");

                Trace.WriteLine($"DemoScene: method step number {stepResponse} called \n");

                Console.ReadKey();
            }

            Console.WriteLine("All methods have been executed...");
            Trace.WriteLine("DemoScene: All methods have been executed \n");
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
            float[] height = new float[256 * 256];

            height = VRUTil.GenerateTerrain(256, 256, 3, 0.01f);


            Handler.SendToTunnel(JSONCommandHelper.WrapTerrain(new int[] { 256, 256 }, height));

            Handler.SendToTunnel(JSONCommandHelper.WrapShowTerrain("ground", new Transform(1, new int[3] { -128, 0, -128 }, new int[3] { 0, 0, 0 })), new Action<string>(Textureplacer));



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
                Trace.WriteLine($"DemoScene: object name = {o.GetValue("name")} \n");
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
            Handler.SendToTunnel(JSONCommandHelper.WrapTime(14.5));

            //Normal bike rotation (270, 270, 0).
            // Ask the server to add the bike object, uuid of model is saved in this.uuidModel using lambda expression
            Handler.SendToTunnel(JSONCommandHelper.Wrap3DObject("bike", "data/NetworkEngine/models/bike/bike.blend", new Transform(1, new int[3] { 0, 5, 0 }, new int[3] { 270, 270, 0 })), (message) => uuidModel = VRUTil.GetId(message));
            return "Spawned a bike.";
            // this.Handler.SendToTunnel(JSONCommandHelper.Wrap3DObject("podracer", "data/NetworkEngine/models/podracer/podracer.obj", new Transform(1 , new int[3] { 0, 0, 0}, new int[3] { 0, 0, 0 })));
            //  return "Spawned a podracer.";
        }


        /// <summary>
        /// Step 6. Create a new route.
        /// </summary>
        /// <returns>Returns a string of the status of the step</returns>
        private string AddRoute()
        {

            PosVector[] posVectors = new PosVector[]
            {
            new PosVector(new int[]{0,0,0 }, new int[]{5,0,-5}),
            new PosVector(new int[]{20,0,0 }, new int[]{5,0,5}),
            new PosVector(new int[]{20,0,20 }, new int[]{-5,0,5}),
            new PosVector(new int[]{0,0,20 }, new int[]{-5,0,-5}),
        };

            // Ask the server to add a route, uuid of route is saved in this.uuidModel using lambda expression
            Handler.SendToTunnel(JSONCommandHelper.WrapAddRoute(posVectors), (message) => uuidRoute = VRUTil.GetId(message));
            return "Added a route.";
        }

        /// <summary>
        /// Step 7. Add a road to the previous route.
        /// </summary>
        /// <returns>Returns a string of the status of the step</returns>
        private string AddRoad()
        {
            Handler.SendToTunnel(JSONCommandHelper.WrapAddRouteTerrain(uuidRoute));
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
        private void Textureplacer(string json)
        {

            Handler.SendToTunnel(JSONCommandHelper.WrapAddTexture(VRUTil.GetId(json), "data/NetworkEngine/textures/tarmac_normal.png", "data/NetworkEngine/textures/tarmac_diffuse.png", 0, 3, 1));
        }

        private string Reset()
        {
            Console.Write("Do you want to reset the scene? (y/n) ");
            string userCommand = Console.ReadLine();

            if (userCommand == "y")
            {
                Handler.SendToTunnel(JSONCommandHelper.WrapReset());

                return "reset the scene";
            }

            return "Not reseting the scene";
        }


    }


}
