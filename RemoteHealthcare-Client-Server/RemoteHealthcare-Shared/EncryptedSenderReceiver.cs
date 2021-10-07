using CommClass;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace RemoteHealthcare_Shared
{
    public class EncryptedSenderReceiver : ISender
    {
        protected SslStream sslStream;

        public string ReadMessage()
        {
            // 4 bytes lenght == 32 bits, always positive unsigned
            byte[] lenghtArray = new byte[4];

            sslStream.Read(lenghtArray, 0, 4);
            int length = BitConverter.ToInt32(lenghtArray, 0);

            byte[] buffer = new byte[length];
            int totalRead = 0;

            //read bytes until stream indicates there are no more
            while (totalRead < length)
            {
                int read = sslStream.Read(buffer, totalRead, buffer.Length - totalRead);
                totalRead += read;
                //Console.WriteLine("ReadMessage: " + read);
            }

            return Encoding.ASCII.GetString(buffer);
        }

        public void SendMessage(string message)
        {
            byte[] data = Encoding.ASCII.GetBytes(message);
            byte[] payload = data;
            byte[] length = new byte[4];
            length = BitConverter.GetBytes(data.Length);
            byte[] final = Combine(length, payload);

            //Debug print of data that is send
            //Console.WriteLine(BitConverter.ToString(final));
            sslStream.Write(final, 0, data.Length + 4);
            sslStream.Flush();
        }

        private static byte[] Combine(byte[] first, byte[] second)
        {
            byte[] bytes = new byte[first.Length + second.Length];
            Buffer.BlockCopy(first, 0, bytes, 0, first.Length);
            Buffer.BlockCopy(second, 0, bytes, first.Length, second.Length);
            return bytes;
        }
    }
}
