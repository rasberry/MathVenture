using System;
using System.IO;
using MathVenture.SequenceGen;
using MathVenture.SequenceGen.Generators;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace test.SequenceGen
{
	[TestClass]
	public class TestPi
	{
		void TestCommon(IGenerator gen, int @base = 10)
		{
			int max = 1000;
			string file = $"pi-base{@base}.txt";
			Helpers.CompareSequence(max,file,@base,gen);
		}

		[TestMethod]
		public void GibPi() {
			TestCommon(new GibPi());
		}

		[TestMethod]
		public void GibPi_16() {
			TestCommon(new GibPi { Base = 16 },16);
		}

		[TestMethod]
		public void GibPi2() {
			TestCommon(new GibPi2());
		}

		[TestMethod]
		public void CofraPi() {
			TestCommon(new CofraPi());
		}

		[TestMethod]
		public void CofraPi_16() {
			TestCommon(new CofraPi { Base = 16 },16);
		}

		[TestMethod]
		public void CofraPi_2() {
			TestCommon(new CofraPi { Base = 2 },2);
		}

		[TestMethod]
		public void BppPi() {
			TestCommon(new BppPi(),16);
		}
	}
}
