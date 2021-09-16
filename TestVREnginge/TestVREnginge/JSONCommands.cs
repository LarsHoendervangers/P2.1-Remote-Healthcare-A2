using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestVREngine
{
    class JSONCommands
    {
        //public static string serverId = "server id";

        public static string SendTunnel(string id, object data, string serverId)
        {
            return JsonConvert.SerializeObject(MainJson(id, data, serverId));
        }

        public static string SendTunnel(string id, object data, string serverId, int serial)
        {
            return JsonConvert.SerializeObject(MainJson(id, data, serverId));
        }

        public static object MainJson(string id, object data, string serverId)
        {
            return new
            {
                id = "tunnel/send",
                data = new
                {
                    dest = serverId,
                    data = new
                    {
                        id = id,
                        data
                    }
                }
            };
        }
    }
}
