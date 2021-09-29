using RemoteHealthcare.ClientVREngine.Util;
using RemoteHealthcare_Client.ClientVREngine.Tunnel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoteHealthcare_Client.ClientVREngine.Scene
{

    /// <summary>
    /// Scene class that lets a saved file be loaded by the server
    /// </summary>
    class LoaderScene : GeneralScene
    {
        private string FileName;

        /// <summary>
        /// Base constructor for LoaderScene.
        /// The program asks the user to enter file to be loaded on Init
        /// </summary>
        /// <param name="handler">The TunnelHandler that the scene needs to communicate to the server with</param>
        public LoaderScene(TunnelHandler handler) : base(handler)
        {

        }

        /// <summary>
        /// Construvtor that lets the file that needs to be loaded be specified in constructor
        /// </summary>
        /// <param name="handler">The TunnelHandler that the scene needs to communicate to the server with</param>
        /// <param name="fileName">The filename of the file to be loaded</param>
        public LoaderScene(TunnelHandler handler, string fileName) : base(handler)
        {
            FileName = fileName;
        }

        /// <summary>
        /// Asks the user to enter a filename that needs to be loaded
        /// </summary>
        public override void InitScene()
        {
            // BLOCKING CALL, program will pause until user input
            Console.WriteLine(
                "\t----------------------------------" + "\n" +
                "\t             SCENE LOADER         " + "\n" +
                "\t  enter the filename to be loaded " + "\n" +
                "\t----------------------------------"
                );

            Console.Write("Enter the file name: ");
            FileName = Console.ReadLine();
        }

        /// <summary>
        /// Tells the server to load the scene, if filename is not null
        /// </summary>
        public override void LoadScene()
        {
            if (FileName != null)
            {

                Handler.SendToTunnel(JSONCommandHelper.WrapLoadScene(FileName), OnLoadCallback);
                Console.WriteLine("the filename {0} has been send to the server", FileName);

            }

        }

        /// <summary>
        /// Callback method for server response from loading a scene
        /// </summary>
        /// <param name="message">The message from the server</param>
        private void OnLoadCallback(string message)
        {
            // TODO ask Senior Developer about return codes of engines, returns with no existing file
            Console.WriteLine("Server responded to load command");
            Trace.WriteLine($"LoaderScene: Server responded to load command: {message}");
        }
    }
}
