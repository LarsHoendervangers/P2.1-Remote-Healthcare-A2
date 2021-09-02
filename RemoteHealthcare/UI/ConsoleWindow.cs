using System;
using RemoteHealthcare.UI.Interfaces;

namespace RemoteHealthcare.UI
{
    // Definition of the line numbers in the console.
    enum Line : int
    {
        BrandLine = 0,
        RPMLine = 2,
        SpeedLine = 3,
        HeartLine = 4,
        ResitanceLine = 5,
        DistanceLine = 6,
        InputLine = 10,
        OutputLine = 12
    }

    class ConsoleWindow : ISpeedListener, IHeartbeatListener, IRPMListener, IResistanceListener, IDistanceListener
    {
        private readonly string enterCommandMsg = "Enter a command: ";
        public ConsoleWindow()
        {
            Console.CursorVisible = false;
        }

        public void PrintData()
        {
            PrintBikeData();
            DrawUI();
        }

        private void DrawUI()
        {
            Console.SetCursorPosition(0, (int) Line.BrandLine);
            Console.WriteLine("Remote Healthcare by A2");

            // Draw a divider line between the data and the input line.
            for (int x = 0; x < Console.BufferWidth; x++)
            {
                Console.SetCursorPosition(x, (int) Line.InputLine - 1);
                Console.Write("-");
            }

            // Clear the input line and reset the cursor. After that print a msg.
            // USER INPUT IS CURRENTLY DISABLED
            //Console.SetCursorPosition(0, (int) Line.InputLine);
            //Console.Write(this.enterCommandMsg);
        }

        private static void PrintBikeData()
        {
            Console.SetCursorPosition(0, (int) Line.RPMLine);
            Console.Write("Power: 5000 Watt");
            Console.SetCursorPosition(0, (int) Line.HeartLine + 1);
        }

        private void ClearLine(int line)
        {
            ClearLine(0, line);
        }

        private void ClearLine(int fromX, int line)
        {
            for (int x = fromX; x < Console.BufferWidth; x++)
            {
                Console.SetCursorPosition(x, line);
                Console.Write(" ");
            }
            Console.SetCursorPosition(0, line);
        }

        public void OnSpeedChanged(double speed)
        {
            //ClearLine(7, (int) Line.SpeedLine);
            Console.SetCursorPosition(0, (int) Line.SpeedLine);
            Console.Write("Speed: {0} m/s     ", speed);
        }

        public void OnHeartBeatChanged(int heartBeat)
        {
            //ClearLine(11, (int) Line.HeartLine);
            Console.SetCursorPosition(0, (int) Line.HeartLine);
            Console.Write("Heartbeat: {0} BPM     ", heartBeat);
        }

        public void OnRPMChanged(int rpm)
        {
            //ClearLine(5, (int)Line.RPMLine);
            Console.SetCursorPosition(0, (int) Line.RPMLine);
            Console.Write("RPM: {0}          ", rpm);
        }

        public void OnResistanceChanged(int resistance)
        {
            Console.SetCursorPosition(0, (int)Line.ResitanceLine);
            Console.Write("Reistance: {0} %   ", resistance);
        }

        public void OnDistanceChanged(int distance)
        {
            Console.SetCursorPosition(0, (int)Line.DistanceLine);
            Console.Write("Distance: {0} m         ", distance);
        }
    }
}
