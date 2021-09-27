using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace CommClass
{
    public class PlaneTextSender
    {
        private NetworkStream stream;

        public PlaneTextSender(NetworkStream network)
        {
            stream = network;
        }

        public void SendMessage(string message)
        {
            Communications.WriteData(Encoding.ASCII.GetBytes(message), stream);
        }

        public string ReadMessage()
        {
            return Encoding.ASCII.GetString(Communications.ReadData(stream));
        }
    }
}
