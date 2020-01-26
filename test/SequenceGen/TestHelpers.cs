using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MathVenture.SequenceGen;

namespace test.SequenceGen
{
	[TestClass]
	public class TestHelpers
	{
		[TestMethod]
		public void TestConvertBase()
		{
			Assert.IsTrue(TestConvert(32,2,new int[] { 1,0,0,0,0,0 }));
			Assert.IsTrue(TestConvert(31,2,new int[] { 1,1,1,1,1 }));
			Assert.IsTrue(TestConvert(32,3,new int[] { 1,0,1,2 }));
			Assert.IsTrue(TestConvert(31,3,new int[] { 1,0,1,1 }));
			Assert.IsTrue(TestConvert(32,4,new int[] { 2,0,0 }));
			Assert.IsTrue(TestConvert(31,4,new int[] { 1,3,3 }));
			Assert.IsTrue(TestConvert(111111,5,new int[] { 1,2,0,2,3,4,2,1 }));
			Assert.IsTrue(TestConvert(111111,6,new int[] { 2,2,1,4,2,2,3 }));
			Assert.IsTrue(TestConvert(111111,7,new int[] { 6,4,1,6,4,0 }));
			Assert.IsTrue(TestConvert(111111,8,new int[] { 3,3,1,0,0,7 }));
			Assert.IsTrue(TestConvert(111111,11,new int[] { 7,6,5,3,0 }));
			Assert.IsTrue(TestConvert(111111,16,new int[] { 1,11,2,0,7 }));
			Assert.IsTrue(TestConvert(111111,21,new int[] { 11,20,20,0 }));
			Assert.IsTrue(TestConvert(111111,256,new int[] { 1,178,7 }));
		}

		static bool TestConvert(int num, int @base, IEnumerable<int> result)
		{
			var conv = MathVenture.SequenceGen.Helpers.ConvertBase(num,@base);
			return Matches(conv,result);
		}

		static bool Matches(IEnumerable<Digit> digits, IEnumerable<int> test)
		{
			var checkList = test.GetEnumerator();
			foreach(var d in digits) {
				if (!checkList.MoveNext()) {
					return false;
				}
				if ((int)d != checkList.Current) {
					return false;
				}
			}
			return true;
		}
	}
}
