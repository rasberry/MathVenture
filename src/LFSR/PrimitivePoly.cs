using System.Numerics;
namespace MathVenture.LFSR;

public static class PrimitivePoly
{
	public static void FindAll(ulong degree = 9)
	{
		var sb = new System.Text.StringBuilder();

		ulong n = degree;
		ulong st = 0;
		//ulong fm = 2;

		var ip = new all_irredpoly(n);
		ulong p = ip.data();
		ulong ict = 0;

		ulong pct = 0;
		ulong nct = 0;
		ulong npct = 0;

		do {
			++ict;
			bool pq = ip.is_primitive();
			if (pq) { pct ++; }
			bool nq = Bits.bitpol_normal_q(p,n,true);
			if(nq) { nct ++; }
			if (nq && pq) { npct ++; }

			if (pq) {
				if (nq) { sb.Append("  N"); }
				else    { sb.Append("   "); }
				if (pq) { sb.Append("  P"); }
				else    { sb.Append("   "); }

				sb.Append("    ");
				Bits.bitpol_print_coeff(sb,"",p);
				Log.Message(sb.ToString());
			}
			sb.Clear();
			if (ict == st) { break; }
			p = ip.next();
		}
		while(p != 0);
	}
}

public class all_irredpoly
{
	public ulong p_;
	public ulong mers_;
	public bit_necklace bn_;
	public necklace2bitpol n2b_;

	public all_irredpoly(ulong n, ulong c = 0, ulong a = 0)
	{
		p_ = 0;
		mers_ = 0;
		bn_ = new bit_necklace(0);
		n2b_ = new necklace2bitpol(n,c,a);
		init(n);
	}

	public void init(ulong n = 0, ulong c = 0, ulong a = 0)
	{
		if (n != 0) {
			mers_ = ~0ul;
			if (n < Extra.BITS_PER_LONG) {
				mers_ = (1ul << (int)n) - 1;
			}
		}

		bn_.init(n);
		n2b_.init(n,c,a);
		next();
	}

	public ulong data() { return p_; }

	public ulong next()
	{
		ulong b;
		do {
			b = bn_.next();
			if (b == 0) { return 0; }
		}
		while(0 == bn_.is_lyndon_word());

		p_ = n2b_.poly(bn_.data());
		return p_;
	}

	public bool is_primitive()
	{
		ulong b = bn_.data();
		return 1 == Extra.gcd(b,mers_);
	}

	public ulong next_primitive()
	{
		ulong b;
		do {
			do {
				b = bn_.next();
				if (b == 0) { return 0; }
			}
			while(0 == bn_.is_lyndon_word());
		}
		while(is_primitive() == false);

		p_ = n2b_.poly(bn_.data());
		return p_;
	}
}

public class bit_necklace
{
	public ulong a_;   // necklace
	public ulong j_;   // period of the necklace
	public ulong n2_;  // bit representing n: n2==2**(n-1)
	public ulong j2_;  // bit representing j: j2==2**(j-1)
	public ulong n_;   // number of bits in words
	public ulong mm_;  // mask of n ones
	public ulong tfb_;  // for fast factor lookup

	public bit_necklace(ulong n)
	{
		init(n);
	}

	public void init(ulong n)
	{
		if ( 0==n )  n = 1;  // avoid hang
		if ( n>=Extra.BITS_PER_LONG ) {
			n = Extra.BITS_PER_LONG;
		}
		n_ = n;

		n2_ = 1ul << (int)(n-1);
		mm_ = (~0ul) >> (int)(Extra.BITS_PER_LONG - n);
		tfb_ = Extra.tiny_factors_tab[n] >> 1;
		tfb_ |= n2_;  // needed for n==BITS_PER_LONG
		first();
	}

	public void first()
	{
		a_ = 0;
		j_ = 1;
		j2_ = 1;
	}

	public ulong data() { return a_; }
	public ulong period() { return j_; }

