using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestVREnginge
{
    class ConsoleUI
    {
        private static BasicScene Scene = new BasicScene();
        private static TunnelHandler Handler = new TunnelHandler();

        static void Main(string[] args)
        {
            List<ClientData> clients =  Handler.GetAvailableClients();
            Console.WriteLine("Avaliable clients:");
            foreach(ClientData c in clients)
            {
                Console.WriteLine(c.ToString());
            }

            int userinput = 0;
            while (userinput < 1 || userinput > clients.Count)
            {
                Console.WriteLine("\nGive a selection number for a tunnel: ");
                userinput = int.Parse(Console.ReadLine());
            }

            Console.WriteLine("ID that was returend: "  + Handler.SetUpConnection(clients[userinput - 1].Adress).Item2);
        }
    }
}
