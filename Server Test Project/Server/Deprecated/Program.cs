using CommClass;
using System;
using System.Collections;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace test
{
    class Program
    {



        static void Main(string[] args)
        {
            TcpListener listener = new TcpListener(IPAddress.Any, 8080);
            listener.Start();
            while (true)
            {
                Console.WriteLine("Waiting for a client to connect...");
                // Application blocks while waiting for an incoming connection.
                // Type CNTL-C to terminate the server.
                TcpClient client = listener.AcceptTcpClient();
                SessionHandler(client);
            }
        }

        private static void SessionHandler(TcpClient client)
        {
            NetworkStream stream = client.GetStream();

            EncryptedSender sender = new EncryptedSender(stream);

            while (true)
            {
                Console.WriteLine(sender.ReadMessage(stream));
            }




        }

        

      

    }

}
