using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestVREnginge
{
    struct ClientData
    {
        public string Id { get; set; }
        public string Host { get; set; }
        public string User { get; set; }
        public string GPU { get; set; }

        public ClientData(string id, string host, string user, string gpu)
        {
            this.Id = id;
            this.Host = host;
            this.User = user;
            this.GPU = gpu;

        }
    }
}
