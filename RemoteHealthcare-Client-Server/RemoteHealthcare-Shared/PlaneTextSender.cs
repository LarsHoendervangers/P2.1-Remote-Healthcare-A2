using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace CommClass
{
    class PlaneTextSender
    {
        public void SendMessage(string message, NetworkStream stream)
        {
            Communications.WriteData(Encoding.ASCII.GetBytes(message), stream);
        }

        public string ReadMessage(NetworkStream stream)
        {
            return Encoding.ASCII.GetString(Communications.ReadData(stream));
        }
    }
}