	public ulong next()
	{
		if (a_ == mm_) {
			first();
			return 0;
		}

		do {
			j_ = n_ - 1;
			ulong jb = 1ul << (int)j_;
			while((a_ & jb) != 0) {
				--j_;
				jb >>= 1;
			}
			j2_ = 1ul << (int)j_;
			++j_;
			a_ |= j2_;
			a_ = Bits.bit_copy_periodic(a_, j_, n_);
		}
		while((tfb_ & j2_) == 0);
		return j_;
	}

	public ulong is_lyndon_word()
	{
		return j2_ & n2_;
	}

	public ulong next_lyn()
	{
		if (a_ == mm_) {
			first();
			return 0;
		}
		do {
			next();
		}
		while(is_lyndon_word() == 0);
		return n_;
	}

}

public class necklace2bitpol
{
	public ulong n_;  // degree of c_
	public ulong c_;  // modulus (irreducible polynomial)
	public ulong h_;  // mask used for computation
	public ulong a_;  // generator modulo c
	public ulong e_;  // a^b
	public ulong bp_;  // result as bit-vector

	public necklace2bitpol(ulong n, ulong c = 0, ulong a = 0)
	{
		n_ = n;
		c_ = c;
		a_ = a;
		init(n,c,a);
	}

	public void init(ulong n, ulong c = 0, ulong a = 0)
	{
		if (c == 0) { c_ = Extra.lowbit_primpoly[n_]; }
		if (a == 0) { a_ = 2ul; }
		h_ = 1ul << (int)(n-1);
	}

	public ulong poly(ulong b)
	{
		ulong e = Bits.bitpolmod_power(a_,b,c_,h_);
		e_ = e;
		ulong x = 2;
		ulong s = e;
		ulong m = 1ul;
		for(ulong j = 0; j < n_; ++j) {
			ulong t = x ^ s;
			m = Bits.bitpolmod_mult(m,t,c_,h_);
			s = Bits.bitpolmod_square(s,c_,h_);
		}
		bp_ = m ^ c_;
		return bp_;
	}
}

static class Extra
{
	public const ulong BITS_PER_LONG = 64;

