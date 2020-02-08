using System;
using System.Diagnostics;
using System.IO;
using System.Numerics;
using System.Text;

namespace MathVenture.PrimeGen
{
	public class Start : IMain
	{
		public bool ParseArgs(string[] args)
		{
			return Options.ParseArgs(args);
		}

		public void Usage(StringBuilder sb)
		{
			Options.Usage(sb);
		}

		public void Main()
		{
			switch(Options.Action)
			{
			case ActionType.Gen:     DoGen(); break;
			case ActionType.Bits:    DoBits(); break;
			case ActionType.BitsImg: DoBitsImg(); break;
			case ActionType.Test:    DoTest(); break;
			}
		}

		static void DoGen()
		{
			IPrimeSource gen = null;
			switch(Options.WhichGen)
			{
			case GenType.Division:
				gen = new GenDivision();
				break;
			case GenType.Pascal:
				gen = new GenPascal();
				break;
			case GenType.Fermat:
				gen = new GenFermat();
				break;
			}

			TextWriter tw = null;
			try {
				if (Options.OutputFile != null) {
					var fs = File.Open(Options.OutputFile,FileMode.Create,FileAccess.Write,FileShare.Read);
					tw = new StreamWriter(fs);
				} else {
					tw = Console.Out;
				}

				BigInteger p = Options.Start - 1;
				Log.Debug("s = "+p);
				while(p <= Options.End) {
					p = gen.NextPrime(p);
					tw.WriteLine(p);
				}
			}
			finally {
				if (Options.OutputFile != null && tw != null) {
					tw.Dispose();
				}
			}
		}

		static void DoBits()
		{
			var bits = new BitsEratosthenes();
			bits.FillPrimes(0);
			//TODO finish
		}

		static void DoBitsImg()
		{
			//TODO finish
		}

		static void DoTest()
		{
			//var gen = new GenDivision();
			//// var gen = new GenFermat() { Power = 3 };
			//BigInteger n = 1;
			//for(int i=0; i<100; i++) {
			//	n = gen.NextPrime(n);
			//	Console.WriteLine(n);
			//}
			//return;
			
			//#if false
			for(int i=2; i<100; i++) {
				var gControl = new GenDivision();
				var gFermat = new GenFermat() { Power = i };

				BigInteger next = 1;
				int liars = 10;
				Console.Write($"{i}");
				while(liars >= 0) {
					var con = gControl.NextPrime(next);
					var fer = gFermat.NextPrime(next);
					while (con < fer) {
						con = gControl.NextPrime(con);
					}
					if (con != fer) {
						Console.Write($"\t{fer}");
						liars--;
					}
					next = fer;
				}
				Console.WriteLine();
			}
			//#endif
		}
	}
}
