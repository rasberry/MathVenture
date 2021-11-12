using System;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace MathVenture.Factors
{
	public static class Methods
	{
		static BigInteger GetGCD(BigInteger n1, BigInteger n2)
		{
			BigInteger rem = 0;

			while (n2 > 0)
			{
				rem = BigInteger.Remainder(n1,n2);
				n1 = n2;
				n2 = rem;
			}
			return n1;
	}

		public static void MethodBasic(BigInteger number)
		{
			ulong sum = 0;
			Log.Message($"Factors of {number}");
			for(ulong i=1; i<=number; i++) {
				if (number % i == 0) {
					Log.Message($"{i}");
					sum += i;
				}
			}

			BigInteger gcd = GetGCD(sum,number);
			Log.Message($"Index = {sum/gcd}/{number/gcd}");
		}

		public static void MethodSqrt(BigInteger number)
		{
			var sq = PrimeGen.Helpers.Sqrt(number);
			BigInteger sum = 0;
			for (BigInteger i=1; i<=sq; i++)
			{
				if (number%i == 0)
				{
					// If divisors are equal, print only one
					if (number/i == i) {
						Log.Message($"{i}");
						sum += i;
					}
					// Otherwise print both
					else {
						Log.Message($"{i}");
						Log.Message($"{number/i}");
						sum += i + (number/i);
					}
				}
			}

			BigInteger gcd = GetGCD(sum,number);
			Log.Message($"Index = {sum/gcd}/{number/gcd}");

		}

		public static void MethodParallel(BigInteger number)
		{
			var sq = PrimeGen.Helpers.Sqrt(number);
			BigInteger pow2s = sq / int.MaxValue; //int floor
			int rem = (int)BigInteger.Remainder(sq,int.MaxValue);
			BigInteger sum = 0;
			object sumLock = new object();
			Log.Debug($"pow2s = {pow2s}");

			for(BigInteger p = pow2s; p >= 0; p--) {
				int max = p == 0 ? rem + 1 : int.MaxValue;
				Parallel.For(1,max,(n,state) => {
					BigInteger i = int.MaxValue * p + n;
					if (number % i == 0) {
						var div = number / i;
						if (div == i) {
							Log.Message($"{i}");
							lock(sumLock) { sum += i; }
						}
						else {
							var add = i + div;
							Log.Message($"{i}");
							Log.Message($"{div}");
							lock(sumLock) { sum += add; }
						}
					}
				});
			}

			BigInteger gcd = GetGCD(sum,number);
			Log.Message($"Index = {sum/gcd}/{number/gcd}");
		}
	}
}