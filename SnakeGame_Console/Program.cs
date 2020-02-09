using SnakeGame_Console;
using System;

namespace SnakeGame
{
	class Program
	{
		static void WriteWithColor(string text, ConsoleColor color)
		{
			Console.ForegroundColor = color;
			Console.Write(text);
			Console.ForegroundColor = ConsoleColor.White;
		}

		static void Main(string[] args)
		{
			int width = 50, height = 15;
			WriteWithColor("What is your name? ", ConsoleColor.Red);
			string name = Console.ReadLine();
			Console.Clear();
			Game game = new Game(width, height, name);
			Console.OutputEncoding = System.Text.Encoding.UTF8;
			game.Play();
		}
	}
}
