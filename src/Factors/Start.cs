using System;
using System.Text;

namespace MathVenture.Factors
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
			var sn = Options.StartNumber;
			switch(Options.WhichMethod){
				case PickMethod.Basic:    Methods.MethodBasic(sn); break;
				case PickMethod.Sqrt:     Methods.MethodSqrt(sn); break;
				case PickMethod.Parallel: Methods.MethodParallel(sn); break;
			}
		}

		void FindFactorsBasic(ulong number)
		{
			ulong sum = 0;
			Log.Message($"Factors of {number}");
			for(ulong i=1; i<=number; i++) {
				if (number % i == 0) {
					Log.Message($"{i}");
					sum += i;
				}
			}

			Log.Message($"Index = {sum}/{number}");
		}
	}
}
