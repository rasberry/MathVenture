using System;
using SequenceGen.Generators;

namespace SequenceGen
{
	class Program
	{
		static void Main(string[] args)
		{
			var pi = new ConstPi();
			for(int i=0; i<1000; i++) {
				Console.Write(pi.Next);
			}
			Console.WriteLine();
		}
	}
}
