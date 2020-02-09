using System;
using System.Numerics;

namespace MathVenture.PrimeGen.Methods
{
	public class PascalRow : IPrimeSource, IPrimeTest
	{
		static BigInteger TWO = 2;
		static BigInteger ONE = 1;
		static BigInteger ZERO = 0;

		public bool IsPrime(BigInteger test)
		{
			BigInteger mid = test / TWO;
			BigInteger last = test;
			for(BigInteger p = TWO; p < mid; p++)
			{
				//https://en.wikipedia.org/wiki/Pascal's_triangle#Calculating_a_row_or_diagonal_by_itself
				BigInteger num = last * (test + ONE - p) / p;
				//wrong -- BigInteger num = last * ((test + 1) / p - 1);
				//slow  -- BigInteger num = (test + 1 - p) * last / p;
				//hangs -- BigInteger num = last / p * (test + 1 - p);
				last = num;

				BigInteger rem;
				BigInteger.DivRem(num,test,out rem);

				if (rem != ZERO) {
					return false;
				}
			}
			return true;
		}

		public BigInteger NextPrime(BigInteger number)
		{
			return Helpers.NextPrime(number,this);
		}
	}
}
