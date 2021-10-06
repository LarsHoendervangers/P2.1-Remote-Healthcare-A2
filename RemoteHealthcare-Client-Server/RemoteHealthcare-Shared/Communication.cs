using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace CommClass
{
    class Communications
    {
        public static void WriteData(byte[] data, NetworkStream stream)
        {
            byte[] payload = data;
            byte[] length = new byte[4];
            length = BitConverter.GetBytes(data.Length);
            byte[] final = Combine(length, payload);

            //Debug print of data that is send
            stream.Write(final, 0, data.Length + 4);
            stream.Flush();
        }

        private static byte[] Combine(byte[] first, byte[] second)
        {
            byte[] bytes = new byte[first.Length + second.Length];
            Buffer.BlockCopy(first, 0, bytes, 0, first.Length);
            Buffer.BlockCopy(second, 0, bytes, first.Length, second.Length);
            return bytes;
        }

        public static byte[] ReadData(NetworkStream stream)
        {
            // 4 bytes lenght == 32 bits, always positive unsigned
            byte[] lenghtArray = new byte[4];

            stream.Read(lenghtArray, 0, 4);
            int length = BitConverter.ToInt32(lenghtArray, 0);

            //Console.WriteLine(length);

            byte[] buffer = new byte[length];
            int totalRead = 0;

            //read bytes until stream indicates there are no more
            while (totalRead < length)
            {
                int read = stream.Read(buffer, totalRead, buffer.Length - totalRead);
                totalRead += read;
            }

            return buffer;
        }
    }
}
