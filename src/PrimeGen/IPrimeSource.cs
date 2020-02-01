using System;
using System.Numerics;

namespace MathVenture.PrimeGen
{
	public interface IPrimeSource
	{
		BigInteger NextPrime(BigInteger number);
	}
}
