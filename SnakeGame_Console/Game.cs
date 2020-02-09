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
		public int width { get; set; }
		public int height { get; set; }
		public string name { get; set; }

		private const char snakePixel = '*';
		private const char berryPixel = '0';
		private const char verticalBorder = '|';
		private const char horizonBorder = '_';

		private bool gameover;
		private Pixel head;
		private Pixel berry;
		private List<Pixel> body;
		private Direction currentMovement;
		private int score;
		private Random random;

		public Game(int width, int height, string name)
		{
			this.width = width;
			this.height = height;
			this.name = name;
		}

		public void DrawBorder()
		{
			WriteLine(new string(horizonBorder, width));
			for (int i = 1; i < height; i++)
			{
				SetCursorPosition(0, i);
				Write(verticalBorder);
				SetCursorPosition(width, i);
				Write(verticalBorder);
			}
			SetCursorPosition(1, height - 1);
			WriteLine(new string(horizonBorder, width - 1));
		}

		public void ClearPixel(Pixel pixel)
		{
			SetCursorPosition(pixel.X, pixel.Y);
			Write(" ");
		}

		public void DrawPixel(Pixel pixel)
		{
			SetCursorPosition(pixel.X, pixel.Y);
			ForegroundColor = pixel.ScreenColor;
			Write(pixel.Symbol);
			SetCursorPosition(0, 0);
		}

		public void DrawBody(List<Pixel> body)
		{
			for (int i = 0; i < body.Count; i++)
			{
				DrawPixel(body[i]);
			}
		}

		public Direction ReadMovement(Direction movement)
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

		public void Play()
		{
			// Initialize default values
			random = new Random();
			score = 0;
			head = new Pixel(width / 2, height / 2, ConsoleColor.Green, snakePixel);
			berry = new Pixel(random.Next(1, width - 2), random.Next(1, height - 2), ConsoleColor.Cyan, berryPixel);
			body = new List<Pixel>();
			currentMovement = Direction.Right;
			gameover = false;

			DrawBorder();
			while (true)
			{
				// The snake touch wall
				gameover |= (head.X == width || head.X == 0 || head.Y == height || head.Y == 0);
				PrintScore();

				// The snake eats berry
				if (berry.X == head.X && berry.Y == head.Y)
				{
					score += 5;
					ClearPixel(berry);
					body.Add(new Pixel(berry.X, berry.Y, ConsoleColor.Red, snakePixel));
					berry = new Pixel(random.Next(1, width - 2), random.Next(1, height - 2), ConsoleColor.Cyan, berryPixel);
				}

				DrawBody(body);
				gameover |= body.Contains(head);

				// End game
				if (gameover)
					break;

				DrawPixel(head);
				DrawPixel(berry);

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

			SetCursorPosition(width / 5, height / 2);
			WriteLine($"Game over, score: {score}");
			SetCursorPosition(0, height + 1);
			ReadKey();
		}

		public void PrintScore()
		{
			int x = width + 5, y = 5;
			SetCursorPosition(x, y);
			Write("Name:\t" + name);
			SetCursorPosition(x, y + 1);
			Write("Score:\t" + score);
		}
	}
}

