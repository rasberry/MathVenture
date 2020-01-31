using System;
using System.Collections.Generic;
using System.Diagnostics;
using MathVenture.AltMath;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace test.AltMath
{
	[TestClass]
	public class TestArcTrig : TestCommon, ITestItemProvider
	{
		const double TestMin = -10.0;
		const double TestMax = 10.0;
		const double XD = 1e-12;

		public IEnumerable<(TestItem,Func<TestItem,double>)> GetItems()
		{
			foreach(var item in GetTestItems()) {
				yield return (item,TestAccTrigMain);
			}
		}

		[DataTestMethod]
		[DynamicData(nameof(GetData),DynamicDataSourceType.Method)]
		public void TestArcTrigMain(TestItem item)
		{
			double d = TestAccTrigMain(item);
			Assert.AreEqual(item.Delta,d,XD);
		}

		double TestAccTrigMain(TestItem item)
		{
			var func = Unpack(item.Method);
			double d = TestCommon(func);
			return d;
		}

		IEnumerable<TestItem> GetTestItems()
		{
			yield return new TestItem { Delta = 0.0136638037737241, Name = nameof(ArcTan.AtanSO1),
				Method = Pack(ArcTan.AtanSO1) };

			yield return new TestItem { Delta = 0.0935295762042112, Name = nameof(ArcTan.AtanMac),
				Method = Pack((double a) => ArcTan.AtanMac(a)) };

			yield return new TestItem { Delta = 0.0658025452458654, Name = nameof(ArcTan.AtanMac)+"-16",
				Method = Pack((double a) => ArcTan.AtanMac(a,16)) };

			yield return new TestItem { Delta = 0.0569347297414578, Name = nameof(ArcTan.AtanMac)+"-32",
				Method = Pack((double a) => ArcTan.AtanMac(a,32)) };

			yield return new TestItem { Delta = 0.00226640192840601, Name = nameof(ArcTan.AtanActon),
				Method = Pack((double a) => ArcTan.AtanActon(a)) };

			yield return new TestItem { Delta = 3.45821481589903E-08, Name = nameof(ArcTan.AtanActon)+"-16",
				Method = Pack((double a) => ArcTan.AtanActon(a,16)) };

			yield return new TestItem { Delta = 5.04457586814056E-14, Name = nameof(ArcTan.AtanActon)+"-32",
				Method = Pack((double a) => ArcTan.AtanActon(a,32)) };

			yield return new TestItem { Delta = 5.64583343687364E-08, Name = nameof(ArcTan.AtanAms),
				Method = Pack((double a) => ArcTan.AtanAms(a)) };

			yield return new TestItem { Delta = 122.793568849762, Name = nameof(ArcTan.Atanfdlibm),
				Method = Pack(ArcTan.Atanfdlibm) };
		}

		public static IEnumerable<object[]> GetData()
		{
			var inst = new TestArcTrig();
			foreach(var item in inst.GetTestItems()) {
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

		static void TestAll(Func<double,double> func)
		{
			string n = func.Method.Name;
			TestCommon(func,TestMin,TestMax,n);
			TestCommon((double y,double x) => ArcTan.Atan2_1(func,y,x),TestMin,TestMax,n+"-atan21");
			TestCommon((double y,double x) => ArcTan.Atan2_2(func,y,x),TestMin,TestMax,n+"-atan22");
		}

		static double TestCommon(Func<double,double> rep, double min = TestMin, double max = TestMax, string name = null)
		{
			double tot = 0.0;
			if (name == null) { name = rep.Method.Name; }
			for(double a=min; a<max; a+=0.1)
			{
				double vrep = rep(a);
				double vchk = Math.Atan(a);
				double diff = Math.Abs(vrep - vchk);
				tot += diff;

				//string txt = string.Format("{0}\ta={1:F6}\tv={2:F6}\tc={3:F6}\td={4:F6}",
				//	name,a,vrep,vchk,diff);
				//Helpers.Log(txt);
			}
			//Helpers.Log(name+"\ttot="+tot);
			return tot;

			//var sw = Stopwatch.StartNew();
			//for(double tt=min; tt<max; tt+=0.00001)
			//{
			//	double vrep = rep(tt);
			//}
			//Helpers.Log(name+"\ttime test="+sw.ElapsedMilliseconds);
		}

		static double TestCommon(Func<double,double,double> rep, double min = TestMin, double max = TestMax,string name = null)
		{
			double tot = 0.0;
			if (name == null) { name = rep.Method.Name; }
			for(double y=min; y<max; y+=0.1)
			for(double x=min; x<max; x+=0.1)
			{
				double vrep = rep(y,x);
				double vchk = Math.Atan2(y,x);
				double diff = Math.Abs(vrep - vchk);
				tot += diff;

				//string txt = string.Format("{0}\ta={1:E}\tv={2:E}\tc={3:E}\td={4:E}",
				//	name,a,vrep,vchk,diff);
				//Helpers.Log(txt);
			}
			return tot;
			//Helpers.Log(name+"\ttot="+tot);
		}
	}
}
