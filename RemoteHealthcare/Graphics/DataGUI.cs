using RemoteHealthcare.Hardware;
using RemoteHealthcare.Software;
using RemoteHealthcare.Tools;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RemoteHealthcare.Graphics
{
    class DataGUI
    {
        private static string Title = "Remote Healthcare by A2";
        private static bool IsTyping = false;

        private static int RPM_Line = 2;
        private static int Speed_Line = 3;
        private static int Heart_Line = 4;
        private static int Time_Line = 5;
        private static int Distance_Line = 6;
        private static int TotalPower_Line = 7;
        private static int CurrentPower_Line = 8;
        private static int Input_Line = 9;

        private static int CurrentDeviceLine = 2;
        private static int LongestDeviceName = 0;

        private Device device = new PhysicalDevice("Tacx Flux 00457", "Decathlon Dual HR");
        //private Device device = new SimulatedDevice();

        public DataGUI()
        {
            device.onHeartrate += DrawHeartrate;
            device.onRPM += DrawRPM;
            device.onSpeed += DrawSpeed;
            device.onDistance += DrawDistance;
            device.onElapsedTime += DrawElapsedTime;
            device.onTotalPower += DrawTotalPower;
            device.onCurrentPower += DrawCurrentPower;

            SetMessage("Searching for devices");
            PrepareGUI();
        }

        static void Main(string[] args)
        {
            new DataGUI().Start();
        }

        private void Start()
        {
            string resistance = "Resistance: ";
            int x = resistance.Length;

            string value = "";

            Console.SetCursorPosition(0, Input_Line);
            Console.Write(resistance);
            do
            {
                char character = Console.ReadKey(true).KeyChar;
                if (Char.IsNumber(character))
                {
                    if (x < Console.BufferWidth)
                    { 
                        Console.SetCursorPosition(x, Input_Line);
                        Console.Write(character);
                        value += character;
                        x++;
                    }
                } else if (Char.IsWhiteSpace(character))
                {
                    IsTyping = false;
                    x = resistance.Length;
                    value = "";
                    GUITools.ClearLine(x, 50, Input_Line);
                    // TODO: Implement an event with 'value'.
                }
            } while (true);
        }

        // Draw the basic layout in the console.
        private void PrepareGUI()
        {
            GUITools.DrawBasicLayout(Title);
            GUITools.DrawHorizontalLine(1, 0, Console.BufferWidth - 1);
            GUITools.DrawVerticalLine((Console.BufferWidth - 1) - (LongestDeviceName + 1), 0, CurrentDeviceLine);
            Console.SetCursorPosition((Console.BufferWidth - 1) - LongestDeviceName, 0);
            Console.Write("Devices");

            SetMessage("Connecting to devices...");
            Trace.Listeners.Add(new TextWriterTraceListener("debug.log"));
            Trace.AutoFlush = true;

            GUITools.DrawBasicLayout("Remote Healthcare by A2");
            Console.CursorVisible = false;

            Console.Read();
        }

        // Called by onRPM event.
        private void DrawRPM(object sender, int e)
        {
            if (!IsTyping) 
            {
                Console.SetCursorPosition(0, RPM_Line);
                Console.WriteLine($"RPM: {e}     ");
            }
        }

        // Called by onSpeed event.
        private void DrawSpeed(object sender, double e)
        {
            if (!IsTyping)
            {
                Console.SetCursorPosition(0, Speed_Line);
                Console.WriteLine($"Speed: {e.ToString("0.##")} KM/H     ");
            }
        }

        // Called by onHeartrate event.
        public void DrawHeartrate(Object sender, int heartrate)
        {
            if (!IsTyping)
            {
                Console.SetCursorPosition(0, Heart_Line);
                Console.WriteLine($"Heartrate: {heartrate} BPM     ");
            }
        }

        // Called by onElapsedTime event.
        private void DrawElapsedTime(object sender, double e)
        {
            if (!IsTyping)
            {
                Console.SetCursorPosition(0, Time_Line);
                Console.WriteLine($"Elapsed time: {e} seconds     ");
            }
        }

        // Called by onDistance event.
        private void DrawDistance(object sender, double e)
        {
            if (!IsTyping)
            {
                Console.SetCursorPosition(0, Distance_Line);
                Console.WriteLine($"Distance: {e} m     ");
            }
        }

        // Called by onTotalPower event.
        private void DrawTotalPower(object sender, int e)
        {
            Console.SetCursorPosition(0, TotalPower_Line);
            Console.WriteLine($"Total power: {e} Watt     ");
        }

        // Called by onCurrentPower event.
        private void DrawCurrentPower(object sender, int e)
        {
            Console.SetCursorPosition(0, CurrentPower_Line);
            Console.WriteLine($"Current power: {e} Watt     ");
        }

        // Prints device on the right side of the console.
        public static void AddDeviceToList(string device)
        {
            int x = Console.BufferWidth - 1;
            int length = device.Length - 1;

            if (length > LongestDeviceName) LongestDeviceName = length;

            foreach (char c in device)
            {
                Console.SetCursorPosition(x, CurrentDeviceLine);
                Console.Write(device[length]);
                // Check for out of bounds exception.
                if (x - 1 >= 0) x--;

                // Check for out of bounds exception.
                if (length - 1 >= 0) length--;
            }
            // Increase the current line for the next device.
            CurrentDeviceLine++;
        }

        // Print a message to the top of the screen.
        public static void SetMessage(string msg)
        {
            String prefix = "Message: ";

            // Clear the line (just in case)
            int startX = Title.Length + prefix.Length;
            GUITools.ClearLine(startX, (Console.BufferWidth - 1) - (LongestDeviceName + 1), 0);

            // Print the new message.
            Console.SetCursorPosition(Title.Length + prefix.Length, 0);
            Console.Write(prefix + msg);
        }
    }
}

