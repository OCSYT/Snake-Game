using SnakeGame.Engine;

namespace Snake_Game.Engine
{

    public class Game
    {
        public void Run()
        {
            const float FixedDeltaTime = 0.16f;
            float Time = 0;
            Renderer MainRenderer = new Renderer(20, 20);
            while (true)
            {
                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo KeyInfo = Console.ReadKey(intercept: true);
                    if (KeyInfo.Key == ConsoleKey.W)
                    {
                        //Go up
                    }
                }

                MainRenderer.Clear(0, 0, 0);
                MainRenderer.SetPixel(10, 10 + (int)(float.Sin(Time) * 3), 255, 255, 255);
                MainRenderer.Render();
                Time += FixedDeltaTime;
                Thread.Sleep(16);
            }
        }
    }
}