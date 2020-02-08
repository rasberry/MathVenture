using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace test
{
	public class TestItem
	{
		//method to test
		public Delegate Method;
		//accuracy metric
		public double Delta;
		//test identifier
		public string Name;
	}

	public interface ITestItemProvider
	{
		//returns sum of inaccuracy over all iterations
		double SpeedTest(TestItem testItem);

		//return collection of:
		// TestItem - test item
		// Func<TestItem,double> - function to run the test and return maximum inaccuracy
		IEnumerable<(TestItem,Func<TestItem,double>)> GetItems();
	}
}