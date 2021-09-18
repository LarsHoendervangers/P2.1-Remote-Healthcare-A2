using System;
using System.Collections.Generic;
using System.Diagnostics;
using TestVREngine.Scene;
using TestVREngine.Tunnel;
using TestVREngine.Util.Structs;

namespace TestVREngine.GUI
{
    class ConsoleUI
    {

        /// <summary>
        /// Runs the program of a scene and interacts with the user
        /// </summary>
        public static void Run()
        {
            //Start logger
            string debugPath = $"DebugLogging/debug{DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss")}.log";
            Console.WriteLine(debugPath);
            Trace.Listeners.Add(new TextWriterTraceListener(debugPath));
            Trace.AutoFlush = true;

            TunnelHandler handler = new TunnelHandler();
            //GeneralScene scene = new LoaderScene(handler);
            GeneralScene scene = new PodraceScene(handler);

            // Getting the data for all the available clients
            List<ClientData> Clients = handler.GetAvailableClients();

            //Lists all the avalable clients and adds the corresponding number in the list
            Console.WriteLine("Avaliable clients:");
            for (int i = 0; i < Clients.Count; i++)
            {
                ClientData C = Clients[i];
                Debug.WriteLine("test2");
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
                    Console.WriteLine("Please give just a number {0}", e.Message);
                }
            }

            handler.SetUpConnection(Clients[Userinput - 1].Adress);
            string id = handler.DestinationID;
            Console.WriteLine(handler.DestinationID);
            Console.WriteLine("ID that was returend: " + id);

            scene.InitScene();

            scene.LoadScene();

        }
    }
}