	public static ulong[] tiny_factors_tab = new ulong[] {
		               0x0ul,  // x = 0:            ( bits: ........)
		               0x2ul,  // x = 1:   1        ( bits: ......1.)
		               0x6ul,  // x = 2:   1 2      ( bits: .....11.)
		               0xaul,  // x = 3:   1 3      ( bits: ....1.1.)
		              0x16ul,  // x = 4:   1 2 4    ( bits: ...1.11.)
		              0x22ul,  // x = 5:   1 5      ( bits: ..1...1.)
		              0x4eul,  // x = 6:   1 2 3 6  ( bits: .1..111.)
		              0x82ul,  // x = 7:   1 7      ( bits: 1.....1.)
		             0x116ul,  // x = 8:   1 2 4 8
		             0x20aul,  // x = 9:   1 3 9
		             0x426ul,  // x = 10:  1 2 5 10
		             0x802ul,  // x = 11:  1 11
		            0x105eul,  // x = 12:  1 2 3 4 6 12
		            0x2002ul,  // x = 13:  1 13
		            0x4086ul,  // x = 14:  1 2 7 14
		            0x802aul,  // x = 15:  1 3 5 15
		           0x10116ul,  // x = 16:  1 2 4 8 16
		           0x20002ul,  // x = 17:  1 17
		           0x4024eul,  // x = 18:  1 2 3 6 9 18
		           0x80002ul,  // x = 19:  1 19
		          0x100436ul,  // x = 20:  1 2 4 5 10 20
		          0x20008aul,  // x = 21:  1 3 7 21
		          0x400806ul,  // x = 22:  1 2 11 22
		          0x800002ul,  // x = 23:  1 23
		         0x100115eul,  // x = 24:  1 2 3 4 6 8 12 24
		         0x2000022ul,  // x = 25:  1 5 25
		         0x4002006ul,  // x = 26:  1 2 13 26
		         0x800020aul,  // x = 27:  1 3 9 27
		        0x10004096ul,  // x = 28:  1 2 4 7 14 28
		        0x20000002ul,  // x = 29:  1 29
		        0x4000846eul,  // x = 30:  1 2 3 5 6 10 15 30
		        0x80000002ul,  // x = 31:  1 31
		       0x100010116ul,  // x = 32:  1 2 4 8 16 32
		       0x20000080aul,  // x = 33:  1 3 11 33
		       0x400020006ul,  // x = 34:  1 2 17 34
		       0x8000000a2ul,  // x = 35:  1 5 7 35
		      0x100004125eul,  // x = 36:  1 2 3 4 6 9 12 18 36
		      0x2000000002ul,  // x = 37:  1 37
		      0x4000080006ul,  // x = 38:  1 2 19 38
		      0x800000200aul,  // x = 39:  1 3 13 39
		     0x10000100536ul,  // x = 40:  1 2 4 5 8 10 20 40
		     0x20000000002ul,  // x = 41:  1 41
		     0x400002040ceul,  // x = 42:  1 2 3 6 7 14 21 42
		     0x80000000002ul,  // x = 43:  1 43
		    0x100000400816ul,  // x = 44:  1 2 4 11 22 44
		    0x20000000822aul,  // x = 45:  1 3 5 9 15 45
		    0x400000800006ul,  // x = 46:  1 2 23 46
		    0x800000000002ul,  // x = 47:  1 47
		   0x100000101115eul,  // x = 48:  1 2 3 4 6 8 12 16 24 48
		   0x2000000000082ul,  // x = 49:  1 7 49
		   0x4000002000426ul,  // x = 50:  1 2 5 10 25 50
		   0x800000002000aul,  // x = 51:  1 3 17 51
		  0x10000004002016ul,  // x = 52:  1 2 4 13 26 52
		  0x20000000000002ul,  // x = 53:  1 53
		  0x4000000804024eul,  // x = 54:  1 2 3 6 9 18 27 54
		  0x80000000000822ul,  // x = 55:  1 5 11 55
		 0x100000010004196ul,  // x = 56:  1 2 4 7 8 14 28 56
		 0x20000000008000aul,  // x = 57:  1 3 19 57
		 0x400000020000006ul,  // x = 58:  1 2 29 58
		 0x800000000000002ul,  // x = 59:  1 59
		0x100000004010947eul,  // x = 60:  1 2 3 4 5 6 10 12 15 20 30 60
		0x2000000000000002ul,  // x = 61:  1 61
		0x4000000080000006ul,  // x = 62:  1 2 31 62
		0x800000000020028aul   // x = 63:  1 3 7 9 21 63
	};


