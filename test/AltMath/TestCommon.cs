using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace test.AltMath
{
	public class TestCommon
	{
		public TestContext TestContext { get; set; }

		[TestInitialize]
		public void TestInit()
		{
			Helpers.Log("\n" + TestContext.TestName + " " + new string('-',40));
		}
	}
}
