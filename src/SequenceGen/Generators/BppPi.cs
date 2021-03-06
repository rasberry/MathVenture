using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;

namespace MathVenture.SequenceGen.Generators
{
	// https://www.experimentalmath.info/bbp-codes/
	// https://en.wikipedia.org/wiki/Bailey%E2%80%93Borwein%E2%80%93Plouffe_formula

	public class BppPi : IGenerator
	{
		public Digit Next { get {
			if (Gen == null) {
				Gen = GetDigits().GetEnumerator();
			}
			Gen.MoveNext();
			return Gen.Current;
		}}

		public void Reset()
		{
			Gen = null;
		}
		static IEnumerator<Digit> Gen = null;

		static IEnumerable<Digit> GetDigits()
		{
			yield return new Digit(3,16); //algorithm doesn't produce the first whole number
			int id = 0;
			while(true)
			{
				var listN0 = piqr8c(id);
				var listN2 = piqr8c(id+2); //checker
				//output first ones since we're sure they're accurate
				id++; yield return listN0[0];
				id++; yield return listN0[1];
				//output untill one doesn't match
				for(int n=2; n<listN0.Length; n++) {
					if (listN0[n] != listN2[n-2]) { break; }
					id++; yield return listN0[n];
				}
			}
		}

		static Digit[] piqr8c(int id)
		{
			double pid, s1, s2, s3, s4;
			const int NHX = 16;
			s1 = Series(1, id);
			s2 = Series(4, id);
			s3 = Series(5, id);
			s4 = Series(6, id);
			pid = 4.0 * s1 - 2.0 * s2 - s3 - s4;
			pid = pid - (int) pid + 1.0;
			// Log.Debug($"s1={s1} s2={s2} s3={s3} s4={s4} pid={pid}");
			return IHex(pid, NHX);
		}

		// returns the first nhx hex digits of the fraction of x.
		static Digit[] IHex(double x, int nhx)
		{
			double y = Math.Abs(x);
			var list = new Digit[nhx];
			for (int i = 0; i < nhx; i++) {
				y = 16.0 * (y - Math.Floor(y));
				list[i] = new Digit((int)y,16);
			}
			return list;
		}

		// This evaluates the series  sum_k 16^(id-k)/(8*k+m)
		// using the modular exponentiation technique.
		static double Series(int m, int id)
		{
			double t,p,ak;
			const double eps = 1e-17;
			double s = 0.0;

			// Sum the series up to id.
			for (int k = 0; k < id; k++) {
				ak = 8 * k + m;
				p = id - k;
				t = ModPow(p, ak);
				s = s + t / ak;
				s = s - (int) s;
			}

			// Compute a few terms where k >= id.
			for (int k = id; k <= id + 100; k++) {
				ak = 8 * k + m;
				t = Math.Pow(16.0, (double)(id - k)) / ak;
				if (t < eps) break;
				s = s + t;
				s = s - (int) s;
			}
			return s;
		}

		// expm = 16^p mod ak.  This routine uses the left-to-right binary
		// exponentiation scheme.
		// https://en.wikipedia.org/wiki/Modular_exponentiation
		static double ModPow(double p, double ak)
		{
			if (ak == 1.0) { return 0.0; }

			double p1 = p, r = 1.0;
			// Find the greatest power of two less than or equal to p.
			int i = (int)Math.Log(p,2);
			double pt = Math.Pow(2,i);

			// Perform binary exponentiation algorithm modulo ak.
			for (int j = 0; j <= i; j++) {
				if (p1 >= pt) {
					r = 16.0 * r;
					r = r - (int)(r / ak) * ak;
					p1 = p1 - pt;
				}
				pt = 0.5 * pt;
				if (pt >= 1.0) {
					r = r * r;
					r = r - (int)(r / ak) * ak;
				}
			}
			// Log.Debug($"p={p} ak={ak} r={r}");
			return r;
		}
	}
}