	//
	//  Primitive polynomials over Z/2Z
	//  with lowest-most possible set bits.
	//  These are the minimal numbers with the corresponding
	//  polynomial of given degree primitive.
	//
	//  Generated by Joerg Arndt, 2002-December-21
	//
	public static ulong[] lowbit_primpoly = new ulong[] {
		//  hex_val,          // ==dec_val            (deg)   [weight]
		0x1,                  // ==1                    (0)   [1]
		0x3,                  // ==3                    (1)   [2]
		0x7UL,                // ==7                    (2)   [3]
		0xbUL,                // ==11                   (3)   [3]
		0x13UL,               // ==19                   (4)   [3]
		0x25UL,               // ==37                   (5)   [3]
		0x43UL,               // ==67                   (6)   [3]
		0x83UL,               // ==131                  (7)   [3]
		0x11dUL,              // ==285                  (8)   [5]
		0x211UL,              // ==529                  (9)   [3]
		0x409UL,              // ==1033                 (10)  [3]
		0x805UL,              // ==2053                 (11)  [3]
		0x1053UL,             // ==4179                 (12)  [5]
		0x201bUL,             // ==8219                 (13)  [5]
		0x402bUL,             // ==16427                (14)  [5]
		0x8003UL,             // ==32771                (15)  [3]
		0x1002dUL,            // ==65581                (16)  [5]
		0x20009UL,            // ==131081               (17)  [3]
		0x40027UL,            // ==262183               (18)  [5]
		0x80027UL,            // ==524327               (19)  [5]
		0x100009UL,           // ==1048585              (20)  [3]
		0x200005UL,           // ==2097157              (21)  [3]
		0x400003UL,           // ==4194307              (22)  [3]
		0x800021UL,           // ==8388641              (23)  [3]
		0x100001bUL,          // ==16777243             (24)  [5]
		0x2000009UL,          // ==33554441             (25)  [3]
		0x4000047UL,          // ==67108935             (26)  [5]
		0x8000027UL,          // ==134217767            (27)  [5]
		0x10000009UL,         // ==268435465            (28)  [3]
		0x20000005UL,         // ==536870917            (29)  [3]
		0x40000053UL,         // ==1073741907           (30)  [5]
		0x80000009UL,         // ==2147483657           (31)  [3]
		0x1000000afUL,        // ==4294967471           (32)  [7]
		0x200000053UL,        // ==8589934675           (33)  [5]
		0x4000000e7UL,        // ==17179869415          (34)  [7]
		0x800000005UL,        // ==34359738373          (35)  [3]
		0x1000000077UL,       // ==68719476855          (36)  [7]
		0x200000003fUL,       // ==137438953535         (37)  [7]
		0x4000000063UL,       // ==274877907043         (38)  [5]
		0x8000000011UL,       // ==549755813905         (39)  [3]
		0x10000000039UL,      // ==1099511627833        (40)  [5]
		0x20000000009UL,      // ==2199023255561        (41)  [3]
		0x4000000003fUL,      // ==4398046511167        (42)  [7]
		0x80000000059UL,      // ==8796093022297        (43)  [5]
		0x100000000065UL,     // ==17592186044517       (44)  [5]
		0x20000000001bUL,     // ==35184372088859       (45)  [5]
		0x40000000012fUL,     // ==70368744177967       (46)  [7]
		0x800000000021UL,     // ==140737488355361      (47)  [3]
		0x10000000000b7UL,    // ==281474976710839      (48)  [7]
		0x2000000000071UL,    // ==562949953421425      (49)  [5]
		0x400000000001dUL,    // ==1125899906842653     (50)  [5]
		0x800000000004bUL,    // ==2251799813685323     (51)  [5]
		0x10000000000009UL,   // ==4503599627370505     (52)  [3]
		0x20000000000047UL,   // ==9007199254741063     (53)  [5]
		0x4000000000007dUL,   // ==18014398509482109    (54)  [7]
		0x80000000000047UL,   // ==36028797018964039    (55)  [5]
		0x100000000000095UL,  // ==72057594037928085    (56)  [5]
		0x20000000000002dUL,  // ==144115188075855917   (57)  [5]
		0x400000000000063UL,  // ==288230376151711843   (58)  [5]
		0x80000000000007bUL,  // ==576460752303423611   (59)  [7]
		0x1000000000000003UL, // ==1152921504606846979  (60)  [3]
		0x2000000000000027UL, // ==2305843009213693991  (61)  [5]
		0x4000000000000069UL, // ==4611686018427388009  (62)  [5]
		0x8000000000000003UL, // ==9223372036854775811  (63)  [3]
	};

	public static T gcd<T>(T a, T b) where T : IBinaryInteger<T>
	{

		while (!T.IsZero(b)) {
			T r = a % b;
			a = b;
			b = r;
		}

		return a;
	}
}

