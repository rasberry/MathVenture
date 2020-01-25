using System;
using System.IO;
using System.Text;
using SequenceGen.Generators;

namespace SequenceGen
{
	public static class Log
	{
		public static void Message(string m)
		{
			Console.WriteLine(m);
		}

		public static void Error(string m)
		{
			Console.Error.WriteLine("E: "+m);
		}

		public static void Debug(string m)
		{
			#if DEBUG
			Console.WriteLine("D: "+m);
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