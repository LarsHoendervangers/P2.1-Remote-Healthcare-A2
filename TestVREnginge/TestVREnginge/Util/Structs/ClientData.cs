using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestVREngine.Util.Structs
{
    struct ClientData
    {
        public string Adress { get; set; }
        public string Host { get; set; }
        public string User { get; set; }
        public string GPU { get; set; }

        public ClientData(string id, string host, string user, string gpu)
        {
            Adress = id;
            Host = host;
            User = user;
            GPU = gpu;
        }


        override
        public string ToString()
        {
            return $"Adress: {Adress}, Host: {Host}, User: {User}, GPU: {GPU}";
        }
    }
}