public static class Bits
{
	// Return word that consists of the lowest p bits of a repeated
	// in the lowest ldn bits (upper bits are zero).
	// E.g.: if p==3, ldn=7 and a=*****xyz (8-bit), the return 0zxyzxyz.
	// Must have p>0 and ldn>0.
	public static ulong bit_copy_periodic(ulong a, ulong p, ulong ldn)
	{
		a &= ~0ul >> (int)(Extra.BITS_PER_LONG-p) ;
		for (ulong s = p; s < ldn; s <<= 1)  { a |= a << (int)s; }
		a &= ~0ul >> (int)(Extra.BITS_PER_LONG-ldn);
		return a;
	}

	// Return A*A mod C
	// == bitpolmod_mult(a, a, c, h);
	// compute \sum_{k=0}^{d}{a_k\,x^k} as \sum_{k=0}^{d}{a_k\,x^{2k}}
	public static ulong bitpolmod_square(ulong a, ulong c, ulong h)
	{
		return bitpolmod_mult(a, a, c, h);
	}

	// Return  (A * B) mod C
	//
	// Must have  deg(A) < deg(C)  and  deg(B) < deg(C)
	//.
	// With b=2 (== 'x') the result is identical to
	//   bitpolmod_times_x(a, c, h)
	public static ulong bitpolmod_mult(ulong a, ulong b, ulong c, ulong h)
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

	// Return (A ** e)  mod C
	public static ulong bitpolmod_power(ulong a, ulong e, ulong c, ulong h)
	{
		ulong s = a;
		ulong b = highest_one(e);
		while(b > 1ul) {
			b >>= 1;
			s = bitpolmod_square(s,c,h); // s *= s;
			if ((e & b) != 0) {
				s = Bits.bitpolmod_mult(s,a,c,h); // s *= a;
			}
		}
		return s;
	}

	// Return word where only the highest bit in x is set.
	// Return 0 if no bit is set.
	public static ulong highest_one(ulong x)
	{
		x = highest_one_01edge(x);
		return  x ^ (x >> 1);
	}

	// Return word where all bits from (including) the
	//   highest set bit to bit 0 are set.
	// Return 0 if no bit is set.
	//
	// Feed the result into bit_count() to get
	//   the index of the highest bit set.
	public static ulong highest_one_01edge(ulong x)
	{
		x |= x>>1;
		x |= x>>2;
		x |= x>>4;
		x |= x>>8;
		x |= x>>16;
		x |= x>>32;
		return  x;
	}

	// print as:  [7, 4, 3, 0]
	public static void bitpol_print_coeff(System.Text.StringBuilder sb, string bla, ulong c)
	{
		sb.Append(bla);
		sb.Append('[');
		ulong i = Extra.BITS_PER_LONG - 1;
		ulong t = 1ul << (int)i;
		while ( c != 0ul ) {
			ulong q = c & t;
			if (q != 0ul) {
				c ^= q;
				sb.Append(i);
				if ( c != 0ul) {
					sb.Append(',');
				}
			}
			t >>= 1;
			--i;
		}
		sb.Append(']');
	}

	// Return whether polynomial c (of degree n) is normal.
	// Set iq to 1 for polynomials known to be irreducible.
	public static bool bitpol_normal_q(ulong c, ulong n, bool iq)
	{
		ulong h = 1ul << (int)(n - 1);
		if ((c & h) == 0ul) { return false; } // polynomial trace must be one
		if (iq == false) {
			iq = bitpol_irreducible_q(c,h);
			if (iq == false) { return false; } // polynomial reducible ==> not normal
		}
		if (c == 3ul) { // special case with c=x+1
			return true;
		}

		ulong[] A = new ulong[n];
		ulong v = 2ul;
		for(ulong k = 0; k < n; ++k) {
			A[k] = v;
			v = bitpolmod_square(v,c,h);
		}

		bool q = bitmat_inverse(A,n,out _);
		if (q == false) { return false; }

		return true;
	}

