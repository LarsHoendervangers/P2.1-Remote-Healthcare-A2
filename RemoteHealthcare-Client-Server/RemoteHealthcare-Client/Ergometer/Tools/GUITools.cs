using System;
using System.Text;

namespace RemoteHealthcare_Client.Ergometer.Tools
{
    class GUITools
    {
        private static readonly string vertical = "│";
        private static readonly string horizontal = "─";
        public static readonly string crossing = "┼";

        /// <summary>
        /// Draw the basic UI
        /// </summary>
        /// <param name="title"></param>
        public static void DrawBasicLayout(string title)
        {
            Console.SetCursorPosition(0, 0);
            Console.WriteLine(title);
        }

        /// <summary>
        /// Clear the specified line in the console.
        /// </summary>
        /// <param name="line"></param>
        public static void ClearLine(int line)
        {
            ClearLine(0, line);
        }

        /// <summary>
        /// Clear the specified line in the console from the 'fromX' position.
        /// </summary>
        /// <param name="fromX"></param>
        /// <param name="line"></param>
        public static void ClearLine(int fromX, int line)
        {
            ClearLine(fromX, Console.BufferWidth, line);
        }

        /// <summary>
        /// Clear the line from 'fromX' to 'toX' on the line 'line'.
        /// </summary>
        /// <param name="fromX"></param>
        /// <param name="toX"></param>
        /// <param name="line"></param>
        public static void ClearLine(int fromX, int toX, int line)
        {
            for (int x = fromX; x < toX; x++)
            {
                if (x < Console.BufferWidth && line < Console.BufferHeight)
                {
                    Console.SetCursorPosition(x, line);
                    Console.Write(" ");
                }
            }
            Console.SetCursorPosition(0, line);
        }

        /// <summary>
        /// Draw a vertical line from position x, startY to x, endY.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="startY"></param>
        /// <param name="endY"></param>
        public static void DrawVerticalLine(int x, int startY, int endY)
        {
            // Reverse start and end position if they are in the wrong order.
            if (endY < startY)
            {
                int y = startY;
                startY = endY;
                endY = y;
            }

            // Draw the line.
            for (int y = startY; y < endY; y++)
            {
                if (x < Console.BufferWidth && y < Console.BufferHeight)
                {
                    Console.SetCursorPosition(x, y);
                    Console.Write(vertical);
                }
            }
        }

        /// <summary>
        /// Draw a horizontal line from startX to endX on line.
        /// </summary>
        /// <param name="line"></param>
        /// <param name="startX"></param>
        /// <param name="endX"></param>
        public static void DrawHorizontalLine(int line, int startX, int endX)
        {
            // Reverse start and end position if they are in the wrong order.
            if (endX < startX)
            {
                int x = startX;
                startX = endX;
                endX = x;
            }

            // Draw the line.
            for (int x = startX; x < endX; x++)
            {
                Console.SetCursorPosition(x, line);
                Console.Write(horizontal);
            }
        }
    }
}
