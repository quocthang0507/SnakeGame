using System;
using System.Collections.Generic;
using System.Diagnostics;
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

		private const char snakePixel = '*';
		private const char berryPixel = '0';
		private const char verticalBorder = '|';
		private const char horizonBorder = '_';
		private const char blank = ' ';

		private bool gameover;
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

		public Game(int width, int height, string name, int speed)
		{
			Width = width;
			Height = height;
			Name = name;
			Speed = speed;
		}

		private void DrawBorder()
		{
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
			SetCursorPosition(pixel.X, pixel.Y);
			Write(blank);
		}

		private void DrawPixel(Pixel pixel)
		{
			SetCursorPosition(pixel.X, pixel.Y);
			ForegroundColor = pixel.ScreenColor;
			Write(pixel.Symbol);
			SetCursorPosition(0, 0);
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
			Write("Name:\t" + Name);
			SetCursorPosition(x, y + 1);
			Write("Score:\t" + score);
			SetCursorPosition(0, 0);
		}

		public void Play()
		{
			// Initialize default values
			random = new Random();
			score = 0;
			head = new Pixel(Width / 2, Height / 2, ConsoleColor.Green, snakePixel);
			berry = new Pixel(random.Next(1, Width - 2), random.Next(1, Height - 2), ConsoleColor.Cyan, berryPixel);
			body = new List<Pixel>();
			currentMovement = Direction.Right;
			gameover = false;

			DrawBorder();
			PrintScore();

			while (true)
			{
				// The snake touched wall
				gameover |= (head.X == Width || head.X == 0 || head.Y == Height || head.Y == 0);
				// The snake was touched by itself
				gameover |= body.Contains(head);

				// End game
				if (gameover)
					break;

				DrawBody(body);
				DrawPixel(head);
				DrawPixel(berry);

				// The snake ate berry
				if (berry.X == head.X && berry.Y == head.Y)
				{
					score += 5;
					ClearPixel(berry);
					body.Add(new Pixel(berry.X, berry.Y, ConsoleColor.Red, snakePixel));
					berry = new Pixel(random.Next(1, Width - 2), random.Next(1, Height - 2), ConsoleColor.Cyan, berryPixel);
					PrintScore();
				}

				var watch = Stopwatch.StartNew();
				while (watch.ElapsedMilliseconds <= 500)
					currentMovement = ReadMovement(currentMovement);

				body.Add(new Pixel(head.X, head.Y, ConsoleColor.Red, snakePixel));

				switch (currentMovement)
				{
					case Direction.Up:
						head.Y--;
						break;
					case Direction.Down:
						head.Y++;
						break;
					case Direction.Right:
						head.X++;
						break;
					case Direction.Left:
						head.X--;
						break;
					default:
						break;
				}

				ClearPixel(body[0]);
				body.RemoveAt(0);
			}

			SetCursorPosition(Width / 5, Height / 2);
			WriteLine($"Game over, score: {score}");
			SetCursorPosition(0, Height + 1);
			ReadKey();
		}
	}
}

