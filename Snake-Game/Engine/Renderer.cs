using System;
using System.Text;

namespace SnakeGame.Engine
{
    public class Pixel
    {
        public byte R { get; }
        public byte G { get; }
        public byte B { get; }

        public Pixel(byte R, byte G, byte B)
        {
            this.R = R;
            this.G = G;
            this.B = B;
        }
    }

    public class Renderer
    {
        public int Width { get; }
        public int Height { get; }
        public Pixel[,] Buffer { get; }
        private readonly StringBuilder OutputBuilder = new();

        public Renderer(int width, int height)
        {
            Width = width;
            Height = height;
            Buffer = new Pixel[Height, Width];
            Console.OutputEncoding = Encoding.UTF8;
            Console.CursorVisible = false;
            Console.Clear();
        }

        public void SetPixel(int X, int Y, byte R, byte G, byte B)
        {
            if (X >= 0 && X < Width && Y >= 0 && Y < Height)
            {
                Buffer[Y, X] = new Pixel(R, G, B);
            }
        }

        public void Clear(byte R = 0, byte G = 0, byte B = 0)
        {
            var ClearPixel = new Pixel(R, G, B);
            for (int Y = 0; Y < Height; Y++)
            {
                for (int X = 0; X < Width; X++)
                {
                    Buffer[Y, X] = ClearPixel;
                }
            }
        }

        public void Render()
        {
            OutputBuilder.Clear();
            OutputBuilder.Append("\u001b[H");
            for (int Y = 0; Y < Height; Y++)
            {
                OutputBuilder.Append($"\u001b[{Y + 1};1H");
                for (int X = 0; X < Width; X++)
                {
                    var Pixel = Buffer[Y, X] ?? new Pixel(0, 0, 0);
                    OutputBuilder.Append($"\u001b[38;2;{Pixel.R};{Pixel.G};{Pixel.B}m██ ");
                }
            }
            OutputBuilder.Append("\u001b[0m");
            Console.Write(OutputBuilder);
        }
    }
}