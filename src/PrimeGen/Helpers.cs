using System;
using System.Numerics;

namespace MathVenture.PrimeGen
{
	public static class Helpers
	{
		public static bool EndsWithIC(this string subj,string check)
		{
			if (subj == null) { return false; }
			return subj.EndsWith(check,StringComparison.OrdinalIgnoreCase);
		}

		public static BigInteger Log2(this BigInteger number)
		{
			BigInteger count = 0;
			while(number > 1) {
				number >>= 1;
				count++;
			}
			return count;
		}

		//BigInteger doesn't have a built-in sqrt function
		public static BigInteger Sqrt(this BigInteger n)
		{
			if (n == 0 || n == 1) { return n; }
			if (n <= 0) {
				throw new ArithmeticException("NaN");
			}

			int bitLength = (int)Math.Ceiling(BigInteger.Log(n, 2));
			BigInteger root = BigInteger.One << (bitLength / 2);

			BigInteger last0 = 0;
			BigInteger last1 = 1;
			//some times the number oscilates between sqrt+0 and sqrt+1
			while(last1 != root)
			{
				last1 = last0;
				last0 = root;
				root = (root + (n / root)) >> 1;
			}

			return BigInteger.Min(root,last0);
		}

		//Generic NextPrime funcion given a primality test
		public static BigInteger NextPrime(this BigInteger number,IPrimeTest gen)
		{
			if (number < 2) {
				return 2;
			}

			var next = number + (number.IsEven ? 1 : 2);

			while(!gen.IsPrime(next)) {
				next += 2;
			}
			return next;
		}
	}

	public struct SizeL
	{
		public SizeL(long w,long h) {
			Width = w;
			Height = h;
		}
		public long Width;
		public long Height;
	}
}