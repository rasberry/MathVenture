using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SequenceGen.Generators;

namespace test
{
	[TestClass]
	public class TestSpigRubyPi
	{
		[TestMethod]
		public void TestMethod1()
		{
			int max = 1000;
			var gen = new SpigRubyPi();
			string path = Path.Join(Helpers.ProjectRoot,"test/data/pi-data.txt");
			var fil = Helpers.ReadDigitsFromFile(path);
			var fig = fil.GetEnumerator();
			while(--max > 0) {
				var o = gen.Next;
				fig.MoveNext();
				var t = fig.Current;

				Assert.AreEqual(o,t);
			}
		}
	}

}
