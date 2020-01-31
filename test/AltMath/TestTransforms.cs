using System;
using MathVenture.AltMath;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace test.AltMath
{
	[TestClass]
	public class TestTransforms : TestCommon
	{
		[TestMethod]
		public void TestChebyshev1()
		{
			Func<double,double> func = x => Math.Sin(Math.PI/2.0*x);
			double min = -1, max = 1;

			TestTramsformCommon(func,
				Transforms.ChebyshevTransform1,
				Transforms.InvChebyshev1,
				min,max,8
			);

			Assert.IsTrue(true);
		}

		[TestMethod]
		public void TestChebyshev2()
		{
			Func<double,double> func = x => Math.Sin(Math.PI/2.0*x);
			double min = -1, max = 1;

			TestTramsformCommon(func,
				Transforms.ChebyshevTransform2,
				Transforms.InvChebyshev2,
				min,max,8
			);

			Assert.IsTrue(true);
		}

		void TestTramsformCommon(Func<double,double> rep,
			Func<Func<double,double>,int,double,double,double[]> forward,
			Func<double[],double,double,double,double> inverse,
			double min, double max, int depth
		) {
			var coefs = forward(rep,depth,min,max);
			//for(int i=0; i<coefs.Length; i++) {
			//	Helpers.Log(i+": "+coefs[i]);
			//}

			double tot = 0.0;
			for(double a=min; a<max; a+=0.01)
			{
				double bcos = rep(a);
				double tcos = inverse(coefs,a,min,max);
				double diff = Math.Abs(tcos-bcos);
				tot += diff;

				//Helpers.Log("a="+a+"\tb="+bcos+"\tt="+tcos+"\td="+diff);
			}
			Helpers.Log(forward.Method.Name+" tot="+tot);
		}
	}
}