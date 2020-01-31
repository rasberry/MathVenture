using System;

namespace MathVenture.AltMath
{
	public static class ArcTan
	{
		// https://en.wikipedia.org/wiki/Atan2
		public static double Atan2_1(Func<double,double> fatan, double y, double x)
		{
			if (x >  0.0)             { return fatan(y/x); }
			if (x <  0.0 && y >= 0.0) { return fatan(y/x) + Math.PI; }
			if (x <  0.0 && y <  0.0) { return fatan(y/x) - Math.PI; }
			if (x == 0.0 && y >  0.0) { return Const.MathPIo2; }
			if (x == 0.0 && y <  0.0) { return -Const.MathPIo2; }
			return 0.0;
		}

		public static double Atan2_2(Func<double,double> fatan, double y, double x)
		{
			if (x > 0.0) { return fatan(y/x); }
			if (y > 0.0) { return Const.MathPIo2 - fatan(x/y); }
			if (y < 0.0) { return -Const.MathPIo2 - fatan(x/y); }
			if (x < 0.0) { return fatan(y/x) + Math.PI; }
			return 0.0;
		}

		// http://www.netlib.org/fdlibm/e_atan2.c
		//TODO

		// https://stackoverflow.com/questions/11930594/calculate-atan2-without-std-functions-or-c99
		//normalized_atan(x) ~ ( C x + x^2 + x^3) / ( 1 + (C + 1) x + (C + 1) x^2 + x^3)
		//where C = (1 + sqrt(17)) / 8
		public static double AtanSO1(double r)
		{
			double a = Math.Abs(r);
			//if (a > pio2) { a -= Math.Floor(a/pio2)*pio2; }
			double a2 = a*a, a3 = a*a*a;
			double t1 = a3 + a2 + atanC*a;
			double t2 = a3 + atanCpp*a2 + atanCpp*a + 1;
			return Math.Sign(r) * Const.MathPIo2 * t1 / t2;
		}

		const double atanC = 0.64038820320220756872767623199676;
		const double atanCpp = 1.0 + atanC;

		//http://mathworld.wolfram.com/InverseTangent.html
		//Maclaurin series
		public static double AtanMac(double r, int accuracy = 8)
		{
			if (r < -1 || r > 1) {
				return Math.Sign(r) * Const.MathPIo2 - AtanMac(1.0/r);
			}
			double z = 0.0;
			for(int n=0; n<accuracy; n++) {
				double m1 = n % 2 == 0 ? 1 : -1;
				double tnp1 = 2 * n + 1;
				z += m1 * Math.Pow(r,tnp1) / tnp1;
			}
			return z;
		}

		//http://mathworld.wolfram.com/InverseTangent.html
		public static double AtanActon(double r, int accuracy = 8)
		{
			double a = 1.0 / Math.Sqrt(r*r + 1.0);
			double b = 1.0;

			for(int i=0; i<accuracy; i++) {
				double an = (a + b) / 2.0;
				double bn = Math.Sqrt(an * b);
				a = an; b = bn;
			}

			double t = r / (a * Math.Sqrt(1 + r*r));
			return t;
		}

		//https://www.ams.org/journals/mcom/1954-08-047/S0025-5718-1954-0063487-2/S0025-5718-1954-0063487-2.pdf
		public static double AtanAms(double r)
		{
			return NBSApplied.Atan(r);
		}

