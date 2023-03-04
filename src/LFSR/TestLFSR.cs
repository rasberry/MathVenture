using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MathVenture.LFSR;

public static class TestLFSR
{
	//sequence by number of bits in the register
	public static IEnumerable<ulong> SequenceBits(ulong initialState, ulong taps, bool repeat = false)
	{
		do {
			ulong lfsr = initialState;
			//ulong taps = GetTapConstant(bitLength);

			do {
				ulong lsb = lfsr & 1;
				lfsr >>= 1;
				if (lsb != 0) {
					lfsr ^= taps;
				}
				yield return lfsr;
			} while(lfsr != initialState);
		} while(repeat);
	}

	public static void TryAll(int bitLength)
	{
		if (bitLength < 1 || bitLength > 64) {
			throw new System.ArgumentOutOfRangeException(nameof(bitLength));
		}

		ulong max = 0;
		if (bitLength == 64) {
			max = ulong.MaxValue;
		}
		else {
			ulong all = ulong.MaxValue;
			ulong mask = (1ul << bitLength) - 1ul;
			max = all & mask;
		}

		for(ulong b = 1; b < max; b++) {
			ulong count = 0;
			var seq = SequenceBits(1,b);
			foreach(ulong s in seq) {
				count++;
			}
			if (count == max) {
				Log.Message($"B:{bitLength} b:{b} S:{b.ToBinary(true)} I:{IndexBits(b)}");
			}
		}
	}

	public static string ToBinary(this ulong u, bool pad = false)
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

		var barr = new BitArray(BitConverter.GetBytes(u));
		for(int b = 0; b < barr.Length; b++) {
			if (barr[b] == doOnes) {
				if (sb.Length > 0) {
					sb.Insert(0,' ');
				}
				sb.Insert(0,b);
			}
		}
		return sb.ToString();
	}
}