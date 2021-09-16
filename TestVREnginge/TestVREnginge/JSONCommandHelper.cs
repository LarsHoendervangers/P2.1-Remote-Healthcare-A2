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

        public static object WrapHeader(string destination, object payload)
        {
            return new
            {
                id = "tunnel/send",
                data = new
                {
                    dest = destination,
                    data = new
                    {
                        payload
                    }
                }
            };
        }

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

        public static object WrapTerrain(int[] size, int[] height)
        {
            return new
            {
                id = "scene/terrain/add",
                data = new
                {
                    size = size,
                    height = height
                }
            };
        }

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

        public static object Wrap3DObject(string name, string fileName, Transform transform)
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
                        model = new
                        {
                            file = fileName
                        }
                    }
                }
            };
        }

        public static object Wrap3DObject(string name, string fileName, string parentName, Transform transform)
        {
            return new
            {
                id = "scene/node/add",
                data = new
                {
                    name = name,
                    parent = parentName,
                    component = new
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

        public static object WrapUpdateTerrainHeight(int[] height)
        {
            return new
            {
                id = "scene/Update/add",
                data = new
                {
                    heights = height
                }
            };
        }
      
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

        public static object WrapFollow(string routeId, string objectId)
        {
            return new
            {
                id = "route/follow",
                data = new
                {
                    route = routeId,
                    node = objectId,
                    speed = 1.0
                }
            };
        }

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

        public static object WrapReset()
        {
            return new
            {
                id = "scene/reset"
            };
        }
    }

    public struct PosVector
    {
        int[] position;
        int[] direction;

        public PosVector(int[] position, int[] direction)
        {
            this.position = position;
            this.direction = direction;
        }
    }

}


