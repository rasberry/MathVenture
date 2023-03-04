using System;
using System.Collections.Generic;
using System.Text;
using Generator = MathVenture.LFSR.LinearFeedbackShiftRegister;

namespace MathVenture.LFSR;

public class Start : IMain
{
	public bool ParseArgs(string[] args)
	{
		return Options.ParseArgs(args);
	}

	public void Usage(StringBuilder sb)
	{
		Options.Usage(sb);
	}

	public void Main()
	{
		int bitCount;
		if (Options.Max != null) {
			bitCount = Generator.FindBitsForMax(Options.Max.Value);
		}
		else {
			bitCount = Options.BitCount.GetValueOrDefault(8);
		}
		ulong initialState = Options.InitialState.GetValueOrDefault(1ul);

		ulong taps;
		if (Options.Taps == null) {
			if (Options.UseXilinx) {
				taps = Generator.GetXilinxTaps(bitCount);
			}
			else {
				taps = Generator.GetRandomTaps(bitCount);
			}
		}
		else {
			taps = Options.Taps.Value;
		}

		Log.Info($"Using primitive polynomial {taps} [0b{ToBinary(taps)}] Exponents: {IndexBits(taps)}");

		IEnumerable<ulong> seq;
		if (Options.Max == null) {
			seq = Generator.SequenceBitsWithTaps(taps,initialState);
		}
		else {
			seq = Generator.SequenceLengthWithTaps(Options.Max.Value,taps,initialState);
		}

		foreach(var n in seq) {
			Log.Message(n.ToString());
		}
	}

	static string ToBinary(ulong u, bool pad = false)
	{
		var sNum = Convert.ToString((long)u,2);

		string sPad = "";
		if (pad) {
			sPad = new string('0',64 - sNum.Length);
		}
		return sPad + Convert.ToString((long)u,2);
	}

	static string IndexBits(ulong u, bool doOnes = true)
	{
		var sb = new StringBuilder();

		var barr = new System.Collections.BitArray(BitConverter.GetBytes(u));
		for(int b = 0; b < barr.Length; b++) {
			if (barr[b] == doOnes) {
				if (sb.Length > 0) {
					sb.Insert(0,' ');
				}
				// add one to make it ones based (controversial?)
				sb.Insert(0,b + 1);
			}
		}
		return sb.ToString();
	}

	public void Main1()
	{
		/*
		var rnd = new System.Random();
		for(int i=0; i<2; i++)
		{
			ulong a = (ulong)rnd.NextInt64(0,100);
			ulong b = (ulong)rnd.NextInt64(0,100);
			ulong c = (ulong)rnd.NextInt64(0, (long)System.Math.Min(a,b) - 1);

			var pp = PrimitivePoly.PolyModMultiply(a,b,c,ulong.MaxValue);
			var np = (System.UInt128)a * b % c;

			Log.Debug($"pp = {pp} np = {np}");
			Log.Debug($"pp = {TestLFSR.ToBinary(pp,true)}");
			Log.Debug($"np = {TestLFSR.ToBinary((ulong)np,true)}");
		}
		return;
		*/

		//for(int i=2; i<=9; i++) {
		//	TestLFSR.TryAll(i);
		//}

		//int bits = 63;

		//PrimitivePolyOne.FindAll((ulong)bits);
		//Log.Debug(new string('=',80));

		/*
		var n2b = new necklace2bitpol(9);

		for(ulong a = 0; a < 512; a++) {
			bool w = PrimitivePoly.TryPrim(a,9);
			//Log.Debug($"a = {a} w = {w}");
			if (w) {
				var sb = new StringBuilder();
				ulong p = n2b.poly(a) >> 1;
				Bits.bitpol_print_coeff(sb,"",p);
				Log.Debug($"p = {p} coeff = {sb}");
				//Log.Debug(sb.ToString());
			}
		}
		*/

		/*
		for(int i=0; i < 10; i++) {
			var t = PrimitivePoly.GetRandomTaps(bits);

			var sb = new StringBuilder();
			Bits.bitpol_print_coeff(sb,"",t);

			Log.Debug($"taps = {t} coeff = {sb}");
		}
		*/

		//for(int i = 0; i < 50; i++) {
		//	var taps = PrimitivePoly.GetRandomPoly(9);
		//	var seq = TestLFSR.SequenceBits(1,taps);
		//
		//	ulong count = 0;
		//	foreach(var n in seq) {
		//		count++;
		//	}
		//	Log.Debug($"taps={taps} B:{TestLFSR.ToBinary(taps)} count={count}");
		//}
	}
}