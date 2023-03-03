using System;
using System.Numerics;

namespace MathVenture.LFSR;

public static class PrimitivePoly
{
	public static bool TryPrim(ulong taps, ulong bits)
	{
		if (bits == 0) { return false; }

		ulong mers = ulong.MaxValue;
		if (bits < Extra.BITS_PER_LONG) {
			mers = (1ul << (int)bits) - 1;
		}

		return 1 == Extra.gcd(taps,mers);
	}

	const int BitsPerLong = 64;
	static Random Rnd = new Random();
	
	public static ulong GetRandomTaps(int bitCount)
	{
		if (bitCount < 1 || bitCount > BitsPerLong) {
			throw new ArgumentOutOfRangeException(nameof(bitCount));
		}

		ulong random;
		do {
			random = (ulong)Rnd.NextInt64();
		}
		while(!IsValid(random, bitCount));

		return GetPolyFromSeed(random,bitCount);
	}

	static bool IsValid(ulong seed, int bits)
	{
		if (bits == 0) { return false; }

		ulong mask = ulong.MaxValue;
		if (bits < BitsPerLong) {
			mask = (1ul << bits) - 1;
		}

		return GreatestCommonDivisor(seed,mask) == 1;
	}

	static T GreatestCommonDivisor<T>(T a, T b) where T : IBinaryInteger<T>
	{
		while (!T.IsZero(b)) {
			T r = a % b;
			a = b;
			b = r;
		}
		return a;
	}

	static ulong GetPolyFromSeed(ulong seed, int bits)
	{
		ulong mod = Extra.lowbit_primpoly[bits];
		ulong genc = 2ul;
		ulong mask = 1ul << (bits - 1);

		ulong s = ModPower(genc,seed,mod,mask);
		ulong x = 2ul;
		ulong m = 1ul;

		for(ulong i = 0; i < (ulong)bits; i++) {
			ulong t = x ^ s;
			m = ModMultiply(m,t,mod,mask);
			s = ModSquare(s,mod,mask);
		}
		return m ^ mod;
	}

	// Return (A ** e) mod C
	static ulong ModPower(ulong a, ulong e, ulong c, ulong h)
	{
		ulong s = a;
		ulong b = SetOnlyHighBit(e);
		while(b > 1ul) {
			b >>= 1;
			s = ModSquare(s,c,h); // s *= s;
			if ((e & b) != 0) {
				s = ModMultiply(s,a,c,h); // s *= a;
			}
		}
		return s;
	}

	// Return number where only the highest bit in x is set.
	// Return 0 if no bit is set.
	static ulong SetOnlyHighBit(ulong x)
	{
		x |= x >> 1;
		x |= x >> 2;
		x |= x >> 4;
		x |= x >> 8;
		x |= x >> 16;
		x |= x >> 32;
		return  x ^ (x >> 1);
	}

	// Return A * A mod C
	static ulong ModSquare(ulong a, ulong c, ulong h)
	{
		return ModMultiply(a, a, c, h);
	}

	// Return  (A * B) mod C
	// Must have deg(A) < deg(C) and deg(B) < deg(C)
	static ulong ModMultiply(ulong a, ulong b, ulong c, ulong h)
	{
		ulong t = 0;
		do {
			{ if ((b & 1ul) != 0ul) { t ^= a; } b >>= 1; ulong s = a & h; a <<= 1; if (s != 0ul) { a^=c; }}
			{ if ((b & 1ul) != 0ul) { t ^= a; } b >>= 1; ulong s = a & h; a <<= 1; if (s != 0ul) { a^=c; }}
			{ if ((b & 1ul) != 0ul) { t ^= a; } b >>= 1; ulong s = a & h; a <<= 1; if (s != 0ul) { a^=c; }}
			{ if ((b & 1ul) != 0ul) { t ^= a; } b >>= 1; ulong s = a & h; a <<= 1; if (s != 0ul) { a^=c; }}
		}
		while(b != 0ul);
		return t;
	}
}

#if false
public class BitNecklace
{
	const int BitsPerLong = 64;
	ulong data;   // necklace
	ulong period;   // period of the necklace
	ulong bitPower;  // bit representing n: n2==2**(n-1)
	ulong perPower;  // bit representing j: j2==2**(j-1)
	int bitCount;   // number of bits in words
	ulong mask;  // mask of n ones
	ulong factor;  // for fast factor lookup

	public BitNecklace(int depth)
	{
		Init(depth);
	}

	void Init(int depth)
	{
		if (depth == 0) { depth = 1; } // avoid hang
		if (depth >= BitsPerLong) { depth = BitsPerLong; }
		bitCount = depth;

		bitPower = 1ul << depth - 1;
		mask = ulong.MaxValue >> BitsPerLong - depth;
		factor = TinyFactors[depth] >> 1;
		factor |= bitPower;  // needed for depth == BitsPerLong
		First();
	}

	public void First()
	{
		data = 0;
		period = 1;
		perPower = 1;
	}

	public ulong Data() { return data; }
	public ulong Period() { return period; }

	public ulong Next()
	{
		if (data == mask) {
			First();
			return 0ul;
		}

		do {
			period = (ulong)(bitCount - 1);
			ulong pb = 1ul << (int)period;
			while((data & pb) != 0ul) {
				period--;
				pb >>= 1;
			}
			perPower = 1ul << (int)period;
			period++;
			data |= perPower;
			data = BitCopyPeriodic(data, period, bitCount);
		}
		while((factor & perPower) == 0);
		return period;
	}

