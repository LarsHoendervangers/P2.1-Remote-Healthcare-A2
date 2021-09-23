using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RemoteHealthcare_Shared
{
    class ComClass
    {
        public static void WriteMessage(string message, NetworkStream stream)
        {
            //Console.WriteLine(message);
            byte[] payload = Encoding.ASCII.GetBytes(message);
            byte[] lenght = new byte[4];
            lenght = BitConverter.GetBytes(message.Length);
            byte[] final = Combine(lenght, payload);

            //Debug print of data that is send
            //Console.WriteLine(BitConverter.ToString(final));
            stream.Write(final, 0, message.Length + 4);
            stream.Flush();
        }

        private static byte[] Combine(byte[] first, byte[] second)
        {
            byte[] bytes = new byte[first.Length + second.Length];
            Buffer.BlockCopy(first, 0, bytes, 0, first.Length);
            Buffer.BlockCopy(second, 0, bytes, first.Length, second.Length);
            return bytes;
        }

        /// <summary>
        /// Reads a message from the TCP connection
        /// </summary>
        /// <returns>The message as a string</returns> 
        public static string ReadMessage(NetworkStream stream)
        {
            // 4 bytes lenght == 32 bits, always positive unsigned
            byte[] lenghtArray = new byte[4];

            try {
                stream.Read(lenghtArray, 0, 4);
            } catch (Exception e)
            {
                Trace.WriteLine(e.Message);
            }
            int lenght = BitConverter.ToInt32(lenghtArray, 0);

            //Console.WriteLine(lenght);

            byte[] buffer = new byte[lenght];
            int totalRead = 0;

            //read bytes until stream indicates there are no more
            while (totalRead < lenght)
            {
                int read = stream.Read(buffer, totalRead, buffer.Length - totalRead);
                totalRead += read;
                //Console.WriteLine("ReadMessage: " + read);
            }

            return Encoding.ASCII.GetString(buffer, 0, totalRead);
        }
    }
}
