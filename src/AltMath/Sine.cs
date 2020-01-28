using System;

namespace MathVenture.AltMath
{
	public static class Sine
	{
		// https://stackoverflow.com/questions/2284860/how-does-c-compute-sin-and-other-math-functions
		// https://stackoverflow.com/a/2284929
		public static double SinSO(double a)
		{
			a %= Const.Math2PI;

			int i = 0;
			double cur = a;
			double acc = 1.0;
			double fact= 1.0;
			double pow = a;
			while (Math.Abs(acc) > double.Epsilon && ++i < 100.0)
			{
				fact *= ((2*i)*(2*i+1));
				pow *= -1 * a*a;
				acc = pow / fact;
				cur += acc;
			}
			return cur;
		}

		// https://stackoverflow.com/a/58203008
		public static double SinSO2(double x)
		{
			int k = 2;
			double r = x, acc = 1, den = 1, num = x;

			// precision drops rapidly when x is not close to 0
			// so move x to 0 as close as possible
			while(x > Math.PI) { x -= Math.PI; }
			while(x < -Math.PI) { x += Math.PI; }
			if (x > Const.MathPIo2) {
				x = Math.PI - x;
			}
			if (x < -Const.MathPIo2) {
				x = -Math.PI - x;
			}
			// not using Math.Abs for performance reasons
			while(acc > double.Epsilon || acc < -double.Epsilon) {
				num *= -x * x;
				den *= k * (k + 1);
				acc = num / den;
				r += acc;
				k += 2;
			}
			return r;
		}

		// https://gist.github.com/XupremZero/49f82c9e21b42ac67a1f4e085c00226c
		public static float SinXupremZero(float x) //x in radians
		{
			x %= 6.28318531f;

			float sinn;
			if (x < -3.14159265f) {
				x += 6.28318531f;
			}
			else if (x > 3.14159265f) {
				x -= 6.28318531f;
			}

			float sgn = x < 0 ? 1.0f : -1.0f;
			sinn = 1.27323954f * x + sgn * 0.405284735f * x * x;
			if (sinn < 0) {
				sinn = 0.225f * (sinn * -sinn - sinn) + sinn;
			} else {
				sinn = 0.225f * (sinn * sinn - sinn) + sinn;
			}
			return sinn;
		}
		
		public static float CosXupremZero(float x) //x in radians
		{
			return SinXupremZero(x + 1.5707963f);
		}

		// sin(x*pi/2) [-1,1]
		static double[] Sin5Coefs = new double[] {
			 1.08801856413265E-16, 1.13364817781175    ,-1.33226762955019E-17,-0.138071776587192   ,
			 1.99840144432528E-16, 0.00449071424655495 ,-8.43769498715119E-17,-6.77012758421158E-05,
			-9.10382880192628E-17, 5.89129532919674E-07, 9.76996261670138E-17,-3.33805964203293E-09,
			 3.10862446895044E-17, 1.32970745525540E-11, 1.02140518265514E-16,-3.94373422807348E-14,
			 1.46549439250521E-16, 1.06581410364015E-16, 2.13162820728030E-16, 6.21724893790088E-17,
			 4.44089209850063E-18,-2.17603712826531E-16, 2.08721928629529E-16, 2.48689957516035E-16,
			 8.65973959207622E-17,-1.15463194561016E-16, 1.57651669496772E-16,-2.39808173319034E-16,
			-8.68194405256872E-16, 6.70574706873595E-16, 6.88338275267597E-16,-5.92859095149834E-16,
			 3.61932706027801E-16,-2.99760216648792E-16,-1.35225164399344E-15,-2.88657986402541E-17,
			 3.99680288865056E-16, 2.59792187762287E-16,-1.86517468137026E-16,-5.10702591327572E-17,
			 4.35207425653061E-16,-7.30526750203353E-16, 2.30926389122033E-16, 2.30926389122033E-16,
			 1.39888101102770E-16, 5.92859095149834E-16, 2.77555756156289E-16, 1.21902488103842E-15,
			 2.66453525910038E-16, 2.44249065417534E-17, 2.70894418008538E-16,-9.85878045867139E-16,
			-1.97619698383278E-16, 5.92859095149834E-16,-1.88737914186277E-16,-6.57252030578093E-16,
			-1.26565424807268E-16, 1.77635683940025E-17, 3.29958282918597E-15, 6.66133814775094E-18,
			-1.77635683940025E-16,-5.21804821573824E-16, 3.77475828372553E-17,-6.94999613415348E-16,
			-8.39328606616618E-16,-9.37028232783632E-16, 5.95079541199084E-16,-2.73114864057789E-16,
			-2.04281036531029E-15, 3.77475828372553E-17, 1.64313007644523E-15, 4.21884749357559E-17,
			-5.58442181386454E-16,-4.24105195406810E-16, 6.81676937119846E-16,-1.28785870856518E-15,
			 2.44915199232310E-15,-2.59792187762287E-16, 2.88657986402541E-17,-2.18713935851156E-16,
			-1.63202784619898E-16,-2.93098878501041E-16, 4.67403893367191E-16,-5.77315972805081E-16,
			 1.20847776230448E-15,-2.10942374678780E-17, 1.02307051719208E-15,-6.27831120425526E-16,
			 1.33226762955019E-16,-1.00419672577345E-15, 2.58126853225349E-16,-8.60422844084496E-17,
			 1.13409281965460E-15, 5.49837952945609E-16, 4.15306677936655E-15, 6.03961325396085E-16,
			 6.71684929898220E-16, 1.24275589818978E-15, 5.02653474399040E-16,-1.27627075574566E-15
		};

