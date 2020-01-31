using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MathVenture.SequenceGen.Generators;
using MathVenture.SequenceGen;

namespace test.SequenceGen
{
	[TestClass]
	public class TestE
	{
		void TestCommon(IGenerator gen, int @base = 10)
		{
			int max = 1000;
			string file = $"e-base{@base}.txt";
			Helpers.CompareSequence(max,file,@base,gen);
		}

		[TestMethod]
		public void CofraE() {
			TestCommon(new CofraE());
		}

		[TestMethod]
		public void CofraE2() {
			TestCommon(new CofraE2());
		}
	}
}