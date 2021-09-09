using RemoteHealthcare.Tools;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoteHealthcare.Graphics
{
    class SimGUI
    {
        private static int Input_line = 10;
        public void Start()
        {
            //Start logger
            Trace.Listeners.Add(new TextWriterTraceListener("debug.log"));
            Trace.AutoFlush = true;

            // Set up the GUI.
            GUITools.DrawBasicLayout("Remote Healthcare by A2 - Simulator");
            PrintCommands();

            GUITools.DrawHorizontalLine(Input_line - 1, 0, Console.BufferWidth);
            Console.SetCursorPosition(0, Input_line);

            // Read user input.
            while (true)
            {
                OnCommand(Console.ReadLine());
            }
        }

        private void PrintCommands()
        {
            StringBuilder commands = new StringBuilder("The following commands are available:");
            commands.Append("\n- resistance <value> \tSets the resistance value on the bike.");
            commands.Append("\n- quit \t\t\tExits the simulator.");
            Console.SetCursorPosition(0, 2);
            Console.WriteLine(commands.ToString());
        }

        private void OnCommand(string line)
        {
            switch (line)
            {
                case "q":
                case "quit":
                    Environment.Exit(0);
                    break;
                case "resistance":
                    break;
            }
            
            GUITools.ClearLine(Input_line);
            Console.SetCursorPosition(0, Input_line);
        }
    }
}
