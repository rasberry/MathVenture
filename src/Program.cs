using System;
using SequenceGen.Generators;

namespace SequenceGen
{
	class Program
	{
		static void Main(string[] args)
		{
			//TODO rename spig to Cofra (short for continued fraction)
			//var pi = new ConstPi();
			//var pi = new SpigSqrt2();
			//var pi = new SpigPi();
			//var pi = new SpigE();
			//var pi = new SpigPhi();
			//var pi = new SpigPi2();
			var pi = new GibPi();
			//var pi = new ConstPi();
			for(int i=0; i<1000; i++) {
				Console.Write(pi.Next);
			}
			Console.WriteLine();
		}
	}
}
