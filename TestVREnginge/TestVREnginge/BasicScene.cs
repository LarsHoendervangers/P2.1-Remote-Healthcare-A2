using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestVREngine
{
    class BasicScene
    {
        private List<string> CommandList;
        private TunnelHandler Handler;
        //private TunnelHandler Handler;

        public BasicScene(TunnelHandler Handler)
        {
            this.CommandList = new List<string>();
            this.Handler = Handler;
        }

        /// <summary>
        /// This method is called and will execute the next step in the exercise.
        /// </summary>
        public string ExecuteNext(int index)
        {
            return "";
        }

        // TODO: Test if the heights attribute is necessary
        /// <summary>
        /// Step 1.
        /// </summary>
        private void CreateTerrain()
        {
            /*JSONCommands.SendTunnel("scene/terrain/add", new
                {
                    size = new int[] {256, 256}
                });*/
        }

        /// <summary>
        /// Step 2.
        /// </summary>
        private void RemoveGroundPlane()
        {
            //JSONCommands.SendTunnel("scene/terrain/delete", new {});
        }

        /// <summary>
        /// Step 3.
        /// </summary>
        private void ChangeTime()
        {
            /*JSONCommands.SendTunnel("scene/skybox/settime", new
            {
                time = 5
            });*/
        }

        /// <summary>
        /// Step 4.
        /// </summary>
        private void AddModels()
        {
            /*JSONCommands.SendTunnel("scene/terrain/add", new
                {
                    
                });*/
        }

        /// <summary>
        /// Step 5.
        /// </summary>
        private void ChangeTerrainHeight()
        {
           /* JSONCommands.SendTunnel("scene/terrain/update", new
            {
                
            });*/
        }

        /// <summary>
        /// Step 6.
        /// </summary>
        private void AddRoute()
        {
            /*JSONCommands.SendTunnel("route/add", new
            {

            });*/
        }

        /// <summary>
        /// Step 7.
        /// </summary>
        private void AddRoad()
        {
            /*JSONCommands.SendTunnel("scene/road/add", new
            {

            });*/
        }

        /// <summary>
        /// Step 8.
        /// </summary>
        private void MoveModelOverRoad()
        {
            /*JSONCommands.SendTunnel("route/follow", new
            {

            });*/
        }
    }
}
