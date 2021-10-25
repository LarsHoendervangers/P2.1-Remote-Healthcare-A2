using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Sockets;
using System.Text;

namespace CommClass
{
    class Communications
    {
        /// <summary>
        /// Write bytes to the networkstream.
        /// </summary>
        /// <param name="data">Bytes to write to the stream.</param>
        /// <param name="stream">The stream that is used to write the bytes to.</param>
        public static void WriteData(byte[] data, NetworkStream stream)
        {
            byte[] payload = data;
            byte[] length = new byte[4];
            length = BitConverter.GetBytes(data.Length);
            byte[] final = Combine(length, payload);

            //Debug print of data that is send
            try
            {
                stream?.Write(final, 0, data.Length + 4);
                stream?.Flush();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }

        /// <summary>
        /// Combines two byte arrays into one.
        /// </summary>
        /// <param name="first">This byte array will be placed infront of the second byte array.</param>
        /// <param name="second">This byte array will be placed behind the first byte array.</param>
        /// <returns>A byte array that contains these byte arrays.</returns>
        private static byte[] Combine(byte[] first, byte[] second)
        {
            byte[] bytes = new byte[first.Length + second.Length];
            Buffer.BlockCopy(first, 0, bytes, 0, first.Length);
            Buffer.BlockCopy(second, 0, bytes, first.Length, second.Length);
            return bytes;
        }

        /// <summary>
        /// Reads the data from the stream.
        /// </summary>
        /// <param name="stream">Networkstream that is used to read data from.</param>
        /// <returns>A byte array with the data from the stream.</returns>
        public static byte[] ReadData(NetworkStream stream)
        {
            // 4 bytes length == 32 bits, always positive unsigned
            byte[] lenghtArray = new byte[4];

            stream.Read(lenghtArray, 0, 4);
            int length = BitConverter.ToInt32(lenghtArray, 0);
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
