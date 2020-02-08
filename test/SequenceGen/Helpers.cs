using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using MathVenture.SequenceGen;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace test.SequenceGen
{
	public static class Helpers
	{
		public static IEnumerable<Digit> ReadDigitsFromFile(string file, int @base = 10)
		{
			var fs = File.Open(file,FileMode.Open,FileAccess.Read,FileShare.Read);
			using (fs) {
				while(true) {
					int b = fs.ReadByte();
					if (b < 0) { break; }
					int num = 0;
					if (b >= 0x30 && b <= 0x39) { //0-9
						num = b - 0x30;
					}
					else if (b >= 0x41 && b <= 0x5A) { //A-Z
						num = b - 0x37;
					}
					else if (b >= 0x61 && b <= 0x7A) { //a-z
						num = b - 0x57;
					}
					else {
						continue; //skip unrecognized values
					}
					if (num < 0 || num >= @base) {
						throw new ArgumentOutOfRangeException($"numeric value '{num}' out of range");
					}
					Digit d = new Digit(num,@base);
					yield return d;
				}
			}
		}

		public static void CompareSequence(int max, string dataFile, int @base, IGenerator gen)
		{
			string path = Path.Join(Aids.ProjectRoot,$"test/data/{dataFile}");
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

		public static double SpeedTest(TestItem testItem)
		{
			int max = 1000;
			var func = (Func<Digit>)testItem.Method;
			for(int i=0; i<max; i++) {
				var _ = func.Invoke();
			}
			return 0.0;
		}

		public static Delegate Pack(IGenerator f) {
			return (Func<Digit>)(() => f.Next);
		}
	}
}