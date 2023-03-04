using System;
using System.IO;
using System.Text;

namespace MathVenture
{
	public static class Log
	{
		public static void Message(string m)
		{
			Console.WriteLine(m);
		}

		public static void Error(string m)
		{
			Console.ForegroundColor = ConsoleColor.Red;
			Console.Error.WriteLine("E: "+m);
			Console.ResetColor();
		}

		public static void Warning(string m)
		{
			Console.ForegroundColor = ConsoleColor.DarkYellow;
			Console.Error.WriteLine("W: "+m);
			Console.ResetColor();
		}

		public static void Info(string m)
		{
			if (ShowInfo) {
				Console.ForegroundColor = ConsoleColor.Blue;
				Console.WriteLine("I: "+m);
				Console.ResetColor();
			}
		}

		public static bool ShowInfo { get; set; }

		public static void Debug(string m)
		{
			#if DEBUG
			Console.ForegroundColor = ConsoleColor.Gray;
			Console.WriteLine("D: "+m);
			Console.ResetColor();
			#endif
		}

		public static TextWriter StdOut { get {
			return Console.Out;
		}}

		public static TextWriter StdErr { get {
			return Console.Error;
		}}
	}
}