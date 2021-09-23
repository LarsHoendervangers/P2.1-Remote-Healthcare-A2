using Newtonsoft.Json;
using RemoteHealthcare_Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace RemoteHealthcare_Server
{
    class JSONWrite
    {
        public static void LoginWrite(bool succes, TcpClient client)
        {
            object o;
            if (succes)
            {
                o = new
                {
                    command = "message",
                    data = "succesfull connect"
                };

       
            } else
            {
                o = new
                {
                    command = "message",
                    data = "failed connect"
                };
            }

            //Writing answer...
            ComClass.WriteMessage(JsonConvert.SerializeObject(o), client.GetStream());
        }

    }
}
