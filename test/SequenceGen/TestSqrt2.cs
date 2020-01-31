using System.IO;
using MathVenture.SequenceGen;
using MathVenture.SequenceGen.Generators;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace test.SequenceGen
{
	[TestClass]
	public class TestSqrt2
	{
		void TestCommon(IGenerator gen, int @base = 10)
		{
			int max = 1000;
			string file = $"sqrt2-base{@base}.txt";
			Helpers.CompareSequence(max,file,@base,gen);
		}

		[TestMethod]
		public void CofraSqrt2() { TestCommon(new CofraSqrt2()); }
	}
}
