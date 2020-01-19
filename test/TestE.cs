using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SequenceGen.Generators;

namespace test
{
	[TestClass]
	public class TestE
	{
		void TestCommon(SequenceGen.IGenerator gen, int @base = 10)
		{
			int max = 1000;
			string path = Path.Join(Helpers.ProjectRoot,$"test/data/e-base{@base}.txt");
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
		public void CofraE() {
			TestCommon(new CofraE());
		}

		[TestMethod]
		public void CofraE2() {
			TestCommon(new CofraE2());
		}
	}
}