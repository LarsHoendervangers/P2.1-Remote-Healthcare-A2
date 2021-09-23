using RemoteHealthcare_Client.ClientVREngine.GUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoteHealthcare_Client.ClientVREngine
{
    class Program
    {

        /// <summary>
        /// Main method to start the program
        /// </summary>
        /// <param name="args">Main args</param>
        public static void Main(string[] args)
        {
            ConsoleUI consoleUI = new ConsoleUI();
            ConsoleUI.Run();
        }

    }
}
