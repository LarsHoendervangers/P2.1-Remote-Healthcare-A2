using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestVREngine
{
    public class JSONCommandHelper
    {

        /// <summary>
        /// Returns the JSON object needed to return all nodes in the file
        /// </summary>
        /// <returns>anonymus object that represents JSON string</returns>
        public static object GetAllNodes()
        {
            return new
            {
                id = "scene/get"
            };
        }

        /// <summary>
        /// Returns the JSON object needed to remove a given node in the file
        /// </summary>
        /// <param name="uuid">The id of the noe to remove</param>
        /// <returns>anonymus object that represents JSON string</returns>
        public static object RemoveNode(string uuid)
        {
            return new
            {
                id = "scene/node/delete",
                data = new
                {
                    id = uuid
                }
            };
        }

        /// <summary>
        /// Returns the JSON object needed to remove a given node in the file
        /// </summary>
        /// <param name="uuid">The id of the noe to update</param>
        /// <returns>anonymus object that represents JSON string</returns>
        public static object UpdateNode(string uuid)
        {
            return new
            {
                id = "scene/node/delete",
                data = new
                {
                    id = uuid
                }
            };
        }

        /// <summary>
        /// Wrap a given payload with the required header information.
        /// </summary>
        /// <param name="destination">The destination ID of the tunnel</param>
        /// <param name="payload">The payload of the message to send</param>
        /// <returns>Returns the complete JSON data to return</returns>
        public static object WrapHeader(string destination, object payload)
        {
            return new
            {
                id = "tunnel/send",
                data = new
                {
                    dest = destination,
                    data =  payload
                    
                }
            };
        }

        /// <summary>
        /// Method which returns an object which can be added to the Header and adds a terrain without heigt specified
        /// </summary>
        /// <param name="size">The x and y size of the plain</param>
        /// <returns>The json text needed to add new terrain</returns>
        public static object WrapTerrain(int[] size)
        {
            return new
            {
                id = "scene/terrain/add",
                data = new
                {
                    size = size
                }
            };
        }

        /// <summary>
        /// Method wich returns an object which can be added to the Header and adds a terrain with height specified
        /// </summary>
        /// <param name="size">The size of the plain</param>
        /// <param name="height">The height points of the plain</param>
        /// <returns>The json text needed to add new terrain with height</returns>
        public static object WrapTerrain(int[] size, float[] height)
        {
            return new
            {
                id = "scene/terrain/add",
                data = new
                {
                    size = size,
                    heights = height
                }
            };
        }

        /// <summary>
        /// This method will return the JSON data to show the terrain.
        /// </summary>
        /// <param name="name">The name the terrain will be given</param>
        /// <param name="transform">The transform applied to the terrain</param>
        /// <returns>The json text needed to display the terrain</returns>
        public static object WrapShowTerrain(string name, Transform transform)
        {
            return new
            {
                id = "scene/node/add",
                data = new
                {
                    name = name,
                    components = new
                    {
                        transform = transform,
                        terrain = new
                        {
                        }
                    }
                }
            };
        }

        /// <summary>
        /// Returns the JSON to delete the current terrain
        /// </summary>
        /// <returns> Returns the JSON to delete the current terrain</returns>
        public static object WrapDeleteTerrain()
        {
            return new
            {
                id = "scene/terrain/delete",
                data = new
                {

                }
            };
        }
        /// <summary>
        /// Method to set the time to the given parameter "time"
        /// </summary>
        /// <param name="time">The time to set to, in fractions</param>
        /// <returns>Returns the object to ga to set the time on the server</returns>
        public static object WrapTime(double time)
        {
            return new
            {
                id = "scene/skybox/settime",
                data = new
                {
                    time = time
                }
            };
        }
        /// <summary>
        /// Adds a 3D object to the map, at the location of the given Transform. The filePath leads to the .obj file you want to spawn.
        /// </summary>
        /// <param name="name">The name of the 3d object</param>
        /// <param name="filePath">The file where the object is stored</param>
        /// <param name="transform">the transform applied to the object</param>
        /// <returns>The JSON object to add a 3d object</returns>
        public static object Wrap3DObject(string name, string filePath, Transform transform)
        {
            return new
            {
                id = "scene/node/add",
                data = new
                {
                    name = name,
                    components = new
                    {
                        transform = transform,
                        model = new
                        {
                            file = filePath
                        }
                    }
                }
            };
        }

        /// <summary>
        /// Same as the method above, now including the parent name attribute.
        /// </summary>
        /// <param name="name">The name of the 3d object</param>
        /// <param name="fileName">The name where the file can be found</param>
        /// <param name="parentName">the name of the parent node</param>
        ///< param name="transform">the transform applied to the object</param>
        /// <returns>The JSON object to add a 3d object</returns>
        public static object Wrap3DObject(string name, string fileName, string parentName, Transform transform)
        {
            return new
            {
                id = "scene/node/add",
                data = new
                {
                    name = name,
                    parent = parentName,
                    components = new
                    {
                        transform = transform,
                        model = new
                        {
                            file = fileName
                        }
                    }
                }
            };
        }
        /// <summary>
        /// Sends a node containing a new panel.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="transform"></param>
        /// <param name="sizeX"></param>
        /// <param name="sizeY"></param>
        /// <param name="resolutionX"></param>
        /// <param name="resolutionY"></param>
        /// <param name="color"></param>
        /// <param name="castShadow"></param>
        /// <returns></returns>
        public static object WrapPanel(string name, Transform transform, int sizeX, int sizeY, int resolutionX, int resolutionY, Color color, bool castShadow)
        {
            return new
            {
                id = "scene/node/add",
                data = new
                {
                    name = name,
                    component = new
                    {
                        transform = transform,
                        panel = new
                        {
                            size = new int[sizeX, sizeY],
                            resolution = new int[resolutionX, resolutionY],
                            background = new int[color.R, color.G, color.B, color.A],
                            castShadow = castShadow
                        }
                    }
                }
            };
        }
        /// <summary>
        /// Length of the int[] is length * width of the terrain you want to update the height from.
        /// </summary>
        /// <param name="height"></param>
        /// <returns></returns>
        public static object WrapUpdateTerrainHeight(int[] height)
        {
            return new
            {
                id = "scene/update/update",
                data = new
                {
                    heights = height
                }
            };
        }
        /// <summary>
        /// Sends a route containing of nodes which are positions (in vectors) to the map.
        /// </summary>
        /// <param name="nodes"></param>
        /// <returns></returns>
        public static object WrapAddRoute(PosVector[] nodes)
        {
            return new
            {
                id = "route/add",
                data = new
                {
                    nodes = nodes
                }
            };
        }
        /// <summary>
        /// Sends a route like the method above, but now including a texture.
        /// </summary>
        /// <param name="routeId"></param>
        /// <param name="diffuse"></param>
        /// <param name="normal"></param>
        /// <param name="specular"></param>
        /// <returns></returns>
        public static object WrapAddRouteTerrain(string routeId, string diffuse, string normal, string specular)
        {
            return new
            {
                id = "scene/road/add",
                data = new
                {
                    route = routeId,
                    diffuse = diffuse,
                    normal = normal,
                    specular = specular,
                    heightoffset = 0.01
                }
            };
        }
        /// <summary>
        /// Sends a new route, only using a routeId
        /// </summary>
        /// <param name="routeId"></param>
        /// <returns></returns>
        public static object WrapAddRouteTerrain(string routeId)
        {
            return new
            {
                id = "scene/road/add",
                data = new
                {
                    route = routeId,
                    heightoffset = 0.01
                }
            };
        }
        /// <summary>
        /// Method used to make the object from the given objectId follow the given route.
        /// </summary>
        /// <param name="routeId"></param>
        /// <param name="objectId"></param>
        /// <returns></returns>
        public static object WrapFollow(string routeId, string objectId)
        {
            return new
            {
                id = "route/follow",
                data = new
                {
                    route = routeId,
                    node = objectId,
                    speed = 3.0,
                    rotate = "XZ",
                    rotateOffset = new int[] {-90, 0, 0},
                    followHeight = true
                }
            };
        }
        /// <summary>
        /// Updates the speed of the object following a route.
        /// </summary>
        /// <param name="objectId"></param>
        /// <param name="speed"></param>
        /// <returns></returns>
        public static object WrapUpdateFollow(string objectId, double speed)
        {
            return new
            {
                id = "route/update",
                data = new
                {
                    node = objectId,
                    speed = speed
                }
            };
        }
        /// <summary>
        /// Sends a boolean to either show or hide the active route.
        /// </summary>
        /// <param name="showRoute"></param>
        /// <returns></returns>
        public static object WrapShowRoute(bool showRoute)
        {
            return new
            {
                id = "route/show",
                data = new
                {
                    show = showRoute
                }
            };
        }
        /// <summary>
        /// Sends a command to reset the entire scene
        /// </summary>
        /// <returns></returns>
        public static object WrapReset()
        {
            return new
            {
                id = "scene/reset"
            };
        }

        public static object WrapRequest()
        {
            return new
            {
                id = "session/list"
            };
        }

        public static object WrapTunnel(string adress)
        {
            return new
            {
                id = "tunnel/create",
                data = new
                {
                    session = adress,
                }
            };
        }


        public static object WrapAddTexture(string uuId, string pathNormal, string pathDiffuse, int minHeight, int maxHeight, int fadeDistance)
        {
            return new
            {
                id = "scene/node/addlayer",
                data = new
                {
                    id = uuId,
                    diffuse = pathDiffuse,
                    normal = pathNormal,
                    minHeight = minHeight,
                    maxHeight = maxHeight,
                    fadeDist = fadeDistance
                }
            };
        }
    }


    /// <summary>
    /// Struct used to contain a PositionVector. This is needed to make an array of these PosVectors.
    /// </summary>
    public struct PosVector
    {
        public int[] pos { get; set; }
        public int[] dir { get; set; }

        public PosVector(int[] position, int[] direction)
        {
            this.pos = position;
            this.dir = direction;
        }
    }

}