	//
	// Compute Mi=M^(-1)*B or Mi=M^(-1) if B not given.
	// Return false if M is not invertible.
	//.
	// Algorithm as in Cohen p.48
	public static bool bitmat_inverse(ulong[] M, ulong n, out ulong[] Mi)
	{
		Mi = new ulong[n];
		ulong[] T = new ulong[n];
		bitmat_unit(T,n);

		for(ulong k = 0; k < n; ++k) {
			Mi[k] = M[k];
		}

		for (ulong j = 0; j < n; ++j) {
			// 3 [Find non-zero entry]:
			ulong i = j;
			do {
				if (bitmat_get(Mi,i,j) == 0) { break; }
			}
			while(++i < n);

			if (i == n) { return false; } // M is not invertible

			// 4 [Swap?]:
			if (i > j) {
				swap2(ref Mi[i],ref Mi[j]);
				swap2(ref T[i], ref T[j]);
			}

			ulong c = 0;
			for(ulong k = j + 1; k < n; ++k) {
				ulong v = bitmat_get(Mi,k,j);
				vSet(ref c,k,v);
			}

			for(ulong k = j + 1; k < n; ++k) {
				ulong ck = vGet(c,k);
				if (ck != 0ul) {
					Mi[k] ^= Mi[j];
				}
			}

			for (ulong k = j + 1; k < n; ++k) {
				if (vGet(c, k) != 0ul) { T[k] ^= T[j]; }
			}
		}

		// 6 [Solve triangular system]:
		ulong[] X = T;
		ulong ii = n;
		do {
			--ii;
			ulong v = 0;
			for (ulong j = ii + 1; j < n; ++j) {
				if (bitmat_get(Mi, ii, j) != 0ul) { v ^= X[j]; }
			}
		}
		while(ii != 0ul);

		for (ulong k = 0; k < n; ++k) { Mi[k] = X[k]; }
		return true;
	}

	static void vSet(ref ulong v, ulong j, ulong s)
	{
		ulong bj = 1ul << (int)j;
		if (s != 0ul ) { v |= bj; }
		else { v &=~ bj; }
	}

	static ulong vGet(ulong v, ulong j) {
		return (v >> (int)j) & 1ul;
	}

	public static ulong bitmat_get(ulong[] M, ulong r, ulong c)
	{
		return (M[r] >> (int)c) & 1ul;
	}

	public static void swap2<T>(ref T x, ref T y) where T : IBinaryInteger<T>
	{
		T t = x;
		x = y;
		y = t;
	}

	public static void bitmat_unit(ulong[] M, ulong n)
	{
		for (ulong k=0, b=1ul; k < n; ++k, b <<= 1) { M[k] = b; }
	}

	public static bool bitpol_irreducible_q(ulong c, ulong h)
	{
		if ((h << 1) == 0ul) {
			return bitpol_spi_q(c, h);
		}

		return bitpol_irreducible_ben_or_q(c,h);
	}

	// Return whether C is a strong pseudo irreducible (SPI).
	// A polynomial C of degree d is an SPI if
	//   it has no linear factors, x^(2^k)!=x for 0<k<d, and x^(2^d)==x.
	// h needs to be a mask with one bit set:
	//  h == highest_one(C) >> 1  == 1UL << (degree(C)-1)
	public static bool bitpol_spi_q(ulong c, ulong h)
	{
		bool md = (h << 1) == 0ul;  // whether degree == BITS_PER_LONG
		if (md) {
			if ((c & 1) == 0ul) { return false; }    // factor x
			if (0ul != parity(c)) { return false; }  // factor x+1
		}
		else
		{
			if (c < 4ul) {  // C is one of 0, 1, x, 1+x
				if (c >= 2ul) { return true; }  // x, and 1+x are irreducible
				else { return false; } // constant polynomials are reducible
			}

			if ((c & 1) == 0ul) { return false; }    // factor x
			if (0ul == parity(c)) { return false; }  // factor x+1
		}

		ulong t = 1;
		ulong m = 2;  // x
		m = bitpolmod_square(m, c, h);
		do {
			if (m == 2ul) { return false; }
			m = bitpolmod_square(m, c, h);
			t <<= 1;
		}
		while(t != h);

		if (m != 2ul) { return false; }

		return true;
	}

