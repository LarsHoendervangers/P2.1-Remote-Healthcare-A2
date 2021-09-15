

using System;
using System.Net.Sockets;
using System.Text;

namespace TestVREnginge
{
    class Communication
    {

        /// <summary>
        /// This function is for combining two bye arrays.
        /// </summary>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <returns></returns>
        public static byte[] Combine(byte[] first, byte[] second)
        {
            byte[] bytes = new byte[first.Length + second.Length];
            Buffer.BlockCopy(first, 0, bytes, 0, first.Length);
            Buffer.BlockCopy(second, 0, bytes, first.Length, second.Length);
            return bytes;
        }


        /// <summary>
        /// Sends data to the VPS that allows you to connect to a device
        /// </summary>
        /// <param name="networkStream"></param>
        /// <param name="message"></param>
        public static void WriteMessage(NetworkStream networkStream, string message)
        {
            //Console.WriteLine(message);
            byte[] payload = Encoding.ASCII.GetBytes(message);
            byte[] lenght = new byte[4];
            lenght = BitConverter.GetBytes(message.Length);
            byte[] final = Combine(lenght, payload);

            //Debug print of data that is send
            //Console.WriteLine(BitConverter.ToString(final));



            networkStream.Write(final, 0, message.Length + 4);
            networkStream.Flush();
        }

        /// <summary>
        /// The data that is send back from the VPS
        /// </summary>
        /// <param name="networkStream"></param>
        /// <returns></returns>
        public static string ReadMessage(NetworkStream networkStream)
        {

            // 4 bytes lenght == 32 bits, always positive unsigned
            byte[] lenghtArray = new byte[4];

            networkStream.Read(lenghtArray, 0, 4);
            int lenght = BitConverter.ToInt32(lenghtArray, 0);

            //Console.WriteLine(lenght);

            byte[] buffer = new byte[lenght];
            int totalRead = 0;

            //read bytes until stream indicates there are no more
            while (totalRead < lenght)
            {
                int read = networkStream.Read(buffer, totalRead, buffer.Length - totalRead);
                totalRead += read;
                //Console.WriteLine("ReadMessage: " + read);
            }

            return Encoding.ASCII.GetString(buffer, 0, totalRead);
        }
    }
}
