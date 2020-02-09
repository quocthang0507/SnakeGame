using SnakeGame_Console;
using System;

namespace SnakeGame
{
	class Program
	{
		static void Main(string[] args)
		{
			int width = 30, height = 20;
			Game game = new Game(width, height);
			game.Play();
		}
	}
}
