using System;
using System.Diagnostics;

namespace MathVenture.PrimeGen
{
	public static class Log
	{
		public static void Message(string m)
		{
			Console.WriteLine(m);
		}

		public static void Debug(string m)
		{
			Trace.WriteLine("D: "+m);
		}

		public static void Error(string m)
		{
			Console.Error.WriteLine("E: "+m);
		}

		public static void Info(string m)
		{
			if (ShowInfo) {
				Console.WriteLine("I: "+m);
			}
		}

		public static void Warning(string m)
		{
			Console.Error.WriteLine("W: "+m);
		}

		public static bool ShowInfo { get; set; }
	}
}