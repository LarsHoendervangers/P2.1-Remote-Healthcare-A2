using CommClass;
using Newtonsoft.Json;
using System;

using System.Net.Sockets;

using System.Security.Cryptography;
using System.Text;
using System.Threading;

namespace other
{
    class Programm
    {
        static void Main(string[] args)
        {
            TcpClient client = new TcpClient("localhost", 6969);
            if (client.Connected)
            {
                Console.WriteLine("Connected to the amazing server...");
                NetworkStream stream = client.GetStream();
                PlaneTextSender sender = new PlaneTextSender(stream);


                Console.WriteLine("Try to connect with given credentials...");
                //Sending test logins
                object o = new
                {
                    command = "login",
                    data = new
                    {
                        us = "JHAOogstvogel",
                        pass = "Welkom123",
                        flag = 0
                    }
                };
                sender.SendMessage(JsonConvert.SerializeObject(o));

                //Console.WriteLine(sender.ReadMessage(stream));


                while (true)
                {
                    Console.WriteLine(   sender.ReadMessage());
                }


               /* if (sender.ReadMessage().Contains("succesfull connect"))
                {
                    while (true)
                    {
                        Console.WriteLine("Athenticated");
                        object f = new
                        {
                            command = "ergometer",
                            data = new
                            {
                                time = new DateTime(1, 2, 3),
                                bpm = 100
                            }

                        };
                        sender.SendMessage(JsonConvert.SerializeObject(f));

                       
                        Console.WriteLine("Send data");
                        Thread.Sleep(1000);
                    }

                }*/



            }


        }


        }
   
    }

