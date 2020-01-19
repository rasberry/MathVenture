using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SequenceGen.Generators;

namespace test
{
	[TestClass]
	public class TestSqrt2
	{
		void TestCommon(SequenceGen.IGenerator gen)
		{
			int max = 1000;
			string path = Path.Join(Helpers.ProjectRoot,"test/data/sqrt2-base10.txt");
			var fil = Helpers.ReadDigitsFromFile(path);
			var fig = fil.GetEnumerator();
			while(--max > 0) {
				var o = gen.Next;
				fig.MoveNext();
				var t = fig.Current;

				Assert.AreEqual(o,t);
			}
		}

		[TestMethod]
		public void CofraSqrt2() { TestCommon(new CofraSqrt2()); }
	}
}
