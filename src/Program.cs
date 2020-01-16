using System;
using SequenceGen.Generators;

namespace SequenceGen
{
	class Program
	{
		static void Main(string[] args)
		{
			//var pi = new ConstPi();
			//var pi = new SpigRubySqrt2();
			var pi = new SpigPi();
			for(int i=0; i<1000; i++) {
				Console.Write(pi.Next);
			}
			Console.WriteLine();
		}
	}
}
