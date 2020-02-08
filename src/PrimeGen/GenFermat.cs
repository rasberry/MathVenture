using System;
using System.Numerics;

namespace MathVenture.PrimeGen
{
	public class GenFermat : IPrimeSource
	{
		static BigInteger TWO = 2;
		static BigInteger ONE = 1;

		public BigInteger NextPrime(BigInteger number)
		{
			if (number < TWO) { return TWO; }
			BigInteger next = number + (number.IsEven ? ONE : TWO);

			while(true) {
				//Fermat's little theorem
				//slower: a^p mod p == a
				//faster: a^(p-1) mod p == 1
				var test = BigInteger.ModPow(Power,next-ONE,next);
				if (test == ONE) { return next; }
				next += TWO;
			}
		}

		public BigInteger Power { get; set; } = 2;
	}
}