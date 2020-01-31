using System;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace test.AltMath
{
	[TestClass]
	public class TestHelpers : TestCommon
	{
		[TestMethod]
		public void TestHI()
		{
			int hi = MathVenture.AltMath.Helpers.HI(Math.PI);
			Assert.AreEqual(0x54442D18,hi);
		}

		[TestMethod]
		public void TestLO()
		{
			int lo = MathVenture.AltMath.Helpers.LO(Math.PI);
			Assert.AreEqual(0x400921FB,lo);
		}
	}
}