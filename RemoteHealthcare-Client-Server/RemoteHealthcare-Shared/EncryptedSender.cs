using RemoteHealthcare_Shared;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;

namespace CommClass
{
    public class EncryptedSender : ISender
    {
        private RSACryptoServiceProvider RSAIN;
        private RSACryptoServiceProvider RSAOUT;

        private NetworkStream stream;

        public EncryptedSender(NetworkStream network)
        {
            //Saving stream;
            stream = network;

            //Setting up rsa objects
            RSAIN = new RSACryptoServiceProvider();
            RSAOUT = new RSACryptoServiceProvider();

            //Sending the correct keys
            //TODO update version
            //stream.Write(RSAIN.ExportRSAPublicKey());

            //Receiving the correct keys
            byte[] publicKeyServer = new byte[140];
            int bytesRead = 0;
            
            //TODO update version
            //stream.Read(publicKeyServer);
            //RSAOUT.ImportRSAPublicKey(publicKeyServer, out bytesRead);
        }

        public void SendMessage(string message)
        {
            /*Console.WriteLine("Message decrypted: " + message);
            byte[] decrypted = Encoding.ASCII.GetBytes(message);
            byte[] encrypted = this.RSAOUT.Encrypt(decrypted, false);
            Console.WriteLine("Message encrypted: " + Encoding.ASCII.GetString(encrypted));*/

            Communications.WriteData(this.RSAOUT.Encrypt(Encoding.ASCII.GetBytes(message), false), stream);
        }

        public string ReadMessage()
        {
          /*  Console.WriteLine("Message received");
            byte[] encrypted = Communications.ReadData(stream);
            Console.WriteLine("Message encrypted: " + Encoding.ASCII.GetString(encrypted));
            byte[] decrypted = this.RSAIN.Decrypt(encrypted, false);
            Console.WriteLine("Message decrypted: " + Encoding.ASCII.GetString(decrypted));*/

            return Encoding.ASCII.GetString(RSAIN.Decrypt(Communications.ReadData(stream), false));
        }
    }
}
