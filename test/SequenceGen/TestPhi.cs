using System;
using System.IO;
using MathVenture.SequenceGen;
using MathVenture.SequenceGen.Generators;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace test.SequenceGen
{
	[TestClass]
	public class TestPhi
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
	}
}