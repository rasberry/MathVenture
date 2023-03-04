using System;
using MathVenture.LFSR;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.IO;

namespace test.LFSR
{
	[TestClass]
	public class TestLFSR
	{
		[TestMethod]
		public void TestLengthRandom()
		{
			var seq = LinearFeedbackShiftRegister.SequenceBits(8);
			Assert.AreEqual(255,seq.Count());
		}

		[TestMethod]
		public void TestSequenceMax()
		{
			var seq = LinearFeedbackShiftRegister.SequenceLength(250);
			Assert.AreEqual(250,seq.Count());
		}

		[TestMethod]
		public void TestSequenceXilinx()
		{
			var taps = LinearFeedbackShiftRegister.GetXilinxTaps(8);
			var seq = LinearFeedbackShiftRegister.SequenceBitsWithTaps(taps);
			var neq = ReadNumersFromFile("lfsr-x8.txt");

			var good = Enumerable.SequenceEqual(neq,seq);
			Assert.IsTrue(good);
		}

		[DataTestMethod]
		[DataRow(0)]
		[DataRow(64)]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void TestLimitsXilinx(int bits)
		{
			LinearFeedbackShiftRegister.GetXilinxTaps(bits);
		}

		[DataTestMethod]
		[DataRow(0)]
		[DataRow(64)]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void TestLimitsRandomTaps(int bits)
		{
			LinearFeedbackShiftRegister.GetRandomTaps(bits);
		}

		static IEnumerable<ulong> ReadNumersFromFile(string dataFile)
		{
			string path = Path.Join(Aids.ProjectRoot,$"test/data/{dataFile}");
			var fs = File.Open(path,FileMode.Open,FileAccess.Read,FileShare.Write);
			var sr = new StreamReader(fs);
			using (fs) using(sr) {
				while(!sr.EndOfStream) {
					string s = sr.ReadLine();
					if (String.IsNullOrWhiteSpace(s)) { continue; }
					ulong num = UInt64.Parse(s,System.Globalization.NumberStyles.Any,null);
					yield return num;
				}
			}
		}
	}
}