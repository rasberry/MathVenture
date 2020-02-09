using System;
using System.Numerics;

namespace MathVenture.PrimeGen.Methods
{
	public class FermatLittle : IPrimeSource, IPrimeTest
	{
		static BigInteger TWO = 2;
		static BigInteger ONE = 1;
		static BigInteger ZERO = 0;

		public BigInteger NextPrime(BigInteger number)
		{
			if (number < TWO) { return TWO; }
			BigInteger next = number + (number.IsEven ? ONE : TWO);

			while(true) {
				if (IsPrime(next)) { return next; }
				next += TWO;
			}
		}

		public bool IsPrime(BigInteger number)
		{
			if (number < TWO) { return false; }
			if (number % TWO == ZERO) { return false; }

			//Fermat's little theorem
			//slower: a^p mod p == a
			//faster: a^(p-1) mod p == 1
			var test = BigInteger.ModPow(Power,number-ONE,number);
			return test == ONE;
		}

		public BigInteger Power { get; set; } = 2;
	}
}