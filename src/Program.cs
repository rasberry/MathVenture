using System;
using SequenceGen.Generators;

namespace SequenceGen
{
	class Program
	{
		static void Main(string[] args)
		{
			//var pi = new CofraSqrt2();
			//var pi = new CofraPi();
			//var pi = new CofraE();
			//var pi = new CofraPhi();
			//var pi = new CofraPi2();
			var pi = new GibPi();
			for(int i=0; i<1000; i++) {
				Console.Write(pi.Next);
			}
			Console.WriteLine();
		}
	}
}
