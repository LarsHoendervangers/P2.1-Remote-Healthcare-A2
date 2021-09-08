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
        DistanceLine = 5,
        InputLine = 10,
        OutputLine = 12
    }

    class ConsoleWindow : ISpeedListener, IHeartbeatListener, IRPMListener, IDistanceListener, ITimeListener
    {
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
        }

        private static void PrintBikeData()
        {
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
            Console.SetCursorPosition(0, (int) Line.SpeedLine);
            Console.Write("Speed: {0} km/h     ", speed);
        }

        public void OnHeartBeatChanged(int heartBeat)
        {
            Console.SetCursorPosition(0, (int) Line.HeartLine);
            Console.Write("Heartbeat: {0} BPM     ", heartBeat);
        }

        public void OnRPMChanged(int rpm)
        {
            Console.SetCursorPosition(0, (int) Line.RPMLine);
            Console.Write("RPM: {0}          ", rpm);
        }

        public void OnDistanceChanged(int distance)
        {
            Console.SetCursorPosition(0, (int)Line.DistanceLine);
            Console.Write("Distance: {0} m         ", distance);
        }

        public void OnElapsedTime(int time)
        {
            Console.SetCursorPosition(0, (int)Line.DistanceLine + 1);
            Console.Write("Time elapsed: {0} s         ", time);
        }
    }
}
