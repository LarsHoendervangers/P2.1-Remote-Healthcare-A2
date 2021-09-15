using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestVREnginge
{
    class JSONCommands
    {
        public static string serverId = "server id";

        public static string SendTunnel(string id, object data)
        {
            return JsonConvert.SerializeObject(MainJson(id, data));
        }

        public static string SendTunnel(string id, object data, int serial)
        {
            return JsonConvert.SerializeObject(MainJson(id, data));
        }

        public static object MainJson(string id, object data)
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
