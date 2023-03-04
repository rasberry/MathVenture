using System;
using System.Collections.Generic;

namespace MathVenture.LFSR;

/// <summary>
/// Implements a Linear Feedback Shift Register
/// </summary>
/// <seealso href="https://en.wikipedia.org/wiki/Linear-feedback_shift_register#Galois_LFSRs" />
public static class LinearFeedbackShiftRegister
{
	/// <summary>
	/// Generate a non-ordered sequence of numbers between 1 and 2^(bit count)-1
	///  using a random primitive polynomial for taps
	/// </summary>
	/// <param name="bitCount">bit depth [1-63]</param>
	/// <param name="initialState">initial state of the LFSR (defaults to 1)</param>
	/// <param name="repeat">if true, repeats the sequence forever (defaults to false)</param>
	/// <returns>A sequence of numbers</returns>
	public static IEnumerable<ulong> SequenceBits(int bitCount, ulong initialState = 1ul, bool repeat = false)
	{
		ulong taps = GetRandomTaps(bitCount);
		return SequenceBitsWithTaps(taps,initialState,repeat);
	}

	/// <summary>
	/// Generate a non-ordered sequence of numbers between 1 and 2^(bit count)-1
	///  using the given primitive polynomial for taps
	/// </summary>
	/// <param name="taps">the primitive polynomial</param>
	/// <param name="initialState">initial state of the LFSR (defaults to 1)</param>
	/// <param name="repeat">if true, repeats the sequence forever (defaults to false)</param>
	/// <returns>A sequence of numbers</returns>
	public static IEnumerable<ulong> SequenceBitsWithTaps(ulong taps, ulong initialState = 1ul, bool repeat = false)
	{
		do {
			ulong lfsr = initialState;

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


	/// <summary>
	/// Generate a non-ordered sequence of numbers between 1 and max
	///  using a random primitive polynomial for taps
	/// </summary>
	/// <param name="max">the maximum number to include in the sequence</param>
	/// <param name="initialState">initial state of the LFSR (defaults to 1)</param>
	/// <param name="repeat">if true, repeats the sequence forever (defaults to false)</param>
	/// <returns>A sequence of numbers</returns>
	public static IEnumerable<ulong> SequenceLength(ulong max, ulong initialState = 1ul, bool repeat = false)
	{
		//sequence by total length (rounded up to nearest bit length)
		int bits = FindBitsForMax(max);
		ulong taps = GetRandomTaps(bits);
		return SequenceLengthWithTaps(max,taps,initialState,repeat);
	}

	/// <summary>
	/// Generate a non-ordered sequence of numbers between 1 and max
	///  using the given primitive polynomial for taps
	/// </summary>
	/// <param name="max">the maximum number to include in the sequence</param>
	/// <param name="taps">the primitive polynomial</param>
	/// <param name="initialState">initial state of the LFSR (defaults to 1)</param>
	/// <param name="repeat">if true, repeats the sequence forever (defaults to false)</param>
	/// <returns>A sequence of numbers</returns>
	public static IEnumerable<ulong> SequenceLengthWithTaps(ulong max, ulong taps, ulong initialState = 1ul, bool repeat = false)
	{
		do {
			foreach(ulong x in SequenceBitsWithTaps(taps,initialState,false)) {
				//skip numbers above the length since bitLength rounds up
				if (x <= max) {
					yield return x;
				}
			}
		} while(repeat);
	}

	/// <summary>
	/// Determine the fewest bits needed to count up to max
	/// </summary>
	/// <param name="max">Given number</param>
	/// <returns>Number of bits</returns>
	public static int FindBitsForMax(ulong max)
	{
		//the max length of an LFSR sequence is 2^n-1 (not 2^n). a '0' is not produced
		int bits = (int)Math.Ceiling(Math.Log(max + 1,2.0));
		return bits;
	}

	/// <summary>
	/// Get a random primitive polynomial for use as taps
	/// </summary>
	/// <param name="bitCount">bit depth [1-63]</param>
	/// <returns>bit representation of the polynomial</returns>
	public static ulong GetRandomTaps(int bitCount)
	{
		return PrimitivePoly.GetRandomPoly(bitCount);
	}

	/// <summary>
	/// Get a primitive polynomial for use as taps - these taps come from the
	///  xilinx documentation
	/// </summary>
	/// <seealso href="http://www.xilinx.com/support/documentation/application_notes/xapp052.pdf" />
	/// <param name="bitCount">bit depth [1-63]</param>
	/// <returns>bit representation of the polynomial</returns>
	public static ulong GetXilinxTaps(int bitCount)
	{
		switch(bitCount) {
			case 03: return                   0x6; // 03 02
			case 04: return                   0xC; // 04 03
			case 05: return                  0x14; // 05 03
			case 06: return                  0x30; // 06 05
			case 07: return                  0x60; // 07 06
			case 08: return                  0xB8; // 08 06 05 04
			case 09: return                 0x110; // 09 05
			case 10: return                 0x240; // 10 07
			case 11: return                 0x500; // 11 09
			case 12: return                 0x829; // 12 06 04 01
			case 13: return                0x100D; // 13 04 03 01
			case 14: return                0x2015; // 14 05 03 01
			case 15: return                0x6000; // 15 14
			case 16: return                0xD008; // 16 15 13 4
			case 17: return              0x1_2000; // 17 14
			case 18: return              0x2_0400; // 18 11
			case 19: return              0x4_0023; // 19 06 02 01
			case 20: return              0x9_0000; // 20 17
			case 21: return             0x14_0000; // 21 19
			case 22: return             0x30_0000; // 22 21
			case 23: return             0x42_0000; // 23 18
			case 24: return             0xE1_0000; // 24 23 22 17
			case 25: return            0x120_0000; // 25 22
			case 26: return            0x200_0023; // 26 06 02 01
			case 27: return            0x400_0013; // 27 05 02 01
			case 28: return            0x900_0000; // 28 25
			case 29: return           0x1400_0000; // 29 27
			case 30: return           0x2000_0029; // 30 06 04 01
			case 31: return           0x4800_0000; // 31 28
			case 32: return           0x8020_0003; // 32 22 02 01
			case 33: return         0x1_0008_0000; // 33 20
			case 34: return         0x2_0400_0003; // 34 27 02 01
			case 35: return         0x5_0000_0000; // 35 33
			case 36: return         0x8_0100_0000; // 36 25
			case 37: return        0x10_0000_001F; // 37 05 04 03 02 01
			case 38: return        0x20_0000_0031; // 38 06 05 01
			case 39: return        0x44_0000_0000; // 39 35
			case 40: return        0xA0_0014_0000; // 40 38 21 19
			case 41: return       0x120_0000_0000; // 41 38
			case 42: return       0x300_000C_0000; // 42 41 20 19
			case 43: return       0x630_0000_0000; // 43 42 38 37
			case 44: return       0xC00_0003_0000; // 44 43 18 17
			case 45: return      0x1B00_0000_0000; // 45 44 42 41
			case 46: return      0x3000_0300_0000; // 46 45 26 25
			case 47: return      0x4200_0000_0000; // 47 42
			case 48: return      0xC000_0018_0000; // 48 47 21 20
			case 49: return    0x1_0080_0000_0000; // 49 40
			case 50: return    0x3_0000_00C0_0000; // 50 49 24 23
			case 51: return    0x6_000C_0000_0000; // 51 50 36 35
			case 52: return    0x9_0000_0000_0000; // 52 49
			case 53: return   0x18_0030_0000_0000; // 53 52 38 37
			case 54: return   0x30_0000_0003_0000; // 54 53 18 17
			case 55: return   0x40_0000_4000_0000; // 55 31
			case 56: return   0xC0_0006_0000_0000; // 56 55 35 34
			case 57: return  0x102_0000_0000_0000; // 57 50
			case 58: return  0x200_0040_0000_0000; // 58 39
			case 59: return  0x600_0030_0000_0000; // 59 58 38 37
			case 60: return  0xC00_0000_0000_0000; // 60 59
			case 61: return 0x1800_3000_0000_0000; // 61 60 46 45
			case 62: return 0x3000_0000_0000_0030; // 62 61 06 05
			case 63: return 0x6000_0000_0000_0000; // 63 62
		}
		throw new ArgumentOutOfRangeException(nameof(bitCount));
	}
}
