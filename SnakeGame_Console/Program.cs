using SnakeGame_Console;
using System;

namespace SnakeGame
{
	class Program
	{
		static int max = 6;

		static void Main(string[] args)
		{
			Console.OutputEncoding = System.Text.Encoding.UTF8;
			int width = 50, height = 15;
			Game.WriteWithColor("What is your name? ", ConsoleColor.Red);
			string name = Console.ReadLine();
			Console.WriteLine();
			int speed = LevelOption();
			Game game = new Game(width, height, name, speed);
			game.Play();
		}

		static int LevelOption()
		{
			int level = 0;
			int[] speeds = { 500, 450, 400, 350, 300, 250, 200 };
			while (true)
			{
				Console.SetCursorPosition(0, 1);
				Game.WriteWithColor("Choose level (Press Right/Left arrow key to Increase/Decrease level): ", ConsoleColor.Red);
				ConsoleKey input = Console.ReadKey(true).Key;
				switch (input)
				{
					case ConsoleKey.RightArrow:
						if (level < max)
							level++;
						break;
					case ConsoleKey.LeftArrow:
						if (level > 0)
							level--;
						break;
					default:
						return speeds[level];
				}
				string str = "[" + "".PadRight(level, '*') + "".PadRight(max - level, ' ') + "]\n";
				Game.WriteWithColor(str, ConsoleColor.White);
			}
			return speeds[level];
		}
	}
}
