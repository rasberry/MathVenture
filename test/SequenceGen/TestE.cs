using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MathVenture.SequenceGen.Generators;
using MathVenture.SequenceGen;
using System.Collections.Generic;

namespace test.SequenceGen
{
	[TestClass]
	public class TestE : ITestItemProvider
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

		public double SpeedTest(TestItem testItem)
		{
			return Helpers.SpeedTest(testItem);
		}

		public IEnumerable<(TestItem, Func<TestItem, double>)> GetItems()
		{
			yield return (new TestItem {
				Name = nameof(CofraE),
				Method = Helpers.Pack(new CofraE())
			},null);
			yield return (new TestItem {
				Name = nameof(CofraE2),
				Method = Helpers.Pack(new CofraE2())
			},null);
		}
	}
}