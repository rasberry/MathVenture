using System;
using System.Collections.Generic;
using System.IO;
using MathVenture.SequenceGen;
using MathVenture.SequenceGen.Generators;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace test.SequenceGen
{
	[TestClass]
	public class TestSqrt2 : ITestItemProvider
	{
		void TestCommon(IGenerator gen, int @base = 10)
		{
			int max = 1000;
			string file = $"sqrt2-base{@base}.txt";
			Helpers.CompareSequence(max,file,@base,gen);
		}

		[TestMethod]
		public void CofraSqrt2() {
			TestCommon(new CofraSqrt2());
		}

		public double SpeedTest(TestItem testItem)
		{
			return Helpers.SpeedTest(testItem);
		}

		public IEnumerable<(TestItem, Func<TestItem, double>)> GetItems()
		{
			yield return (new TestItem {
				Name = nameof(CofraSqrt2),
				Method = Helpers.Pack(new CofraSqrt2())
			},null);
		}
	}
}
