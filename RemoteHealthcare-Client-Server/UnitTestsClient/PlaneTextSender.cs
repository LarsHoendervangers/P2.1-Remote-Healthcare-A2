using RemoteHealthcare_Shared;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace CommClass
{
    public class PlaneTextSender : ISender
    {
        private NetworkStream stream;

        /// <summary>
        /// Constructor. Unencrypted networkstream.
        /// </summary>
        /// <param name="network"></param>
        public PlaneTextSender(NetworkStream network)
        {
            stream = network;
        }

        /// <summary>
        /// Sends a string to the connection.
        /// </summary>
        /// <param name="message">String to send.</param>
        public void SendMessage(string message)
        {
            if (!stream.CanWrite) return;
            Communications.WriteData(Encoding.ASCII.GetBytes(message), stream);
        }

        /// <summary>
        /// Read the encrypted message.
        /// </summary>
        /// <returns>The string that was received.</returns>
        public string ReadMessage()
        {
            return Encoding.ASCII.GetString(Communications.ReadData(stream));
        }
    }
}
