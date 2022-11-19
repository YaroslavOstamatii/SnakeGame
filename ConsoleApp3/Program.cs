using ConsoleApp3;
using System;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;

internal class Program
{
    static readonly Random random = new Random();
    const int MapWidth = 80;
    const int MapHeight = 40;
    const int FrameMs = 200;
    static int score = 0;

    const ConsoleColor BorderColor = ConsoleColor.Gray;
    const ConsoleColor headColor = ConsoleColor.Blue;
    const ConsoleColor bodyColor = ConsoleColor.Cyan;
    const ConsoleColor foodColor = ConsoleColor.Green;

    public static Queue<Pixel> Border { get; } = new Queue<Pixel>();

    public static void Main(string[] args)
    {
        Console.SetWindowSize(MapWidth, MapHeight);
        Console.SetBufferSize(MapWidth, MapHeight);
        Console.CursorVisible = false;
        var user = new User();

        while (true)
        {
            using (SnakeContext db = new SnakeContext())
            {
                Console.Write("Vvedi imya: ");
                user.Name = Console.ReadLine();
                StartGame();
                Thread.Sleep(1000);
                Console.ReadLine();
                user.Score = score;
                user.dateTime = DateTime.Now;
                db.Users.Add(user);
                db.SaveChanges();
                var users = db.Users.ToList();
                Console.WriteLine("Список объектов:");
                foreach (User u in users)
                {
                    Console.WriteLine($"{u.Id}.{u.Name} - {u.dateTime} - {u.Score}");
                }
            }
        }
    }

    static public void DrawBorder()
    {
        for (int i = 0; i < MapWidth/2;)
        {
            Border.Enqueue(new Pixel(i, 15, BorderColor));
            Border.Enqueue(new Pixel(i+40, 30, BorderColor));
            i += 2;
        }
        Draw();
        void Draw()
        {
            
            foreach (Pixel pixel in Border)
            {
                pixel.Draw();
            }
        }
    }
    static Direction ReadMovement(Direction currentDirection)
    {
        if (!Console.KeyAvailable) return currentDirection;
        ConsoleKey key = Console.ReadKey(true).Key;
        currentDirection = key switch
        {
            ConsoleKey.UpArrow when currentDirection != Direction.Down => Direction.Up,
            ConsoleKey.DownArrow when currentDirection != Direction.Up => Direction.Down,
            ConsoleKey.RightArrow when currentDirection != Direction.Left => Direction.Right,
            ConsoleKey.LeftArrow when currentDirection != Direction.Right => Direction.Left,
            _ => currentDirection
        };
        return currentDirection;
    }
    static Pixel GenFood(Snake snake, Queue<Pixel> BodyBorder)
    {
        int bufferWidth=random.Next(2, MapWidth-2) / 2;
        int bufferHeight=random.Next(2, MapHeight-2) / 2;

        

        Pixel food;
        do
        {
            food = new Pixel(bufferWidth*2,bufferHeight*2 , foodColor);
        } while (snake.Head.X==food.X && snake.Head.Y == food.Y || snake.Body.Any(b=>b.X==food.X && b.Y == food.Y) || BodyBorder.Any(b => b.X == food.X && b.Y == food.Y));
        return food;
    }
    static void StartGame()
    {
        Console.Clear();
        DrawBorder();
        Direction currentMovement = Direction.Right;
        var snake = new Snake(20, 10, headColor, bodyColor);
        Pixel food = GenFood(snake, Border);
        food.Draw();
        Stopwatch sw = new Stopwatch();
        while (true)
        {
            sw.Restart();
            Direction oldMovement = currentMovement;
            while (sw.ElapsedMilliseconds <= FrameMs)
            {
                if (oldMovement == currentMovement)
                {
                    currentMovement = ReadMovement(currentMovement);
                }

            }
            if(snake.Head.X==food.X && snake.Head.Y == food.Y)
            {
                snake.Move(currentMovement, true);
                food = GenFood(snake,Border);
                food.Draw();
                score++;
            }
            else
            {
                snake.Move(currentMovement);
            }
                


            if (snake.Body.Any(b => b.X == snake.Head.X && b.Y == snake.Head.Y)
                || Border.Any(b => b.X == snake.Head.X && b.Y == snake.Head.Y))
                break;
        }
        snake.Clear();
        Console.SetCursorPosition(MapWidth / 3, MapHeight / 3);
        Console.WriteLine("gameOver,tou score {0}",score);
    }
}