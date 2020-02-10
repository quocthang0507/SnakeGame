using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using static System.Console;

namespace SnakeGame_Console
{
	public enum Direction
	{
		Up,
		Down,
		Right,
		Left
	}

	public class Game
	{
		public int Width { get; set; }
		public int Height { get; set; }
		public string Name { get; set; }
		public int Speed { get; set; }

		private const ConsoleColor textColor = ConsoleColor.Yellow;
		private const ConsoleColor borderColor = ConsoleColor.DarkYellow;
		private const ConsoleColor berryColor = ConsoleColor.Cyan;
		private const ConsoleColor headColor = ConsoleColor.Green;
		private const ConsoleColor bodyColor = ConsoleColor.Red;
		private const char snakePixel = '*';
		private const char berryPixel = '0';
		private const char verticalBorder = '|';
		private const char horizonBorder = '_';
		private const char blank = ' ';

		private Pixel head;
		private Pixel berry;
		private List<Pixel> body;
		private Direction currentMovement;
		private int score;
		private Random random;

		public Game(int width, int height, string name)
		{
			this.Width = width;
			this.Height = height;
			this.Name = name;
		}

		public Game(int width, int height, string name, int speed = 400)
		{
			Width = width;
			Height = height;
			Name = name;
			Speed = speed;
		}

		private void DrawPixel(Pixel pixel)
		{
			SetCursorPosition(pixel.Left, pixel.Top);
			ForegroundColor = pixel.ScreenColor;
			Write(pixel.Symbol);
			SetCursorPosition(0, 0);
			ForegroundColor = ConsoleColor.White;
		}

		private void DrawBorder()
		{
			ForegroundColor = borderColor;
			WriteLine(new string(horizonBorder, Width));
			for (int i = 1; i < Height; i++)
			{
				SetCursorPosition(0, i);
				Write(verticalBorder);
				SetCursorPosition(Width, i);
				Write(verticalBorder);
			}
			SetCursorPosition(1, Height - 1);
			WriteLine(new string(horizonBorder, Width - 1));
		}

		private void ClearPixel(Pixel pixel)
		{
			if (pixel.Top == Height - 1)
				DrawPixel(new Pixel(pixel.Left, pixel.Top, ConsoleColor.DarkYellow, horizonBorder));
			else
				DrawPixel(new Pixel(pixel.Left, pixel.Top, ConsoleColor.DarkYellow, blank));
		}

		private void DrawBody(List<Pixel> body)
		{
			for (int i = 0; i < body.Count; i++)
			{
				DrawPixel(body[i]);
			}
		}

		private Direction ReadMovement(Direction movement)
		{
			if (KeyAvailable)
			{
				var key = ReadKey(true).Key;
				if (key == ConsoleKey.UpArrow && movement != Direction.Down)
					movement = Direction.Up;
				else if (key == ConsoleKey.DownArrow && movement != Direction.Up)
					movement = Direction.Down;
				else if (key == ConsoleKey.LeftArrow && movement != Direction.Right)
					movement = Direction.Left;
				else if (key == ConsoleKey.RightArrow && movement != Direction.Left)
					movement = Direction.Right;
			}
			return movement;
		}

		private void PrintScore()
		{
			int x = Width + 5, y = 5;
			SetCursorPosition(x, y);
			WriteWithColor("Name:\t" + Name, textColor);
			SetCursorPosition(x, y + 1);
			WriteWithColor("Score:\t" + score, textColor);
			SetCursorPosition(0, 0);
		}

		private void Init()
		{
			// Initialize default values
			random = new Random();
			score = 0;
			head = new Pixel(Width / 2, Height / 2, headColor, snakePixel);
			berry = new Pixel(random.Next(1, Width - 2), random.Next(1, Height - 2), berryColor, berryPixel);
			body = new List<Pixel>();
			currentMovement = Direction.Right;
		}

		private bool IsDied()
		{
			return (head.Left == Width || head.Left == 0 || head.Top == Height || head.Top == 0) || body.Where(p => p.Left == head.Left && p.Top == head.Top).ToList().Count > 0;
		}

		public static void WriteWithColor(string text, ConsoleColor color)
		{
			Console.ForegroundColor = color;
			Console.Write(text);
			Console.ForegroundColor = ConsoleColor.White;
		}

		public void Play()
		{
		start:
			Clear();

			Init();

			DrawBorder();
			PrintScore();

			while (true)
			{
				// End game
				if (IsDied())
					break;

				DrawBody(body);
				DrawPixel(head);
				DrawPixel(berry);

				// If the snake eats berry
				if (berry.Left == head.Left && berry.Top == head.Top)
				{
					score += 5;
					PrintScore();
					body.Add(new Pixel(berry.Left, berry.Top, bodyColor, snakePixel));
					ClearPixel(berry);
					berry = new Pixel(random.Next(1, Width - 2), random.Next(1, Height - 2), berryColor, berryPixel);
				}

				var watch = Stopwatch.StartNew();
				while (watch.ElapsedMilliseconds < Speed)
					currentMovement = ReadMovement(currentMovement);

				body.Add(new Pixel(head.Left, head.Top, bodyColor, snakePixel));

				switch (currentMovement)
				{
					case Direction.Up:
						head.Top--;
						break;
					case Direction.Down:
						head.Top++;
						break;
					case Direction.Right:
						head.Left++;
						break;
					case Direction.Left:
						head.Left--;
						break;
					default:
						break;
				}

				ClearPixel(body[0]);
				body.RemoveAt(0);
			}

			SetCursorPosition(Width / 5, Height / 2);
			WriteWithColor($"Game over, score: {score}", textColor);
			SetCursorPosition(0, Height + 1);
			WriteWithColor("Play again? Press R to restart, press other key to exit ", textColor);
			char input = ReadKey(true).KeyChar;
			if (input == 'R' || input == 'r')
				goto start;
		}
	}
}

