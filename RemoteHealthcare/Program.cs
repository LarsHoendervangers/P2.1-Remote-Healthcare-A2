using System;
using RemoteHealthcare.UI;
using RemoteHealthcare.Simulator;

namespace RemoteHealthcare
{
    class Program
    {
        static void Main(string[] args)
        {
            ConsoleWindow consoleWindow = new ConsoleWindow();
            DataSimulator dataSimulator = new DataSimulator();

            dataSimulator.SetHeartBeatListener(consoleWindow);
            dataSimulator.SetSpeedListener(consoleWindow);
            dataSimulator.SetRPMListener(consoleWindow);
            dataSimulator.SetResitanceListener(consoleWindow);
            dataSimulator.SetDistanceListener(consoleWindow);

            consoleWindow.PrintData();
            dataSimulator.Start();
        }
    }
}
