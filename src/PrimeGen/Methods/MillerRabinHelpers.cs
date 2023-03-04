using System;
using System.Collections.Generic;
using System.Numerics;

namespace MathVenture.PrimeGen.Methods
{
	public static class MillerRabinHelpers
	{
		static BigInteger TWO = 2;

		public static IEnumerable<BigInteger> RandomBases()
		{
			return RandomBases(10,BigInteger.Pow(2,31));
		}

		public static IEnumerable<BigInteger> RandomBases(int rounds,BigInteger number)
		{
			for(int r=0; r<rounds; r++) {
				yield return Helpers.Random(TWO,number - TWO);
			}
		}

		//http://miller-rabin.appspot.com/
		public static IEnumerable<BigInteger> JimSinclair2011()
		{
			yield return 0000000002L; // 1
			yield return 0000000325L; // 2
			yield return 0000009375L; // 3
			yield return 0000028178L; // 4
			yield return 0000450775L; // 5
			yield return 0009780504L; // 6
			yield return 1795265022L; // 7
		}

		// https://en.wikipedia.org/wiki/Jacobi_symbol#Implementation_in_Lua
		public static int JacobiSymbol(BigInteger n,BigInteger k)
		{
			if (k <= 0 || k % 2 != 1) {
				throw new ArgumentOutOfRangeException("invalid k");
			}
			n = n % k;
			int t = 1;
			while(n != 0) {
				while(n % 2 == 0) {
					n >>= 1; //n = n / 2;
					var r = k % 8;
					if (r == 3 || r == 5) {
						t = -t;
					}
				}
				(n,k) = (k,n); //swap n and k
				if (n % 4 == 3 && k % 4 == 3) {
					t = -t;
				}
				n = n % k;
			}
			if (k == 1) {
				return t;
			}
			else {
				return 0;
			}
		}
	}
}
