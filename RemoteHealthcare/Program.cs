using RemoteHealthcare.Tools;
using System;

namespace RemoteHealthcare
{
    class Program
    {
        static void Main(string[] args)
        {
            DrawLayout();
        }

        private static void DrawLayout()
        {
            Console.SetCursorPosition(0, 0);
            Console.WriteLine("Remote Healthcare by A2");
            GUITools.DrawHorizontalLine(1, 0, Console.BufferWidth);
        }
    }
}