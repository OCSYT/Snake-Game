using SnakeGame.Engine;
using System.Numerics;
using System.Diagnostics;

namespace Snake_Game.Engine
{
    public class Game
    {
        private float Time = 0;
        private Renderer MainRenderer = new Renderer(20, 20);
        private Vector2 Position = Vector2.Zero;
        private Vector2 Movement = Vector2.UnitY;
        private List<Vector2> ApplePositions = new List<Vector2>();
        private int Score = 0;
        private void MakeApple()
        {
            Random Random = new Random();
            Vector2 ApplePosition = new Vector2(Random.Next(MainRenderer.Width), Random.Next(MainRenderer.Height));
            ApplePositions.Add(ApplePosition);
        }

        public void Run()
        {
            Position = new Vector2(MainRenderer.Width / 2, MainRenderer.Height / 2); 
            MakeApple();
            

            Stopwatch Stopwatch = new Stopwatch();
            Stopwatch.Start();

            while (true)
            {
                float DeltaTime = (float)Stopwatch.Elapsed.TotalSeconds;
                Stopwatch.Restart();

                // Stats
                Console.WriteLine("\n");
                Console.WriteLine($"FPS: {float.Floor(1/DeltaTime)}");
                Console.WriteLine("\n");
                Console.WriteLine($"Score: {Score}");
                MainRenderer.Clear(0, 0, 0);

                // Movement Keypresses
                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo KeyInfo = Console.ReadKey(intercept: true);
                    switch (KeyInfo.Key)
                    {
                        case ConsoleKey.W:
                            Movement = -Vector2.UnitY;
                            break;
                        case ConsoleKey.A:
                            Movement = -Vector2.UnitX;
                            break;
                        case ConsoleKey.S:
                            Movement = Vector2.UnitY;
                            break;
                        case ConsoleKey.D:
                            Movement = Vector2.UnitX;
                            break;
                    }
                }

                Position += Movement * DeltaTime * 10f;

                foreach (Vector2 ApplePosition in ApplePositions.ToList())
                {
                    int X = (int)MathF.Floor(ApplePosition.X);
                    int Y = (int)MathF.Floor(ApplePosition.Y);

                    MainRenderer.SetPixel(X, Y, 255, 0, 0);

                    // if contact apple
                    if (X == (int)MathF.Floor(Position.X) && Y == (int)MathF.Floor(Position.Y))
                    {
                        ApplePositions.Remove(ApplePosition);
                        Score++;
                        MakeApple();
                    }
                }

              

                Position.X = Math.Clamp(Position.X, 0, MainRenderer.Width - 1);
                Position.Y = Math.Clamp(Position.Y, 0, MainRenderer.Height - 1);

                MainRenderer.SetPixel((int)Position.X, (int)Position.Y, 0, 255, 0);
                MainRenderer.Render();
                Time += DeltaTime;
            }
        }
    }
}
