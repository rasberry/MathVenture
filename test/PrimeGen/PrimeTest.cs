using System;
using System.Numerics;
using MathVenture.PrimeGen;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace test.PrimeGen
{
	[TestClass]
	public class PrimeTest
	{
		[TestMethod]
		public void PrimeSieveTest1()
		{
			var p = new GenDivision();
			RunPrimeTest(p);
		}

		[TestMethod]
		public void PrimePascalTest1()
		{
			var p = new GenPascal();
			RunPrimeTest(p);
		}

		static void RunPrimeTest(IPrimeSource prime)
		{
			BigInteger p = 0;
			for(int c=0; c<1000; c++) {
				p = prime.NextPrime(p);
				int check = FirstPrimes.List[c];
				Assert.AreEqual(p,check);
			}
		}
	}
}
