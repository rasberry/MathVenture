using System;
using System.Numerics;

namespace MathVenture.PrimeGen
{
	public enum ActionType {
		None = 0,
		Gen = 1,
		Bits = 2,
		BitsImg = 3,
		Test = 4
	}

	public enum GenType {
		None = 0,
		Division = 1,
		Pascal = 2,
		Fermat = 3,
		PascalBit = 4
	}

	public enum BitsType {
		None = 0
	}

	public enum BitsImgType {
		None = 0
	}

	public interface IPrimeSource
	{
		BigInteger NextPrime(BigInteger number);
	}

	public interface IPrimeTest
	{
		bool IsPrime(BigInteger number);
	}
}