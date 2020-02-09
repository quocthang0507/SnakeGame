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

		private const char pixel = '■';

		private bool gameover;
		private Pixel head;
		private Pixel berry;
		private List<Pixel> body;
		private Direction currentMovement;
		private int score;
		private Random random;

		public Game(int width, int height)
		{
			this.width = width;
			this.height = height;
		}

		public void DrawBorder()
		{
			for (int i = 0; i < width; i++)
			{
				SetCursorPosition(i, 0);
				Write("■");

				SetCursorPosition(i, height - 1);
				Write("■");
			}

			for (int i = 0; i < height; i++)
			{
				SetCursorPosition(0, i);
				Write("■");

				SetCursorPosition(width - 1, i);
				Write("■");
			}
		}

		public void DrawPixel(Pixel pixel)
		{
			SetCursorPosition(pixel.X, pixel.Y);
			ForegroundColor = pixel.ScreenColor;
			Write(Game.pixel);
			SetCursorPosition(0, 0);
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
			score = 5;
			head = new Pixel(width / 2, height / 2, ConsoleColor.Red);
			berry = new Pixel(random.Next(1, width - 2), random.Next(1, height - 2), ConsoleColor.Cyan);
			body = new List<Pixel>();
			currentMovement = Direction.Right;
			gameover = false;

			while (true)
			{
				Clear();
				gameover |= (head.X == width - 1 || head.X == 0 || head.Y == height - 1 || head.Y == 0);
				DrawBorder();
				// The snake eats berry
				if (berry.X == head.X && berry.Y == head.Y)
				{
					score++;
					berry = new Pixel(random.Next(1, width - 2), random.Next(1, height - 2), ConsoleColor.Cyan);
				}
				// The snake touched it's tail
				for (int i = 0; i < body.Count; i++)
				{
					DrawPixel(body[i]);
					gameover |= (body[i].X == head.X && body[i].Y == head.Y);
				}
				// End game
				if (gameover)
					break;

				DrawPixel(head);
				DrawPixel(berry);

				var watch = Stopwatch.StartNew();
				while (watch.ElapsedMilliseconds <= 500)
					currentMovement = ReadMovement(currentMovement);

				body.Add(new Pixel(head.X, head.Y, ConsoleColor.Green));

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

				if (body.Count > score)
					body.RemoveAt(0);
			}

			SetCursorPosition(width / 5, height / 2);
			WriteLine($"Game over, score: {score - 5}");
			SetCursorPosition(0, height + 1);
			ReadKey();
		}
	}
}

