using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting.Logging;

namespace test.AltMath
{
	[TestClass]
	public class TestConstants : TestCommon
	{
		[DataTestMethod]
		[DataRow(3.141592653589793238462643383279502884)]
		[DataRow(3.14159265358979323846)]
		[DataRow(3.1415926535897932384626433832795028841971693993751d)]
		[DataRow(Math.PI)]
		public void TestPI(double pi)
		{
			var bytes = BitConverter.GetBytes(pi);

			Assert.IsTrue(bytes != null);
			Assert.IsTrue(bytes.SequenceEqual(new byte[] {
				// https://en.wikipedia.org/wiki/Double-precision_floating-point_format
				0x18,0x2D,0x44,0x54,0xFB,0x21,0x09,0x40 //PI in IEEE 754
			}));
		}

		//can be used instead of maxima to compute the cordic values
		//[TestMethod]
		public void GenerateCordicAngles()
		{
			//atan(2^-n); n=[0,27]
			for(int n=0; n<28; n++) {
				double a = Math.Atan(Math.Pow(2.0,-n));
				Helpers.Log("angle "+n+" = "+a);
			}
			Assert.IsTrue(true);
		}

		//can be used instead of maxima to compute the cordic values
		//[TestMethod]
		public void GenerateCordicKValues()
		{
			// b(i) := sqrt(1/(1 + 2^(-2*i)));
			// bfloat(product(b(j),j,0,n)); n=[0,23]
			for(int n=0; n<24; n++) {
				decimal p = 1.0M;
				for(int j=0; j<=n; j++) {
					p *= (decimal)Math.Sqrt(1/(1+Math.Pow(2,-2*j)));
				}
				Helpers.Log("K "+n+" = "+p);
			}
			Assert.IsTrue(true);
		}
	}
}
