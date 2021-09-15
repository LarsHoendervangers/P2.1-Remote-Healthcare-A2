using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestVREnginge
{
    class ConsoleUI
    {
        private BasicScene Scene = new BasicScene();
        private TunnelHandler Handler = new TunnelHandler();

        public void start()
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

            Handler.SetUpConnection(clients[userinput - 1].Id);
        }
    }
}
