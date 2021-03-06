using System;
using System.Collections.Generic;
using System.IO;
using MathVenture.SequenceGen;
using MathVenture.SequenceGen.Generators;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace test.SequenceGen
{
	[TestClass]
	public class TestPhi : ITestItemProvider
	{
		void TestCommon(IGenerator gen, int @base = 10)
		{
			int max = 1000;
			string file = $"phi-base{@base}.txt";
			Helpers.CompareSequence(max,file,@base,gen);
		}

		[TestMethod]
		public void CofraPhi() {
			TestCommon(new CofraPhi());
		}

		public double SpeedTest(TestItem testItem)
		{
			return Helpers.SpeedTest(testItem);
		}

		public IEnumerable<(TestItem, Func<TestItem, double>)> GetItems()
		{
			yield return (new TestItem {
				Name = nameof(CofraPhi),
				Method = Helpers.Pack(new CofraPhi())
			},null);
		}
	}
}