		// http://www.netlib.org/fdlibm/e_atanh.c
		static double[] AtanHI = new double[] {
			4.63647609000806093515e-01, /* atan(0.5)hi 0x3FDDAC67, 0x0561BB4F */
			7.85398163397448278999e-01, /* atan(1.0)hi 0x3FE921FB, 0x54442D18 */
			9.82793723247329054082e-01, /* atan(1.5)hi 0x3FEF730B, 0xD281F69B */
			1.57079632679489655800e+00, /* atan(inf)hi 0x3FF921FB, 0x54442D18 */
		};
		static double[] AtanLO = new double[] {
			2.26987774529616870924e-17, /* atan(0.5)lo 0x3C7A2B7F, 0x222F65E2 */
			3.06161699786838301793e-17, /* atan(1.0)lo 0x3C81A626, 0x33145C07 */
			1.39033110312309984516e-17, /* atan(1.5)lo 0x3C700788, 0x7AF0CBBD */
			6.12323399573676603587e-17, /* atan(inf)lo 0x3C91A626, 0x33145C07 */
		};
		static double[] AT = new double[] {
			 3.33333333333329318027e-01, /* 0x3FD55555, 0x5555550D */
			-1.99999999998764832476e-01, /* 0xBFC99999, 0x9998EBC4 */
			 1.42857142725034663711e-01, /* 0x3FC24924, 0x920083FF */
			-1.11111104054623557880e-01, /* 0xBFBC71C6, 0xFE231671 */
			 9.09088713343650656196e-02, /* 0x3FB745CD, 0xC54C206E */
			-7.69187620504482999495e-02, /* 0xBFB3B0F2, 0xAF749A6D */
			 6.66107313738753120669e-02, /* 0x3FB10D66, 0xA0D03D51 */
			-5.83357013379057348645e-02, /* 0xBFADDE2D, 0x52DEFD9A */
			 4.97687799461593236017e-02, /* 0x3FA97B4B, 0x24760DEB */
			-3.65315727442169155270e-02, /* 0xBFA2B444, 0x2C6A6C2F */
			 1.62858201153657823623e-02, /* 0x3F90AD3A, 0xE322DA11 */
		};
		const double One = 1.0;
		const double Huge = 1.0e300;

		public static double Atanfdlibm(double x)
		{
			if (x < -1.0 || x > 1.0) {
				return Math.Sign(x) * Const.MathPIo2 - Atanfdlibm(1.0/x);
			}

			double w,s1,s2,z;
			int ix,hx,id;

			hx = Helpers.HI(x);
			ix = hx & 0x7fffffff;
			if (ix >= 0x44100000) {        /* if |x| >= 2^66 */
				if(ix > 0x7ff00000 || (ix == 0x7ff00000 && (Helpers.LO(x) != 0))) {
					return x+x;            /* NaN */
				}
				if (hx > 0) { return  AtanHI[3] + AtanLO[3]; }
				else        { return -AtanHI[3] - AtanLO[3]; }
			}
			if (ix < 0x3fdc0000) {         /* |x| < 0.4375 */
				if (ix < 0x3e200000) {     /* |x| < 2^-29 */
					if (Huge + x > One) { return x; } /* raise inexact */
				}
				id = -1;
			}
			else {
				x = Math.Abs(x);
				if (ix < 0x3ff30000) {     /* |x| < 1.1875 */
					if (ix < 0x3fe60000) { /* 7/16 <=|x|<11/16 */
						id = 0;
						x = (2.0 * x - One) / (2.0 + x);
					}
					else {                 /* 11/16<=|x|< 19/16 */
						id = 1;
						x = (x - One) / (x + One);
					}
				}
				else {
					if (ix < 0x40038000) { /* |x| < 2.4375 */
						id = 2;
						x = (x - 1.5) / (One + 1.5 * x);
					}
					else {                 /* 2.4375 <= |x| < 2^66 */
						id = 3;
						x = -1.0 / x;
					}
				}
			}
			/* end of argument reduction */
			z = x * x;
			w = z * z;
			/* break sum from i=0 to 10 aT[i]z**(i+1) into odd and even poly */
			s1 = z * (AT[0] + w * (AT[2] + w * (AT[4] + w * (AT[6] + w * (AT[8] + w * AT[10])))));
			s2 = w * (AT[1] + w * (AT[3] + w * (AT[5] + w * (AT[7] + w * AT[9]))));
			if (id < 0) {
				return x - x * (s1 + s2);
			}
			else {
				z = AtanHI[id] - ((x * (s1 + s2) - AtanLO[id]) - x);
				return (hx < 0) ? -z : z;
			}
		}
	}
}
