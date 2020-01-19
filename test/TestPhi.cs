using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SequenceGen.Generators;

namespace test
{
	[TestClass]
	public class TestPhi
	{
		void TestCommon(SequenceGen.IGenerator gen, int @base = 10)
		{
			int max = 1000;
			string path = Path.Join(Helpers.ProjectRoot,$"test/data/phi-base{@base}.txt");
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
		public void CofraPhi() {
			TestCommon(new CofraPhi());
		}
	}
}