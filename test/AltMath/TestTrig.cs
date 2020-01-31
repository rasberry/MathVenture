using System;
using System.Collections.Generic;
using System.Diagnostics;
using MathVenture.AltMath;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace test.AltMath
{
	[TestClass]
	public class TestTrig : TestCommon, ITestItemProvider
	{
		const double Math2PI = 2.0 * Math.PI;
		const double MathPIo2 = Math.PI / 2.0;
		const double MathPIo4 = Math.PI / 4.0;
		const double TestMin = -Math2PI;
		const double TestMax = Math2PI;
		const double XD = 1e-12;

		public IEnumerable<(TestItem,Func<TestItem,double>)> GetItems()
		{
			foreach(var item in SinGetTestItems()) {
				yield return (item,TestTrigSinAcc);
			}
			foreach(var item in CosGetTestItems()) {
				yield return (item,TestTrigCosAcc);
			}
		}

		[DataTestMethod]
		[DynamicData(nameof(SinGetData),DynamicDataSourceType.Method)]
		public void TestTrigSinMain(TestItem item)
		{
			double d = TestTrigSinAcc(item);
			Assert.AreEqual(item.Delta,d,XD);
		}

		double TestTrigSinAcc(TestItem item)
		{
			var func = Unpack(item.Method);
			double d = TestCommon(func,Math.Sin);
			return d;
		}

		IEnumerable<TestItem> SinGetTestItems()
		{
			yield return new TestItem { Delta = 1.09810064102525E-13, Name = nameof(Sine.SinSO),
				Method = Pack(Sine.SinSO) };

			yield return new TestItem { Delta = 78.6051247042265, Name = nameof(SpectrumRom.Sin),
				Method = Pack(SpectrumRom.Sin) };

			yield return new TestItem { Delta = 0.0635031546303115, Name = nameof(Sine.SinXupremZero),
				Method = Pack(Sine.SinXupremZero) };

			yield return new TestItem { Delta = 3.06749508765944E-08, Name = nameof(NBSApplied.Sin),
				Method = Pack(NBSApplied.Sin) };

			yield return new TestItem { Delta = 4.73341042769437E-05, Name = nameof(Sine.Sin5),
				Method = Pack((double a) => Sine.Sin5(a)) };

			yield return new TestItem { Delta = 1.28626401479628E-13, Name = nameof(Sine.Sin5)+"-16",
				Method = Pack((double a) => Sine.Sin5(a,16)) };

			yield return new TestItem { Delta = 1.93058194713749E-13, Name = nameof(Sine.Sin5)+"-32",
				Method = Pack((double a) => Sine.Sin5(a,32)) };

			yield return new TestItem { Delta = 0.301510668694616, Name = nameof(Cordic.Sin)+"-Sin",
				Method = Pack((double a) => Cordic.Sin(a)) };

			yield return new TestItem { Delta = 4.72747762933103E-06, Name = nameof(Cordic.Sin)+"-Sin-24",
				Method = Pack((double a) => Cordic.Sin(a,24)) };

			yield return new TestItem { Delta = 0.00400130951601312, Name = nameof(Sine.SinTaylor),
				Method = Pack(Sine.SinTaylor) };

			yield return new TestItem { Delta = 2.261290882078, Name = nameof(Sine.SinFdlibm),
				Method = Pack(Sine.SinFdlibm) };

			yield return new TestItem { Delta = 14.075154510891, Name = nameof(Sine.Sin6),
				Method = Pack((double a) => Sine.Sin6(a)) };

			yield return new TestItem { Delta = 6.75028061584193, Name = nameof(Sine.Sin6)+"-16",
				Method = Pack((double a) => Sine.Sin6(a,16)) };

			yield return new TestItem { Delta = 3.30773444231687, Name = nameof(Sine.Sin6)+"-32",
				Method = Pack((double a) => Sine.Sin6(a,32)) };

		}

		public static IEnumerable<object[]> SinGetData()
		{
			var inst = new TestTrig();
			foreach(var item in inst.SinGetTestItems()) {
				yield return new object[] { item };
			}
		}

		[DataTestMethod]
		[DynamicData(nameof(CosGetData),DynamicDataSourceType.Method)]
		public void TestTrigCosMain(TestItem item)
		{
			double d = TestTrigCosAcc(item);
			Assert.AreEqual(item.Delta,d,XD);
		}

		public double TestTrigCosAcc(TestItem item)
		{
			var func = Unpack(item.Method);
			double d = TestCommon(func,Math.Cos);
			return d;
		}

		IEnumerable<TestItem> CosGetTestItems()
		{
			yield return new TestItem { Delta = 4.28993995275212E-08, Name = nameof(NBSApplied.Cos),
				Method = Pack(NBSApplied.Cos) };

			yield return new TestItem { Delta = 0.30451787266993, Name = nameof(Cordic.Cos)+"-Cos",
				Method = Pack((double a) => Cordic.Cos(a)) };

			yield return new TestItem { Delta = 4.68074049085002E-06, Name = nameof(Cordic.Cos)+"-Cos-24",
				Method = Pack((double a) => Cordic.Cos(a,24)) };
		}

		public static IEnumerable<object[]> CosGetData()
		{
			var inst = new TestTrig();
			foreach(var item in inst.CosGetTestItems()) {
				yield return new object[] { item };
			}
		}

		public double SpeedTest(TestItem testItem)
		{
			var func = Unpack(testItem.Method);
			double tot = 0.0;
			for(double tt=TestMin; tt<TestMax; tt+=1e-05)
			{
				double vrep = func(tt);
				tot += vrep;
			}
			return tot;
		}

		static Func<double,double> Unpack(Delegate d) {
			return (Func<double,double>)d;
		}
		static Delegate Pack(Func<double,double> f) {
			return f;
		}
		static Delegate Pack(Func<float,float> f) {
			return new Func<double,double>((double a) => (double)f((float)a));
		}

		static double TestCommon(Func<double,double> rep, Func<double,double> check, double min = TestMin, double max = TestMax, string name = null)
		{
			double tot = 0.0;
			if (name == null) { name = rep.Method.Name; }
			for(double a=min; a<max; a+=0.1)
			{
				double vrep = rep(a);
				double vchk = check(a);
				double diff = Math.Abs(vrep - vchk);
				tot += diff;

				//string txt = string.Format("{0}\ta={1:E}\tv={2:E}\tc={3:E}\td={4:E}",
				//	name,a,vrep,vchk,diff);
				//Helpers.Log(txt);
			}
			return tot;
			//Helpers.Log(name+"\ttot="+tot);

			//var sw = Stopwatch.StartNew();
			//for(double tt=min; tt<max; tt+=0.00001)
			//{
			//	double vrep = rep(tt);
			//}
			//Helpers.Log(name+"\ttime test="+sw.ElapsedMilliseconds);
		}


		//TODO === these are pretty much wrong
		// they were supposed to take the full cirlce range
		// and cut it down to a specified interval
		// i think these actually need to be transforms instead
		// taking a function as input
		// because they need to reduce the input angle
		// then modify the output to the appropriate quatrant
		#if false
		[TestMethod]
		public void TestSinRange1()
		{
			TestSinRange((double a) => {
				return a % Math2PI;
			});
		}

		[TestMethod]
		public void TestSinRange2()
		{
			TestSinRange((double ang) => {
				double r = ang % Math2PI;
				double a = ang % Math.PI;
				return r < -Math.PI || r > Math.PI ? -a : a;
			});
		}

		[TestMethod]
		public void TestSinRange3()
		{
			TestSinRange((double ang) => {
				double a = ang % MathPIo2;
				double m = Math.Floor(Math.Abs(ang/MathPIo2));
				if (ang < -MathPIo2) { return  a - m * MathPIo2; }
				if (ang > MathPIo2)  { return  a + m * MathPIo2; }
				return a;
			});
		}

		[TestMethod]
		public void TestSinRange4()
		{
			TestSinRange((double ang) => {
				double a = ang % MathPIo4;
				double m = Math.Floor(Math.Abs(ang/MathPIo4));
				if (ang < -MathPIo4) { return a - m * MathPIo4; }
				if (ang > MathPIo4)  { return a + m * MathPIo4; }
				return a;
			});
		}

		static void TestSinRange(Func<double,double> trans)
		{
			double min = -10.0;
			double max = 10.0;

			//Helpers.Log(trans.Method.Name + " " + new string('=',10));
			double tot = 0.0;
			for(double a=min; a<max; a+=0.1) {
				double ang = trans(a);
				double n = Math.Sin(a);
				double c = Math.Sin(ang);
				double d = Math.Abs(c-n);
				tot += d;
				//Helpers.Log(string.Format("a={0:F6}\tn={1:F6}\tc={2:F6}\td={3:F6}",
				//	a,n,c,d
				//));
			}
			//Helpers.Log("tot="+tot);
			Assert.AreEqual(tot,1e-10,1e-10);
		}
		#endif
	}
}
