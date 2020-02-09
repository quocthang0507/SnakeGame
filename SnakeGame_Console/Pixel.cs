using System;
using System.Collections.Generic;
using System.Text;

namespace SnakeGame_Console
{
	public class Pixel
	{
		public Pixel(int x, int y, ConsoleColor screenColor, char symbol)
		{
			X = x;
			Y = y;
			ScreenColor = screenColor;
			Symbol = symbol;
		}

		public int X { get; set; }
		public int Y { get; set; }
		public ConsoleColor ScreenColor { get; set; }
		public char Symbol { get; set; }
	}
}
