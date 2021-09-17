using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestVREngine.Tunnel;
using TestVREngine.Util;

namespace TestVREngine.Scene
{
    class LoaderScene : GeneralScene
    {
        private string FileName;

        public LoaderScene(TunnelHandler handler) : base(handler)
        {

        }

        public LoaderScene(TunnelHandler handler, string fileName) : base(handler)
        {
            this.FileName = fileName;
        }

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
            this.FileName = Console.ReadLine();
        }

        public override void LoadScene()
        {
            if (FileName != null)
            {

                this.Handler.SendToTunnel(JSONCommandHelper.WrapLoadScene(this.FileName), OnLoadCallback);
                Console.WriteLine("the filename {0} has been send to the server", this.FileName);

            }

        }

        private void OnLoadCallback(string message)
        {
            Console.WriteLine("Server responded to load command");
        }
    }
}
