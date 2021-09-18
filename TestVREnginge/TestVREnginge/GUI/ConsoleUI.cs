using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
            //Set the window to be a bit wider
            Console.SetWindowSize(140, 40);

            SetupLogging();

            TunnelHandler handler = new TunnelHandler();
            //GeneralScene scene = new LoaderScene(handler);
            GeneralScene scene = new DemoScene(handler);
            GetConnection(handler);

            // Initing the scene
            scene.InitScene();

            //Starting the scene
            scene.LoadScene();

        }

        private static void GetConnection(TunnelHandler handler)
        {
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
                    Console.WriteLine("Please give just a number {0}", e.Message);
                }
            }

            handler.SetUpConnection(Clients[Userinput - 1].Adress);
            Trace.WriteLine("Connecting to server: ID that was returend: {0} \n", handler.DestinationID);
        }

        private static void SetupLogging()
        {
            //Creates the logging folder, if it already exists line is ignored
            Directory.CreateDirectory("DebugLogging");
            //Start logger
            string debugPath = $"DebugLogging/debug{DateTime.Now:dd-MM-yyyy-HH-mm-ss}.log";
            Trace.Listeners.Add(new TextWriterTraceListener(debugPath));
            Trace.AutoFlush = true;

            Trace.WriteLine(
                $"-----------------------------------------" + "\n" +
                $"             DEBUG LOGGING               " + "\n" +
                $"Started logging at: {DateTime.Now:dd-MM-yyyy-HH-mm-ss}" + "\n" +
                $"-----------------------------------------"
                );
        }
    }
}
