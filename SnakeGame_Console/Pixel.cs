using System;

namespace SnakeGame_Console
{
	public class Pixel
	{
		public int Left { get; set; }
		public int Top { get; set; }
		public ConsoleColor ScreenColor { get; set; }
		public char Symbol { get; set; }

		public Pixel(int left, int top, ConsoleColor screenColor, char symbol)
		{
			Left = left;
			Top = top;
			ScreenColor = screenColor;
			Symbol = symbol;
		}
	}
}
