using CommClass;
using Newtonsoft.Json;
using System;
using System.Net.Sockets;

namespace test
{
    class Program
    {
        static void Main(string[] args)
        {
            TcpClient client = new TcpClient("localhost", 8080);
            if (client.Connected)
            {
                Console.WriteLine("Connected to the amazing server...");
                NetworkStream stream = client.GetStream();
                EncryptedSender sender = new EncryptedSender(stream);


                Console.WriteLine("Try to connect with given credentials...");
                //Sending test logins
                object o = new
                {
                    command = "login",
                    data = new
                    {
                        us = "COMBomen",
                        pass = "Communication",
                        flag = 1
                    }
                };
                sender.SendMessage(JsonConvert.SerializeObject(o), client.GetStream());


                


                if (sender.ReadMessage(stream).Contains("succesfull connect"))
                {

                    Console.WriteLine("Athenticated");
                    object f = new
                    {
                        command = "newsession",
                        data = new
                        {
                            patientid = "A12345",
                            state = true
                        }
              
            };
                    sender.SendMessage(JsonConvert.SerializeObject(f), client.GetStream());

                    Console.WriteLine("Send to make a new session");


                    Console.ReadLine();

            }
                


            }


        }
    }
}
