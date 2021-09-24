using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;

namespace CommClass
{
    class EncryptedSender
    {
        private RSACryptoServiceProvider RSAIN;
        private RSACryptoServiceProvider RSAOUT;

        public EncryptedSender(NetworkStream stream)
        {
            //Setting up rsa objects
            this.RSAIN = new RSACryptoServiceProvider();
            this.RSAOUT = new RSACryptoServiceProvider();

            //Sending the correct keys
            stream.Write(RSAIN.ExportRSAPublicKey());

            //Receiving the correct keys
            byte[] publicKeyServer = new byte[140];
            int bytesRead = 0;
            stream.Read(publicKeyServer);
            RSAOUT.ImportRSAPublicKey(publicKeyServer, out bytesRead);
        }

        public void SendMessage(string message, NetworkStream stream)
        {
            /*Console.WriteLine("Message decrypted: " + message);
            byte[] decrypted = Encoding.ASCII.GetBytes(message);
            byte[] encrypted = this.RSAOUT.Encrypt(decrypted, false);
            Console.WriteLine("Message encrypted: " + Encoding.ASCII.GetString(encrypted));*/

            Communications.WriteData(this.RSAOUT.Encrypt(Encoding.ASCII.GetBytes(message), false), stream);
        }

        public string ReadMessage(NetworkStream stream)
        {
          /*  Console.WriteLine("Message received");
            byte[] encrypted = Communications.ReadData(stream);
            Console.WriteLine("Message encrypted: " + Encoding.ASCII.GetString(encrypted));
            byte[] decrypted = this.RSAIN.Decrypt(encrypted, false);
            Console.WriteLine("Message decrypted: " + Encoding.ASCII.GetString(decrypted));*/

            return Encoding.ASCII.GetString(this.RSAIN.Decrypt(Communications.ReadData(stream), false));
        }
    }
}
