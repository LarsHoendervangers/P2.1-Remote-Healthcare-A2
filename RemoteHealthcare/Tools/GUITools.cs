using System;

namespace RemoteHealthcare.Tools
{
    class GUITools
    {
        private static string vertical = "│";
        private static string horizontal = "─";

        // Clear the specified line in the console.
        public static void ClearLine(int line)
        {
            ClearLine(0, line);
        }

        // Clear the specified line in the console from the 'fromX' position.
        public static void ClearLine(int fromX, int line)
        {
            ClearLine(fromX, Console.BufferWidth, line);
        }

        // Clear the line from 'fromX' to 'toX' on the line 'line'.
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
