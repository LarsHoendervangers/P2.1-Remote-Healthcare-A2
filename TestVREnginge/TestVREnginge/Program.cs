using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;

namespace TestVREnginge
{
    class Program
    {
        static void Main(string[] args)
        {
            String test = "{\r\n\"id\" : \"session/list\"\r\n}";

            byte[] payload = Encoding.ASCII.GetBytes(test);
            byte[] lenght = new byte[] { 0x1B, 0x00, 0x00, 0x00 };
            byte[] final = Combine(lenght, payload);

            Console.WriteLine(BitConverter.ToString(final));

            TcpClient client = new TcpClient("145.48.6.10", 6666);
            NetworkStream stream = client.GetStream();

            stream.Write(final, 0, test.Length + 4);
            stream.Flush();



            //Reading
            Console.WriteLine(  ReadMessage(stream));
        }


       

        public static byte[] Combine(byte[] first, byte[] second)
        {
            byte[] bytes = new byte[first.Length + second.Length];
            Buffer.BlockCopy(first, 0, bytes, 0, first.Length);
            Buffer.BlockCopy(second, 0, bytes, first.Length, second.Length);
            return bytes;
        }

        public static string ReadMessage(NetworkStream networkStream)
        {

            // 4 bytes lenght == 32 bits, always positive unsigned
            byte[] lenghtArray = new byte[4];

            networkStream.Read(lenghtArray, 0, 4);
            int lenght = BitConverter.ToInt32(lenghtArray, 0);

            Console.WriteLine(lenght);

            byte[] buffer = new byte[lenght];
            int totalRead = 0;

            //read bytes until stream indicates there are no more
            while (totalRead < lenght)
            {
                int read = networkStream.Read(buffer, totalRead, buffer.Length - totalRead);
                totalRead += read;
                Console.WriteLine("ReadMessage: " + read);
            } 

            return Encoding.ASCII.GetString(buffer, 0, totalRead);
        }



    }
}