	public ulong IsLyndonWord()
	{
		return perPower & bitPower;
	}

	// Return word that consists of the lowest p bits of a repeated
	// in the lowest ldn bits (upper bits are zero).
	// E.g.: if p==3, ldn=7 and a=*****xyz (8-bit), the return 0zxyzxyz.
	// Must have p>0 and ldn>0.
	public static ulong BitCopyPeriodic(ulong a, ulong p, int count)
	{
		ulong end = (ulong)count;
		a &= ulong.MaxValue >> BitsPerLong - (int)p;
		for (ulong s = p; s < end; s <<= 1)  { a |= a << (int)s; }
		a &= ulong.MaxValue >> BitsPerLong - count;
		return a;
	}

	static ulong[] TinyFactors = new ulong[] {
		0x0ul,                 // x = 0:            ( bits: ........)
		0x2ul,                 // x = 1:   1        ( bits: ......1.)
		0x6ul,                 // x = 2:   1 2      ( bits: .....11.)
		0xaul,                 // x = 3:   1 3      ( bits: ....1.1.)
		0x16ul,                // x = 4:   1 2 4    ( bits: ...1.11.)
		0x22ul,                // x = 5:   1 5      ( bits: ..1...1.)
		0x4eul,                // x = 6:   1 2 3 6  ( bits: .1..111.)
		0x82ul,                // x = 7:   1 7      ( bits: 1.....1.)
		0x116ul,               // x = 8:   1 2 4 8
		0x20aul,               // x = 9:   1 3 9
		0x426ul,               // x = 10:  1 2 5 10
		0x802ul,               // x = 11:  1 11
		0x105eul,              // x = 12:  1 2 3 4 6 12
		0x2002ul,              // x = 13:  1 13
		0x4086ul,              // x = 14:  1 2 7 14
		0x802aul,              // x = 15:  1 3 5 15
		0x10116ul,             // x = 16:  1 2 4 8 16
		0x20002ul,             // x = 17:  1 17
		0x4024eul,             // x = 18:  1 2 3 6 9 18
		0x80002ul,             // x = 19:  1 19
		0x100436ul,            // x = 20:  1 2 4 5 10 20
		0x20008aul,            // x = 21:  1 3 7 21
		0x400806ul,            // x = 22:  1 2 11 22
		0x800002ul,            // x = 23:  1 23
		0x100115eul,           // x = 24:  1 2 3 4 6 8 12 24
		0x2000022ul,           // x = 25:  1 5 25
		0x4002006ul,           // x = 26:  1 2 13 26
		0x800020aul,           // x = 27:  1 3 9 27
		0x10004096ul,          // x = 28:  1 2 4 7 14 28
		0x20000002ul,          // x = 29:  1 29
		0x4000846eul,          // x = 30:  1 2 3 5 6 10 15 30
		0x80000002ul,          // x = 31:  1 31
		0x100010116ul,         // x = 32:  1 2 4 8 16 32
		0x20000080aul,         // x = 33:  1 3 11 33
		0x400020006ul,         // x = 34:  1 2 17 34
		0x8000000a2ul,         // x = 35:  1 5 7 35
		0x100004125eul,        // x = 36:  1 2 3 4 6 9 12 18 36
		0x2000000002ul,        // x = 37:  1 37
		0x4000080006ul,        // x = 38:  1 2 19 38
		0x800000200aul,        // x = 39:  1 3 13 39
		0x10000100536ul,       // x = 40:  1 2 4 5 8 10 20 40
		0x20000000002ul,       // x = 41:  1 41
		0x400002040ceul,       // x = 42:  1 2 3 6 7 14 21 42
		0x80000000002ul,       // x = 43:  1 43
		0x100000400816ul,      // x = 44:  1 2 4 11 22 44
		0x20000000822aul,      // x = 45:  1 3 5 9 15 45
		0x400000800006ul,      // x = 46:  1 2 23 46
		0x800000000002ul,      // x = 47:  1 47
		0x100000101115eul,     // x = 48:  1 2 3 4 6 8 12 16 24 48
		0x2000000000082ul,     // x = 49:  1 7 49
		0x4000002000426ul,     // x = 50:  1 2 5 10 25 50
		0x800000002000aul,     // x = 51:  1 3 17 51
		0x10000004002016ul,    // x = 52:  1 2 4 13 26 52
		0x20000000000002ul,    // x = 53:  1 53
		0x4000000804024eul,    // x = 54:  1 2 3 6 9 18 27 54
		0x80000000000822ul,    // x = 55:  1 5 11 55
		0x100000010004196ul,   // x = 56:  1 2 4 7 8 14 28 56
		0x20000000008000aul,   // x = 57:  1 3 19 57
		0x400000020000006ul,   // x = 58:  1 2 29 58
		0x800000000000002ul,   // x = 59:  1 59
		0x100000004010947eul,  // x = 60:  1 2 3 4 5 6 10 12 15 20 30 60
		0x2000000000000002ul,  // x = 61:  1 61
		0x4000000080000006ul,  // x = 62:  1 2 31 62
		0x800000000020028aul   // x = 63:  1 3 7 9 21 63
	};

	/*
	public ulong next_lyn()
	{
		if (data == mask) {
			first();
			return 0;
		}
		do {
			next();
		}
		while(is_lyndon_word() == 0);
		return bitCount;
	}
	*/

}
#endif