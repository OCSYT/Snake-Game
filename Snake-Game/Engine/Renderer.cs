using System;
using System.Text;

namespace Snake_Game.Engine
{
    public class Pixel
    {
        public byte R { get; }
        public byte G { get; }
        public byte B { get; }

        public Pixel(byte r, byte g, byte b)
        {
            R = r;
            G = g;
            B = b;
        }

        public bool Equals(Pixel other)
        {
            if (other == null) return false;
            return R == other.R && G == other.G && B == other.B;
        }
    }

    public class Renderer
    {
        private int Width { get; }
        private int Height { get; }

        private Pixel[,] FrontBuffer { get; }
        private Pixel[,] BackBuffer { get; }

        private readonly StringBuilder OutputBuilder = new();

        public Renderer(int width, int height)
        {
            Width = width;
            Height = height;
            FrontBuffer = new Pixel[Height, Width];
            BackBuffer = new Pixel[Height, Width];

            Console.OutputEncoding = Encoding.UTF8;
            Console.CursorVisible = false;
            Console.Clear();
        }

        public void SetPixel(int x, int y, byte r, byte g, byte b)
        {
            if (x >= 0 && x < Width && y >= 0 && y < Height)
            {
                BackBuffer[y, x] = new Pixel(r, g, b);
            }
        }

        public void Clear(byte r = 0, byte g = 0, byte b = 0)
        {
            var ClearPixel = new Pixel(r, g, b);
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    BackBuffer[y, x] = ClearPixel;
                }
            }
        }

        public void Render()
        {
            OutputBuilder.Clear();
            OutputBuilder.Append("\u001b[H");

            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    Pixel Current = BackBuffer[y, x] ?? new Pixel(0, 0, 0);
                    Pixel Previous = FrontBuffer[y, x];

                    if (Previous == null || !Current.Equals(Previous))
                    {
                        OutputBuilder.Append($"\u001b[{y + 1};{x + 1}H"); // Move cursor
                        OutputBuilder.Append($"\u001b[38;2;{Current.R};{Current.G};{Current.B}m█");
                        FrontBuffer[y, x] = Current;
                    }
                }
            }

            OutputBuilder.Append("\u001b[0m");
            Console.Write(OutputBuilder.ToString());
        }
    }
}
