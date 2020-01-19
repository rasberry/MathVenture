using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SequenceGen.Generators;

namespace test
{
	[TestClass]
	public class TestPi
	{
		void TestCommon(SequenceGen.IGenerator gen, int @base = 10)
		{
			int max = 1000;
			string path = Path.Join(Helpers.ProjectRoot,$"test/data/pi-base{@base}.txt");
			var fil = Helpers.ReadDigitsFromFile(path,@base);
			var fig = fil.GetEnumerator();
			while(--max > 0) {
				var o = gen.Next;
				fig.MoveNext();
				var t = fig.Current;
				// Console.WriteLine($"o={o} t={t} o==t {o.Equals(t)}");
				Assert.AreEqual(o,t);
			}
		}

		[TestMethod]
		public void GibPi() {
			TestCommon(new GibPi());
		}

		[TestMethod]
		public void GibPi_16() {
			TestCommon(new GibPi { Base = 16 },16);
		}

		[TestMethod]
		public void GibPi2() {
			TestCommon(new GibPi2());
		}

		[TestMethod]
		public void CofraPi() {
			TestCommon(new CofraPi());
		}

		[TestMethod]
		public void CofraPi_16() {
			TestCommon(new CofraPi { Base = 16 },16);
		}
	}
}
