using System;
using System.Diagnostics;
using System.IO;
using System.Net.Security;
using System.Text;

namespace RemoteHealthcare_Shared
{
    public class EncryptedSenderReceiver : ISender
    {
        protected SslStream sslStream;

        public string ReadMessage()
        {
            // 4 bytes length == 32 bits, always positive unsigned
            byte[] lengthArray = new byte[4];
            if (sslStream.CanRead) {
                sslStream.Read(lengthArray, 0, 4);
                int length = BitConverter.ToInt32(lengthArray, 0);

                byte[] buffer = new byte[length];
                int totalRead = 0;

                //read bytes until stream indicates there are no more
                while (totalRead < length)
                {
                    int read = sslStream.Read(buffer, totalRead, buffer.Length - totalRead);
                    totalRead += read;
                }
                //Can also return an empty string.
                return Encoding.ASCII.GetString(buffer);
            }
            //Empty string is caught in the Host class which will terminate the connection.
            return "";
        }

        public void SendMessage(string message)
        {
            byte[] data = Encoding.ASCII.GetBytes(message);
            byte[] payload = data;
            byte[] length = new byte[4];
            length = BitConverter.GetBytes(data.Length);
            byte[] final = Combine(length, payload);

            //Debug print of data that is send
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
