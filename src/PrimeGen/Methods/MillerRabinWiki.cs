using System;
using System.Collections.Generic;
using System.Numerics;

// https://en.wikipedia.org/wiki/Miller%E2%80%93Rabin_primality_test
namespace MathVenture.PrimeGen.Methods
{
	public class MillerRabinWiki : IPrimeSource, IPrimeTest
	{
		static BigInteger ZERO = 0;
		static BigInteger ONE = 1;
		static BigInteger TWO = 2;
		static BigInteger THREE = 3;

		public bool IsPrime(BigInteger number)
		{
			if (number < TWO) { return false; }
			if (number == TWO) { return true; }
			if (number == THREE) { return true; }
			if (number % TWO == ZERO) { return false; }

			if (BaseSequence == null) {
				BaseSequence = MillerRabinHelpers.RandomBases(10,number);
			}
			var minone = number - ONE; //should be even;
			var d = minone;
			BigInteger r = 0;
			while(d % 2 == 0) {
				r++;
				d >>= 1; //divide by 2
			}
			
			foreach(var a in BaseSequence)
			{
				var x = a * d % number;
				if (x == ONE || x == minone) {
					continue;
				}
				for(BigInteger i=r-ONE; i>ZERO; i--) {
					x = BigInteger.ModPow(x,2,number);
					if (x == minone) {
						continue;
					}
				}
				return false;
			}
			return true;
		}

		//bases used to test for primality - defaults to Random
		public IEnumerable<BigInteger> BaseSequence { get; set; }

		public BigInteger NextPrime(BigInteger number)
		{
			return Helpers.NextPrime(number,this);
		}
	}
}