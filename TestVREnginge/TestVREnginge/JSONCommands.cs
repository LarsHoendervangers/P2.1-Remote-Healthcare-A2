using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestVREngine
{
    class JSONCommands
    {
        public static string GetJson(string id, object data, string serverId)
        {
            return JsonConvert.SerializeObject(MainJson(id, data, serverId));
        }

        public static string GetJson(string id, object data, int serial, string serverId)
        {
            return JsonConvert.SerializeObject(MainJson(id, data, serverId));
        }

        private static object MainJson(string id, object data, string serverId)
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
