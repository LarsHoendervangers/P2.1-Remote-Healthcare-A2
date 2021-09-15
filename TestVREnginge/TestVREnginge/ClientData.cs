using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestVREnginge
{
    struct ClientData
    {
        public string Adress { get; set; }
        public string Host { get; set; }
        public string User { get; set; }
        public string GPU { get; set; }

        public ClientData(string id, string host, string user, string gpu)
        {
            this.Adress = id;
            this.Host = host;
            this.User = user;
            this.GPU = gpu;
        }


        override
        public string ToString()
        {
            return ($"Adress: {this.Adress}, Host: {this.Host}, User: {this.User}, GPU: {this.GPU}");
        }
    }
}
