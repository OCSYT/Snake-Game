using SnakeGame.Engine;
using System.Numerics;
using System.Diagnostics;

namespace Snake_Game.Engine
{
    public class Game
    {
        // statistics
        private float _time = 0;
        private int _score = 0;
        
        // game setup
        private Renderer _mainRenderer = new Renderer(20, 20);
        private Vector2 _position = Vector2.Zero;
        private Vector2 _movement = -Vector2.UnitY;
        
        // apple setup
        private List<Vector2> _applePositions = new List<Vector2>();
        private void MakeApple()
        {
            // add apple to random square
            Random random = new Random();
            Vector2 applePosition = new Vector2(random.Next(_mainRenderer.Width), random.Next(_mainRenderer.Height));
            _applePositions.Add(applePosition);
        }
        
        // snake setup
        private List<Vector2> _snakePositions = new List<Vector2>();
        
        private void UpdatePosition()
        {
            // checks if snake's head is not equal to desired destination,
            // adds positions as new snake head.
            // default/in beginning = true
            if (
                _snakePositions.Count == 0 
                || (int)MathF.Floor(_snakePositions[0].X) != (int)MathF.Floor(_position.X)
                || (int)MathF.Floor(_snakePositions[0].Y) != (int)MathF.Floor(_position.Y)
            )
            {
                _snakePositions.Insert(0, _position);
            }
            
            // removes last item in the list while it's longer than 1-based score (length val)
            while (_snakePositions.Count > _score + 1)
            {
                _snakePositions.RemoveAt(_snakePositions.Count - 1);
            }
        }

        private void RenderPositions()
        {
            // renders every snake square
            for (int i = 0; i < _snakePositions.Count; i++)
            {
                Vector2 snakePosition = _snakePositions[i];

                // colour creation — clamps to prevent overflow
                byte red = (byte)Math.Clamp(0 + (i * 10), 0, 255);
                byte green = 0;
                byte blue = (byte)Math.Clamp(255 - (i * 10), 0, 255);

                // render square
                _mainRenderer.SetPixel((int)snakePosition.X, (int)snakePosition.Y, red, green, blue);
            }
        }


        private void Die()
        {
            Console.WriteLine("You Died!");
        }

        // game
        public void Run()
        {
            // init game && apple setup
            _position = new Vector2(_mainRenderer.Width / 2, _mainRenderer.Height / 2); 
            MakeApple();
            

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            while (true)
            {
                Task.Delay(16).Wait(); //Wait 16ms to limit framerate to 60fps
                
                float deltaTime = (float)stopwatch.Elapsed.TotalSeconds;
                stopwatch.Restart();

                // display statistics
                Console.WriteLine("\n");
                Console.WriteLine($"FPS: {float.Floor(1/deltaTime)}");
                Console.WriteLine("\n");
                Console.WriteLine($"Score: {_score}");
                _mainRenderer.Clear(0, 0, 0);

                // movement keypresses
                Vector2 prevMove = _movement;
                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo keyInfo = Console.ReadKey(intercept: true);
                    switch (keyInfo.Key)
                    {
                        case ConsoleKey.W:
                        case ConsoleKey.UpArrow:
                            _movement = -Vector2.UnitY;
                            break;
                        case ConsoleKey.A:
                        case ConsoleKey.LeftArrow:
                            _movement = -Vector2.UnitX;
                            break;
                        case ConsoleKey.S:
                        case ConsoleKey.DownArrow:
                            _movement = Vector2.UnitY;
                            break;
                        case ConsoleKey.D:
                        case ConsoleKey.RightArrow:
                            _movement = Vector2.UnitX;
                            break;
                    }
                }

                // prevents going directly backwards
                if (Vector2.Dot(_movement, prevMove) == -1 
                    && _score != 0)
                {
                    _movement = -_movement;
                }

                // update position with new movement
                _position += _movement * deltaTime * 10f;

                //apple handling
                foreach (Vector2 applePosition in _applePositions.ToList())
                {
                    // render each apple
                    int x = (int)MathF.Floor(applePosition.X);
                    int y = (int)MathF.Floor(applePosition.Y);
                    _mainRenderer.SetPixel(x, y, 255, 0, 0);

                    // if contact apple
                    if (x == (int)MathF.Floor(_position.X) && y == (int)MathF.Floor(_position.Y))
                    {
                        _applePositions.Remove(applePosition);
                        // adds to score, ergo adds to snake length
                        _score++;
                        MakeApple();
                    }
                }

                
                int headX = (int)MathF.Floor(_position.X);
                int headY = (int)MathF.Floor(_position.Y);
                
                if (_mainRenderer.Height == headY || _mainRenderer.Width == headX
                    || headY == -1  || headX == -1)
                {
                    Die();
                    // exit game loop
                    return;
                }
                
                for (int i = 1; i < _snakePositions.Count; i++)
                {
                    int bodyX = (int)MathF.Floor(_snakePositions[i].X);
                    int bodyY = (int)MathF.Floor(_snakePositions[i].Y);

                    if (bodyX == headX && bodyY == headY)
                    {
                        Die();
                        // exit game loop
                        return; 
                    }
                }

                
                
                UpdatePosition();
                RenderPositions();
                
                _mainRenderer.Render();
                
                _time += deltaTime;
            }
        }
    }
}
