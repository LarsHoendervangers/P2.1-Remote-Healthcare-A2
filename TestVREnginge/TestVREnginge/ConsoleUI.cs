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
        private static TunnelHandler Handler = new TunnelHandler();
        //private static BasicScene Scene = new BasicScene(Handler);

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

            for (int i = 0; i < 1; i++)
            {
                Handler.exampleFunction(JSONCommands.SendTunnel("scene/node/add",
                    new
                    {
                        name = "house",
                        components = new
                        {
                            transform = new
                            {
                                position = new[] { 0, 30, 0 },
                                scale = 1,
                                rotation = new[] { 0, 0, 0 }
                            },
                            model = new
                            {
                                file = $"data/NetworkEngine/models/tatooine_test/tatooine_test.obj",
                                cullbackfaces = false
                            }
                        }
                    }, id
                ));
                //Console.WriteLine(Scene.ExecuteNext(i));
                //Console.ReadKey();
            }
            
        }
    }
}