	// Return 0 if the number of set bits is even, else 1
	public static ulong parity(ulong x)
	{
		return inverse_gray_code(x) & 1ul;
	}

	// inverse of gray_code()
	// note: the returned value contains at each bit position
	// the parity of all bits of the input left from it (incl. itself)
	public static ulong inverse_gray_code(ulong x)
	{
		// ----- VERSION 3 (use: gray ** BITSPERLONG == id):
		x ^= x>>1;  // gray ** 1
		x ^= x>>2;  // gray ** 2
		x ^= x>>4;  // gray ** 4
		x ^= x>>8;  // gray ** 8
		x ^= x>>16;  // gray ** 16
		x ^= x>>32;  // for 64bit words
		return x;
	}

	// Return whether C is irreducible (via the Ben-Or irreducibility test_;
	// h needs to be a mask with one bit set:
	//  h == highest_one(C) >> 1  == 1UL << (degree(C)-1)
	// Note: routine will fail for deg(c)==BITS_PER_LONG (GCD fails)
	public static bool bitpol_irreducible_ben_or_q(ulong c, ulong h)
	{
		if (c < 4ul) {
			if (c >= 2ul) { return true; } // x, and 1+x are irreducible
			else { return false; }         // constant polynomials are reducible
		}

		if ((1ul & c) == 0ul) { return false; } // x is a factor

		ulong d = h >> 1;
		ulong u = 2;  // =^= x
		while (0 != d) { // floor( degree/2 ) times
			// Square r-times for coefficients of c in GF(2^r).
			// We have r==1:
			u = bitpolmod_square(u, c, h);
			ulong upx = u ^ 2;  // =^= u+x

			// Note: GCD does not work for work for deg(C)==BITS_PER_LONG
			ulong g = bitpol_binary_gcd(upx, c);
			if (1ul != g) { return false; }  // reducible
			d >>= 2;
		}
		return true;  // irreducible
	}

	// Return polynomial gcd(A, B)
	public static ulong bitpol_binary_gcd(ulong a, ulong b)
	{
		if (a == 0ul || b == 0ul) { return a | b; } // one (or both) of a, b zero?
		ulong ka = lowest_one_idx(a);
		a >>= (int)ka;
		ulong kb = lowest_one_idx(b);
		b >>= (int)kb;
		ulong k =  ka < kb ? ka : kb ;

		while (a != b) {
			if (a < b)  { ulong tt=a; a=b; b=tt; }  // swap if deg(A)<deg(B)
			ulong t = (a ^ b) >> 1;
			a = t >> (int)lowest_one_idx(t);
		}
		return  a << (int)k;
	}

	// Return index of lowest bit set.
	// Bit index ranges from zero to BITS_PER_LONG-1.
	// Examples:
	//    ***1 --> 0
	//    **10 --> 1
	//    *100 --> 2
	// Return 0 (also) if no bit is set.
	public static ulong lowest_one_idx(ulong x)
	{
		ulong r = 0;
		x &= ulong.MaxValue - x;  // isolate lowest bit

		if ((x & 0xffffffff00000000ul) == 0ul) { r += 32; }
		if ((x & 0xffff0000ffff0000ul) == 0ul) { r += 16; }
		if ((x & 0xff00ff00ff00ff00ul) == 0ul) { r += 8;  }
		if ((x & 0xf0f0f0f0f0f0f0f0ul) == 0ul) { r += 4;  }
		if ((x & 0xccccccccccccccccul) == 0ul) { r += 2;  }
		if ((x & 0xaaaaaaaaaaaaaaaaul) == 0ul) { r += 1;  }

		return r;
	}
}