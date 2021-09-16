using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestVREngine
{
    class BasicScene
    {
        private List<Func<string>> CommandList;
        private TunnelHandler Handler;

        private string uuidRoute;
        private string uuidModel;

        public BasicScene(TunnelHandler Handler)
        {
            this.CommandList = new List<Func<string>>();
            this.Handler = Handler;

            // Add methods to queue.
            this.CommandList.Add(CreateTerrain);
            this.CommandList.Add(RemoveGroundPlane);
            this.CommandList.Add(ChangeTime);
            this.CommandList.Add(AddModels);
            this.CommandList.Add(ChangeTerrainHeight);
            this.CommandList.Add(AddRoute);
            this.CommandList.Add(AddRoad);
            this.CommandList.Add(MoveModelOverRoad);
        }

        /// <summary>
        /// This method is called and will execute the next step in the exercise.
        /// </summary>
        public string ExecuteNext(int index)
        {
            if (index < this.CommandList.Count) {
                return this.CommandList[index].ToString();
            } else
            {
                return "There is nothing left to do.";
            }
        }

        /// <summary>
        /// Step 1. Create a new terrain with size: 256 x 256.
        /// </summary>
        private string CreateTerrain()
        {
            //this.Handler.SendToTunnel(JSONCommandHelper.WrapTerrain(new int[] { 256, 256 }));
            return "Created a new terrain with size: 256 x 256.";
        }

        /// <summary>
        /// Step 2. Remove the terrain.
        /// </summary>
        private string RemoveGroundPlane()
        {
            //this.Handler.SendToTunnel(JSONCommandHelper.WrapDeleteTerrain());
            return "Removed the terrain.";
        }

        /// <summary>
        /// Step 3. Change the time to 5:30.
        /// </summary>
        private string ChangeTime()
        {
            //this.Handler.SendToTunnel(JSONCommandHelper.WrapTime(5.5));
            return "Changed the time.";
        }

        /// <summary>
        /// Step 4. Place a new house.
        /// </summary>
        private string AddModels()
        {
            //this.Handler.SendToTunnel(JSONCommandHelper.Wrap3DObject("bike", "data/NetworkEngine/models/bike/bike.fbx", new Transform(1 , new int[3] { 0, 0, 0}, new int[3] { 270, 0, 270 })), uuidModel);
            return "Spawned a house.";
        }

        /// <summary>
        /// Step 5. Change the terrain height to a linear incline (does not look great)
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
            return "Changed terrain height.";
        }

        /// <summary>
        /// Step 6. Create a new route.
        /// </summary>
        private string AddRoute()
        {
            
            PosVector[] posVectors = new PosVector[7];
            for (int i = 0; i < 6; i++)
            {
                posVectors[i] = new PosVector(new[] {i, i + 1, i + 2}, new[] {i, i + 1, i + 2});
            }
            //this.Handler.SendToTunnel(JSONCommandHelper.WrapAddRoute(posVectors), uuidRoute);
            return "Added a route.";
        }

        /// <summary>
        /// Step 7. Add a road to the previous route.
        /// </summary>
        private string AddRoad()
        { 
            //Handler.SendToTunnel(JSONCommandHelper.WrapAddRouteTerrain(uuidRoute));
            return "Added a road to the previous route.";
        }

        /// <summary>
        /// Step 8. Move a model over the route.
        /// </summary>
        private string MoveModelOverRoad()
        {
            //Handler.SendToTunnel(JSONCommandHelper.WrapFollow(uuidRoute,uuidModel));
            return "The bike is now moving over the route.";
        }
    }
}
