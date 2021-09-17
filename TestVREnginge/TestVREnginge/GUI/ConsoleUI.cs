using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TestVREngine.Scene;
using TestVREngine.Structs;
using TestVREngine.Tunnel;

namespace TestVREngine.GUI
{
    class ConsoleUI
    {

        public static void Run()
        {
            TunnelHandler handler = new TunnelHandler();
            BasicScene scene = new BasicScene(handler);

            // Getting the data for all the available clients
            List<ClientData> Clients = handler.GetAvailableClients();

            //Lists all the avalable clients and adds the corresponding number in the list
            Console.WriteLine("Avaliable clients:");
            for (int i = 0; i < Clients.Count; i++)
            {
                ClientData C = Clients[i];
                Console.WriteLine("{0}: {1}", i + 1, C.ToString());
            }

            //Ask for userinput
            int Userinput = 0;
            while (Userinput < 1 || Userinput > Clients.Count)
            {
                Console.WriteLine("\nGive a selection number for a tunnel: ");

                try
                {
                    Userinput = int.Parse(Console.ReadLine());
                }
                catch (Exception e)
                {
                    Console.WriteLine("Please give just a number");
                }
            }

            handler.SetUpConnection(Clients[Userinput - 1].Adress);
            string id = handler.destinationID;
            Console.WriteLine(handler.destinationID);
            Console.WriteLine("ID that was returend: " + id);

            //Loop which calls a method from the BasicScene class and starts the corresponding activity from teh list
            for (int i = 0; i < 7; i++)
            {
                Console.WriteLine(scene.ExecuteNext(i));
                Console.ReadKey();
            }

            Console.WriteLine("All methods have been executed...");
        }
    }
}
