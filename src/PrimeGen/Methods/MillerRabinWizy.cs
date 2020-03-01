using System;
using System.Collections.Generic;
using System.Numerics;

// https://github.com/wizykowski/miller-rabin/blob/master/sprp64_sf.h
namespace MathVenture.PrimeGen.Methods
{
	public class MillerRabinWizy : IPrimeSource, IPrimeTest
	{
		public BigInteger NextPrime(BigInteger number)
		{
			return Helpers.NextPrime(number,this);
		}

		static BigInteger THREE = 3;
		static BigInteger TWO = 2;
		static BigInteger ONE = 1;
		static BigInteger ZERO = 0;

		public bool IsPrime(BigInteger n)
		{
			if (BaseSequence == null) {
				BaseSequence = MillerRabinHelpers.JimSinclair2011();
			}

			//make sure n in odd and n > 3
			if (n < TWO) { return false; }
			if (n == TWO || n == THREE) { return true; }
			if (n % TWO == ZERO) { return false; }
			var m = n - ONE;

			// u will be even, as n is required to be odd
			var u = m >> 1;
			long t = 1; //long should work for n up to 2^(2^63) ~ (2.7 pentillion digits)

			//while even
			while(u % TWO == ZERO) {
				t++;
				u >>= 1;
			}

			foreach(var b in BaseSequence)
			{
				var a = b;
				if (a >= n) { a %= n; }
				if (a == 0) { continue; }
				var x = BigInteger.ModPow(a,u,n);
				if (x == ONE || x == m) { continue; }

				long i;
				for(i=1; i<t; i++) {
					x = x * x % n;
					if (x == ONE) { return false; }
					if (x == m) { break; }
				}

				// if we didn't break, the number is composite
				if (i == t) { return false; }
			}
			return true;
		}

		//bases used to test for primality - defaults to Random
		public IEnumerable<BigInteger> BaseSequence { get; set; }
	}
}