		public static double Sin5(double ang, int accuracy = 8)
		{
			double abound = ang % Const.Math2PI;
			double a = abound * 2.0 / Math.PI;

			//shift these over
			     if (a < -3.0 || a > 3.0) { a =  a - Math.Sign(a) * 4.0; }
			//reflect these
			else if (a < -1.0 || a > 1.0) { a = -a + Math.Sign(a) * 2.0; }

			double min = -1.0, max = 1.0;

			//InvChebyshev2
			int depth = Math.Clamp(accuracy,3,Sin5Coefs.Length);
			double dip1 = 0.0;
			double di = 0.0;
			double y = (2.0 * a - min - max) / (max - min);
			for(int i=depth-1; 1 <= i; i--) {
				double dip2 = dip1;
				dip1 = di;
				di = 2.0 * y * dip1 - dip2 + Sin5Coefs[i];
			}
			return y * di - dip1 + 0.5 * Sin5Coefs[0];
		}

		//https://en.wikipedia.org/wiki/Taylor_series#Approximation_error_and_convergence
		public static double SinTaylor(double ang)
		{
			double a = ang % Const.Math2PI;
			     if (a < -Math.PI) { a += Const.Math2PI; }
			else if (a >  Math.PI) { a -= Const.Math2PI; }

			double a2 = a * a;
			double a3 = a2 * a;
			double a5 = a3 * a2;
			double a7 = a5 * a2;
			double a9 = a7 * a2;
			double a11 = a9 * a2;
			double s = a - a3/6.0 + a5/120.0 - a7/5040.0 + a9/362880.0 - a11/39916800.0;
			return s;
		}

		// http://www.netlib.org/fdlibm/k_sin.c
		public static double SinFdlibm(double ang)
		{
			// [-pi/4,pi/4]     do nothing
			// [-3pi/4,-pi/4]   x+pi/2
			// [3pi/4,pi/4]     -x-pi/2
			//

			double a = ang % Const.Math2PI;
			//     if (a < -Math.PI) { a += Math2PI; }
			//else if (a >  Math.PI) { a -= Math2PI; }

			//shift these over
			if (a < -3.0*Const.MathPIo2 || a > 3.0*Const.MathPIo2) {
				a =  a - Math.Sign(a) * Const.Math2PI;
			}
			//reflect these
			else if (a < -Const.MathPIo2 || a > Const.MathPIo2) {
				a = -a + Math.Sign(a) * Math.PI;
			}

			double x = a, y = 0, iy = 0;
			//TODO not sure what "Input y is the tail of x." means

			double z,r,v;
			int ix = Helpers.HI(ang);       /* high word of x */
			if (ix < 0x3e400000) {          /* |x| < 2**-27 */
				if((int)x==0) { return x; } /* generate inexact */
			}
			z = x * x;
			v = z * x;
			r = S2 + z * (S3 + z * (S4 + z * (S5 + z * S6)));
			if (iy == 0) { return x + v * (S1 + z * r); }
			else         { return x - ((z * (HF * y - v * r) - y) - v * S1); }
		}

		const double HF  =  5.00000000000000000000e-01; /* 0x3FE00000, 0x00000000 */
		const double S1  = -1.66666666666666324348e-01; /* 0xBFC55555, 0x55555549 */
		const double S2  =  8.33333333332248946124e-03; /* 0x3F811111, 0x1110F8A6 */
		const double S3  = -1.98412698298579493134e-04; /* 0xBF2A01A0, 0x19C161D5 */
		const double S4  =  2.75573137070700676789e-06; /* 0x3EC71DE3, 0x57B1FE7D */
		const double S5  = -2.50507602534068634195e-08; /* 0xBE5AE5E6, 0x8A2B9CEB */
		const double S6  =  1.58969099521155010221e-10; /* 0x3DE5D93A, 0x5ACFD57C */

		// http://math2.org/math/algebra/functions/sincos/expansions.htm
		public static double Sin6(double ang, int accuracy = 8)
		{
			double a = ang % Const.Math2PI;
			double t = a;
			for(int i=1; i<=accuracy; i++) {
				double ipi = a / (i * Math.PI);
				double step = 1 - ipi * ipi;
				t *= step;
			}
			return t;
		}

		//maybe look at
		// https://github.com/JuliaMath/openlibm/blob/master/src/s_sin.c
		// https://github.com/freemint/fdlibm/blob/master/e_atan2.c
	}
}
