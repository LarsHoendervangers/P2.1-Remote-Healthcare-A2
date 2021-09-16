using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TestVREngine
{
    class ConsoleUI
    {
        private static BasicScene Scene = new BasicScene();
        private static TunnelHandler Handler = new TunnelHandler();

        static void Main(string[] args)
        {
            // Getting the data for all the available clients
            List<ClientData> clients =  Handler.GetAvailableClients();

            Console.WriteLine("Avaliable clients:");
            for (int i = 0; i < clients.Count; i++)
            {
                ClientData c = clients[i];
                Console.WriteLine("{0}: {1}",i + 1, c.ToString());
            }

            int userinput = 0;
            while (userinput < 1 || userinput > clients.Count)
            {
                Console.WriteLine("\nGive a selection number for a tunnel: ");
                userinput = int.Parse(Console.ReadLine());
            }

            string id = Handler.SetUpConnection(clients[userinput - 1].Adress).Item2;
            Console.WriteLine("ID that was returend: "  +  id);


            //Example for controlling vr network enigine 
            //TODO: Delete when there is a proper implementetation

            for (int i = 0; i < 7; i++)
            {
                Console.WriteLine(Scene.ExecuteNext(i));
                Console.ReadKey();
            }
            
        }
    }
}
