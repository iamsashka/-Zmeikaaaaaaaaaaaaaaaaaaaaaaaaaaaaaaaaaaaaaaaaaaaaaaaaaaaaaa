using System;
using System.Collections.Generic;
using System.Threading;

enum Border
{
    Right = 79, // Максимальная правая граница
    Bottom = 24 // Максимальная нижняя граница
}

class SnakeGame
{
    private List<int> snakeX; // Координаты X змейки
    private List<int> snakeY; // Координаты Y змейки
    private int foodX; // Координата X еды
    private int foodY; // Координата Y еды
    private bool isGameRunning; // Флаг для проверки состояния игры
    private Direction direction; // Текущее направление движения змейки

    public SnakeGame()
    {
        snakeX = new List<int>() { 10, 9, 8 }; // Изначальные координаты змейки
        snakeY = new List<int>() { 10, 10, 10 };
        foodX = new Random().Next(1, (int)Border.Right - 1); // Случайные координаты еды
        foodY = new Random().Next(1, (int)Border.Bottom - 1);
        isGameRunning = true;
        direction = Direction.Right; // Змейка начинает движение вправо
    }

    public void Run()
    {
        Console.CursorVisible = false;
        Console.SetWindowSize((int)Border.Right + 1, (int)Border.Bottom + 1);

        DrawBorder();
        DrawFood();
        DrawSnake();

        Thread inputThread = new Thread(ReadInput);
        inputThread.Start();

        while (isGameRunning)
        {
            MoveSnake();
            CheckCollision();
            Thread.Sleep(100);
        }

        Console.Clear();
        Console.WriteLine("Game Over!");
        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
    }

    private void DrawBorder()
    {
        for (int x = 0; x <= (int)Border.Right; x++)
        {
            Console.SetCursorPosition(x, 0);
            Console.Write("#");
            Console.SetCursorPosition(x, (int)Border.Bottom);
            Console.Write("#");
        }

        for (int y = 0; y <= (int)Border.Bottom; y++)
        {
            Console.SetCursorPosition(0, y);
            Console.Write("#");
            Console.SetCursorPosition((int)Border.Right, y);
            Console.Write("#");
        }
    }

    private void DrawFood()
    {
        Console.SetCursorPosition(foodX, foodY);
        Console.Write("*");
    }

    private void DrawSnake()
    {
        for (int i = 0; i < snakeX.Count; i++)
        {
            Console.SetCursorPosition(snakeX[i], snakeY[i]);
            Console.Write(i == 0 ? "@" : "*");
        }
    }

    private void MoveSnake()
    {
        int snakeHeadX = snakeX[0];
        int snakeHeadY = snakeY[0];

        switch (direction)
        {
            case Direction.Up:
                snakeHeadY--;
                break;
            case Direction.Down:
                snakeHeadY++;
                break;
            case Direction.Left:
                snakeHeadX--;
                break;
            case Direction.Right:
                snakeHeadX++;
                break;
        }

        snakeX.Insert(0, snakeHeadX);
        snakeY.Insert(0, snakeHeadY);

        if (snakeHeadX == foodX && snakeHeadY == foodY)
        {
            foodX = new Random().Next(1, (int)Border.Right - 1);
            foodY = new Random().Next(1, (int)Border.Bottom - 1);
        }
        else
        {
            snakeX.RemoveAt(snakeX.Count - 1);
            snakeY.RemoveAt(snakeY.Count - 1);
        }

        Console.SetCursorPosition(snakeX[1], snakeY[1]);
        Console.Write("*");
        Console.SetCursorPosition(snakeX[0], snakeY[0]);
        Console.Write("@");
    }

    private void CheckCollision()
    {
        int snakeHeadX = snakeX[0];
        int snakeHeadY = snakeY[0];

        if (snakeHeadX == 0 || snakeHeadX == (int)Border.Right || snakeHeadY == 0 || snakeHeadY == (int)Border.Bottom)
        {
            isGameRunning = false;
        }

        for (int i = 1; i < snakeX.Count; i++)
        {
            if (snakeX[i] == snakeHeadX && snakeY[i] == snakeHeadY)
            {
                isGameRunning = false;
                break;
            }
        }
    }

    private void ReadInput()
    {
        while (isGameRunning)
        {
            if (Console.KeyAvailable)
            {
                ConsoleKeyInfo key = Console.ReadKey(true);

                switch (key.Key)
                {
                    case ConsoleKey.UpArrow:
                        if (direction != Direction.Down)
                            direction = Direction.Up;
                        break;
                    case ConsoleKey.DownArrow:
                        if (direction != Direction.Up)
                            direction = Direction.Down;
                        break;
                    case ConsoleKey.LeftArrow:
                        if (direction != Direction.Right)
                            direction = Direction.Left;
                        break;
                    case ConsoleKey.RightArrow:
                        if (direction != Direction.Left)
                            direction = Direction.Right;
                        break;
                }
            }
        }
    }
}

enum Direction
{
    Up,
    Down,
    Left,
    Right
}
