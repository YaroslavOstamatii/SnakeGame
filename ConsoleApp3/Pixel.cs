using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp3
{
    public readonly struct Pixel
    {
        private const string PixelChar = "██"; 
        public Pixel(int x, int y, ConsoleColor color)
        {
            X = x;
            if (x >= 79)
            {
                X = 0;
            }
            else if (x < 0)
            {
                X = 79;
            }

            Y = y;
            if (y >= 39)
            {
                Y = 0;
            }
            else if (y < 0)
            {
                Y = 39;
            }

            
            Color = color;
        }

        public int X { get; }
        public int Y { get; }
        public ConsoleColor Color { get; }

        public void Draw()
        {
            
            
            Console.ForegroundColor = Color;
           Console.SetCursorPosition(X, Y);
           Console.Write(PixelChar);
        }
        public void Clear()
        {
            Console.SetCursorPosition(X, Y);
            Console.Write("  ");
        }

    }
}
