using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestVREngine
{
    class BasicScene
    {
        private Queue<Func<string>> CommandList;
        private TunnelHandler Handler;
        //private TunnelHandler Handler;

        public BasicScene(TunnelHandler Handler)
        {
            this.CommandList = new Queue<Func<string>>();
            this.Handler = Handler;

            // Add methods to queue.
            this.CommandList.Enqueue(CreateTerrain);
            this.CommandList.Enqueue(RemoveGroundPlane);
            this.CommandList.Enqueue(ChangeTime);
            this.CommandList.Enqueue(AddModels);
            this.CommandList.Enqueue(ChangeTerrainHeight);
            this.CommandList.Enqueue(AddRoute);
            this.CommandList.Enqueue(AddRoad);
            this.CommandList.Enqueue(MoveModelOverRoad);
        }

        /// <summary>
        /// This method is called and will execute the next step in the exercise.
        /// </summary>
        public string ExecuteNext(int index)
        {
            return this.CommandList.Dequeue().ToString();
        }

        /// <summary>
        /// Step 1.
        /// </summary>
        private string CreateTerrain()
        {
            //this.Handler.SendToTunnel(JSONCommandHelper.WrapTerrain(new int[] { 256, 256 }));
            return "Created a new terrain with size: 256 x 256.";
        }

        /// <summary>
        /// Step 2.
        /// </summary>
        private string RemoveGroundPlane()
        {
            //this.Handler.SendToTunnel(JSONCommandHelper.WrapDeleteTerrain());
            return "Removed the terrain.";
        }

        /// <summary>
        /// Step 3.
        /// </summary>
        private string ChangeTime()
        {
            //this.Handler.SendToTunnel(JSONCommandHelper.WrapTime(5.5));
            return "Changed the time.";
        }

        /// <summary>
        /// Step 4.
        /// </summary>
        private string AddModels()
        {
            //this.Handler.SendToTunnel(JSONCommandHelper.Wrap3DObject("house", "data/NetworkEngine/models/houses/set1/house1.obj"));
            return "Spawned a house.";
        }

        /// <summary>
        /// Step 5.
        /// </summary>
        private string ChangeTerrainHeight()
        {
            //TODO: Maybe perlin noise generation?
            /*int[] terrainHeight = new int[256 * 256];
            for (int i = 0; i < terrainHeight.Length; i++)
            {
                terrainHeight[i] = i;
            }
            this.Handler.SendToTunnel(JSONCommandHelper.UpdateTerrainHeight(terrainHeight));*/
            return "Changed terrain height";
        }

        /// <summary>
        /// Step 6.
        /// </summary>
        private string AddRoute()
        {
            return "";
        }

        /// <summary>
        /// Step 7.
        /// </summary>
        private string AddRoad()
        {
            return "";
        }

        /// <summary>
        /// Step 8.
        /// </summary>
        private string MoveModelOverRoad()
        {
           // Handler.SendToTunnel(JSONCommandHelper.WrapFollow());
            return "";
        }
    }
}
