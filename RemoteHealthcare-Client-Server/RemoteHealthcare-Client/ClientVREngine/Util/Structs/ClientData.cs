using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoteHealthcare.ClientVREngine.Util.Structs
{
    /// <summary>
    /// Struct that stores the useful data of a server client.
    /// </summary>
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
            // return readable data of the client
            return $"Host: {Host,-20}- User: {User,-8}- GPU: {GPU,-35}- Adress: {Adress}";
        }
    }
}
