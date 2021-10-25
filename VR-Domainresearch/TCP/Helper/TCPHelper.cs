using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace VR_Domainresearch.TCP.helper
{
    static class TCPHelper
    {
        private static Encoding encoding = Encoding.UTF8;

        //TODO change to take JSON message
        public static string ReadMessage(NetworkStream networkStream)
        {

            // 4 bytes lenght == 32 bits, always positive unsigned
            byte[] lenghtArray = new byte[4];

            networkStream.Read(lenghtArray, 0, 4);
            uint lenght = BitConverter.ToUInt32(lenghtArray, 0);

            Console.WriteLine(lenght);

            byte[] buffer = new byte[lenght];
            int totalRead = 0;

            //read bytes until stream indicates there are no more
            do
            {
                int read = networkStream.Read(buffer, totalRead, buffer.Length - totalRead);
                totalRead += read;
                Console.WriteLine("ReadMessage: " + read);
            } while (networkStream.DataAvailable);

            return encoding.GetString(buffer, 0, totalRead);
        }

        //TODO change to take JSON message
        public static void SendMessage(NetworkStream networkStream, object payload)
        {
            // Transforming the object to a string to be able to send the data
            string message = payload.ToString();

            //make sure the other end decodes with the same format!
            byte[] bytes = encoding.GetBytes(message);
            byte[] length = BitConverter.GetBytes((uint)message.Length); // Getting the length of the payload
            Console.WriteLine("Lenghts = {0}", message.Length);

            // Takes the first four bytes that indicate lenght and adds it to the total load
            bytes = length.Concat(bytes).ToArray();

            networkStream.Write(bytes, 0, bytes.Length);
        }

    }
}
