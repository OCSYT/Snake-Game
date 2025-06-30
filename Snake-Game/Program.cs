using System;
using Snake_Game.Engine;

namespace Snake_Game
{
    static class Program
    {
        static void Main(string[] args)
        {
            Game GameInstance = new Game();
            GameInstance.Run();
        }
    }
}