using System;
using System.Numerics;
using MathVenture.PrimeGen;
using MathVenture.PrimeGen.Methods;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace test.PrimeGen
{
	[TestClass]
	public class PrimeTest
	{
		static void RunPrimeTest(IPrimeSource prime)
		{
			BigInteger p = 0;
			for(int c=0; c<1000; c++) {
				p = prime.NextPrime(p);
				int check = FirstPrimes.List[c];
				Assert.AreEqual(p,check);
			}
		}

		[TestMethod]
		public void TestTrialDivision1()
		{
			var p = new TrialDivision();
			RunPrimeTest(p);
		}

		[TestMethod]
		public void TestPascalRow1()
		{
			var p = new PascalRow();
			RunPrimeTest(p);
		}

		[TestMethod]
		public void TestMillerRabinWikiRandom()
		{
			var p = new MillerRabinWiki();
			p.BaseSequence = MillerRabinHelpers.RandomBases();
			RunPrimeTest(p);
		}

		[TestMethod]
		public void TestMillerRabinWikiSinclair()
		{
			var p = new MillerRabinWiki();
			p.BaseSequence = MillerRabinHelpers.JimSinclair2011();
			RunPrimeTest(p);
		}

		[TestMethod]
		public void TestMillerRabinWizyRandom()
		{
			var p = new MillerRabinWizy();
			p.BaseSequence = MillerRabinHelpers.RandomBases();
			RunPrimeTest(p);
		}

		[TestMethod]
		public void TestMillerRabinWizySinclair()
		{
			var p = new MillerRabinWizy();
			p.BaseSequence = MillerRabinHelpers.JimSinclair2011();
			RunPrimeTest(p);
		}
	}
}
