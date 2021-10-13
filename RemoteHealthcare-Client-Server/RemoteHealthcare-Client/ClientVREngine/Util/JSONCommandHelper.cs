using Newtonsoft.Json.Linq;
using RemoteHealthcare.ClientVREngine.Util.Structs;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoteHealthcare.ClientVREngine.Util
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
                id = "scene/node/update",
                data = new
                {
                    id = uuid
                }
            };
        }

        public static object UpdateNodeCamera(string uuid, string parent, Transform transform)
        {
            return new
            {
                id = "scene/node/update",
                data = new
                {
                    id = uuid,
                    parent,
                    transform
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
                    data = payload

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
                    size
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
                    size,
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
                    name,
                    components = new
                    {
                        transform,
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
                    time
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
                    name,
                    components = new
                    {
                        transform,
                        model = new
                        {
                            file = filePath
                        }
                    }
                }
            };
        }
        /// <summary>
        /// Same method as above but without transform
        /// </summary>
        /// <param name="name"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static object Wrap3DObject(string name, string filePath)
        {
            Transform transform = new Transform(1, new double[3] { 0, 0, 0 }, new double[3] { 0, 0, 0 });
            return new
            {
                id = "scene/node/add",
                data = new
                {
                    name,
                    components = new
                    {
                        transform,
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
                    name,
                    parent = parentName,
                    components = new
                    {
                        transform,
                        model = new
                        {
                            file = fileName
                        }
                    }
                }
            };
        }

        /// <summary>
        /// Wraps given variables to JSON wich can be used to create a panel
        /// </summary>
        /// <param name="name">The name of the node</param>
        /// <param name="transform">The transform applied to the node</param>
        /// <param name="sizeX">The x size of the panel</param>
        /// <param name="sizeY">The y size of the panel</param>
        /// <param name="resolutionX">The x resolution of the panel</param>
        /// <param name="resolutionY">The y resolution of the panel</param>
        /// <param name="color">THe background color of the panel</param>
        /// <param name="castShadow">true/false, panel casts a shadow</param>
        /// <returns>The JSON pbject to create a panel</returns>
        public static object WrapPanel(string name, string parent, Transform transform, double sizeX, double sizeY, int resolutionX, int resolutionY, bool castShadow)
        {
            return new
            {
                id = "scene/node/add",
                data = new
                {
                    name,
                    parent,
                    components = new
                    {
                        transform,
                        panel = new
                        {
                            size = new double[] { sizeX, sizeY },
                            resolution = new int[] { resolutionX, resolutionY },
                            background = new int[] { 1, 1, 1, 0 },
                            castShadow
                        }
                    }
                }
            };
        }

        //to add text to a plain first add a panel with the panel WrapPanel method then do a clear with the WrapPanelClear method then do a swap 
        //with the WrapPanelSwap method then draw the text with the WrapPanelText method and at last do a WrapPanelSwap agian.
        public static object WrapPanelText(string id, string text, double[] position, double size, string font)
        {
            return new
            {
                id = "scene/panel/drawtext",
                data = new
                {
                    id,
                    text,
                    position,
                    size,
                    color = new int[] { 0, 0, 0, 1 },
                    font
                }
            };
        }

        public static object WrapPanelClear(string id)
        {
            return new
            {
                id = "scene/panel/clear",
                data = new
                {
                    id
                }
            };
        }

        public static object WrapPanelSwap(string id)
        {
            return new
            {
                id = "scene/panel/swap",
                data = new
                {
                    id
                }
            };
        }



        /// <summary>
        /// Length of the int[] is length * width of the terrain you want to update the height from.
        /// </summary>
        /// <param name="height">The int array needed to set the height map of the terrain</param>
        /// <returns>The JSON needed to update the terrain height</returns>
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
        /// <param name="nodes">The different point in the route</param>
        /// <returns>The JSON object needed to create a new route</returns>
        public static object WrapAddRoute(PosVector[] nodes)
        {
            return new
            {
                id = "route/add",
                data = new
                {
                    nodes
                }
            };
        }

        /// <summary>
        /// Sends a route like the method above, but now including a texture.
        /// </summary>
        /// <param name="routeId">The uuid of the route to add roads to</param>
        /// <param name="diffuse">The diffuse map to add to the road</param>
        /// <param name="normal">The normal map to add to the road</param>
        /// <param name="specular">The specular map to add to the road</param>
        /// <returns>The JSON Object needed to add a texture to a terrain</returns>
        public static object WrapAddRouteTerrain(string routeId, string diffuse, string normal, string specular)
        {
            return new
            {
                id = "scene/road/add",
                data = new
                {
                    route = routeId,
                    diffuse,
                    normal,
                    specular,
                    heightoffset = 0.01
                }
            };
        }

        /// <summary>
        /// Sends a route like the method above, with default textures
        /// </summary>
        /// <param name="routeId">The uuid of the route </param>
        /// <returns>The JSON object to add terrain to a route</returns>
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
        /// <param name="routeId">The uuid of the route node</param>
        /// <param name="objectId">The uuid of the object node</param>
        /// <returns>The JSON object to make a node follow a route</returns>
        public static object WrapFollow(string routeId, string objectId, double[] rotateOffset)
        {
            return new
            {
                id = "route/follow",
                data = new
                {
                    route = routeId,
                    node = objectId,
                    speed = 0.0,
                    rotate = "XZ",
                    rotateOffset,
                    followHeight = true
                }
            };
        }

        /// <summary>
        /// Updates the speed of the object following a route.
        /// </summary>
        /// <param name="objectId"></param>
        /// <param name="speed"></param>
        /// <returns>The JSON object needed to update the speed of the following object</returns>
        public static object WrapUpdateFollow(string objectId, double speed)
        {
            return new
            {
                id = "route/follow/speed",
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
        /// <param name="showRoute">showRoute or not</param>
        /// <returns>The JSON object needed the update the showRoute command</returns>
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
        /// <returns>The JSON object needed to reset the scene</returns>
        public static object WrapReset()
        {
            return new
            {
                id = "scene/reset"
            };
        }

        /// <summary>
        /// Method to create the JSON object needed to retrewive the session list from the server
        /// </summary>
        /// <returns>The JSON object needed to get the session list</returns>
        public static object WrapRequest()
        {
            return new
            {
                id = "session/list"
            };
        }

        /// <summary>
        /// Method to create the JSON object needed to create a tunnel to the server
        /// </summary>
        /// <param name="adress">The addresID of the tunnel to connect to</param>
        /// <returns>The JSON object to create a tunnel with</returns>
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


        /// <summary>
        /// Given the parameters this method wraps them to the correct JSON object to add a texture to a node
        /// </summary>
        /// <param name="uuId">The uuid of the node to add to</param>
        /// <param name="pathNormal">The file of the normal map of the texture</param>
        /// <param name="pathDiffuse">The file of the diffuse map of the texture</param>
        /// <param name="minHeight">The min height of the texture</param>
        /// <param name="maxHeight">The max height of the texture</param>
        /// <param name="fadeDistance">The fade distance of the texture</param>
        /// <returns>The JSON object needed to add a texture to a model</returns>
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
                    minHeight,
                    maxHeight,
                    fadeDist = fadeDistance
                }
            };
        }

        /// <summary>
        /// Wrap the command to save the current scene of the server
        /// </summary>
        /// <param name="filename">The filename where the scene is stored</param>
        /// <returns>The JSON object wrapped to save a scene</returns>
        public static object WrapSaveScene(string filename)
        {
            return new
            {
                id = "scene/save",
                data = new
                {
                    filename,
                    overwrite = true

                }
            };
        }

        /// <summary>
        /// Wrap the command to load a scene to  the server
        /// </summary>
        /// <param name="filename">The filename where the scene is stored</param>
        /// <returns>The JSON object wrapped to load a scene</returns>
        public static object WrapLoadScene(string filename)
        {
            return new
            {
                id = "scene/load",
                data = new
                {
                    filename,
                }
            };
        }


    }

}


