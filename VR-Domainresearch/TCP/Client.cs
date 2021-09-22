using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using VR_Domainresearch.TCP.helper;

namespace VR_Domainresearch.TCP
{
    class Client
    {
        class Program
        {
            //TODO test applicaiton remove
            static void Main(string[] args)
            {
                TcpClient client = new TcpClient("145.49.41.137", 6969);
                NetworkStream networkStream = client.GetStream();

                bool done = false;
                Console.WriteLine("Type 'bye' to end connection");
                while (!done)
                {
                    Console.Write("Enter a message to send to server: ");
                    string message = Console.ReadLine();

                    // Gebruik van optie 2:
                    TCPHelper.SendMessage(networkStream, message);

                    string response = TCPHelper.ReadMessage(networkStream);

                    Console.WriteLine("Response: " + response);
                    done = response.Equals("BYE");
                }

                client.Close();
                networkStream.Close();
            }
        }
    }

}
