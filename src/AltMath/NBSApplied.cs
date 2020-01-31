using System;

namespace MathVenture.AltMath
{
	// https://www.ams.org/journals/mcom/1954-08-047/S0025-5718-1954-0063487-2/S0025-5718-1954-0063487-2.pdf
	// Tables of the Chebyshev Polynomials Sn(x) and Cn{x). NBS Applied Math, series 9, 1952.

	public static class NBSApplied
	{
		public static double Sin(double ang)
		{
			double abound = ang % Const.Math2PI;

			//turns out sin(pi/2*x) [-1,1] is the same as sin(x) from [-pi/2,pi/2]
			//so map [-pi/2,pi/2] to [-1,1]
			double a = abound * 2.0 / Math.PI;

			//shift these over
			     if (a < -3 || a > 3) { a =  a - Math.Sign(a) * 4; }
			//reflect these
			else if (a < -1 || a > 1) { a = -a + Math.Sign(a) * 2; }

			return SinRaw(a);
		}

		public static double Cos(double ang)
		{
			double abound = ang % Const.Math2PI;

			//turns out cos(pi/2*x) [-1,1] is the same as cos(x) from [-pi/2,pi/2]
			//so map [-pi/2,pi/2] to [-1,1]
			double a = abound * 2.0 / Math.PI;
			bool negate = false;

			//shift these over
			if (a < -3 || a > 3) {
				a = a - Math.Sign(a) * 4;
			}
			//reflect these
			else if (a < -1 || a > 1) {
				a = a - Math.Sign(a) * 2;
				negate = true;
			}

			return (negate ? -1 : 1) * CosRaw(a);
		}

		public static double Atan(double r)
		{
			if (r < -1 || r > 1) {
				return Math.Sign(r) * Const.MathPIo2 - AtanRaw(1.0/r);
			}

			return AtanRaw(r);
		}

		public static double Asin(double r)
		{
			if (r < -Const.Sqrt2o2 || r > 1.0) {
				throw new ArgumentOutOfRangeException();
			}
			if (r > Const.Sqrt2o2) {
				return Math.Sqrt(Acos(1 - r*r));
			}
			return AsinRaw(r);
		}

		public static double Acos(double r)
		{
			if (r < 0.0 || r > 1.0) {
				throw new ArgumentOutOfRangeException();
			}
			if (r > Const.Sqrt2o2) {
				return Math.Sqrt(Asin(1 - r*r));
			}
			return Const.MathPIo2 - Asin(r);
		}

		public static double Exp(double x)
		{
			if (x < -1.0 || x > 1.0) {
				throw new ArgumentOutOfRangeException();
			}
			return x < 0.0
				? ExpNegRaw(-x)
				: ExpPosRaw(x)
			;
		}

		static double SinRaw(double a)
		{
			if (a < -1 || a > 1) {
				throw new ArgumentOutOfRangeException();
			}

			//sin(pi/2*x) = x*sum(n=0,inf,An*Tn(x^2)) [-1 <= x <= 1]
			//A0 =  1.276278962
			//A1 = -0.285261569
			//A2 =  0.009118016
			//A3 = -0.000136587
			//A4 =  0.000001185
			//A5 = -0.000000007

			double a2 = a * a;
			double a4 = a2 * a2;
			double a6 = a4 * a2;
			double a8 = a4 * a4;
			double a10 = a6 * a4;

			double sum =
				-0.000003584 * a10
				+0.000160640 * a8
				-0.004681984 * a6
				+0.079692704 * a4
				-0.645964102 * a2
				+1.570796326
			;
			return a * sum;
		}

		static double CosRaw(double a)
		{
			if (a < -1 || a > 1) {
				throw new ArgumentOutOfRangeException();
			}

			//cos(pi/2*x) = sum(n=0,inf,An*Tn(x^2)) [-1 <= x <= 1]
			//A0 =  0.472001216
			//A1 = -0.499403258
			//A2 =  0.027992080
			//A3 = -0.000596695
			//A4 =  0.000006704
			//A5 = -0.000000047

			double a2 = a * a;
			double a4 = a2 * a2;
			double a6 = a4 * a2;
			double a8 = a4 * a4;
			double a10 = a6 * a4;

			double sum =
				-0.000024064 * a10
				+0.000918272 * a8
				-0.020863104 * a6
				+0.253669440 * a4
				-1.233700544 * a2
				+1.0
			;
			return sum;
		}

		static double AtanRaw(double r)
		{
			if (r < -1 || r > 1) {
				throw new ArgumentOutOfRangeException();
			}

			//arctan(x) = x*sum(n=0,inf,An*Tn(x^2)) [-1 <= x <= 1]
			//A0  =  0.881373587
			//A1  = -0.105892925
			//A2  =  0.011135843
			//A3  = -0.001381195
			//A4  =  0.000185743
			//A5  = -0.000026215
			//A6  =  0.000003821
			//A7  = -0.000000570
			//A8  =  0.000000086
			//A9  = -0.000000013
			//A10 =  0.000000002

			double r2 = r * r;
			double r4 = r2 * r2;
			double r6 = r4 * r2;
			double r8 = r4 * r4;
			double r10 = r6 * r4;
			double r12 = r6 * r6;
			double r14 = r8 * r6;
			double r16 = r8 * r8;
			double r18 = r10 * r8;
			double r20 = r10 * r10;

			double sum =
				+0.001048576 * r20
				-0.006946816 * r18
				+0.021626880 * r16
				-0.043425792 * r14
				+0.066340864 * r12
				-0.087535616 * r10
				+0.110391424 * r8
				-0.142761152 * r6
				+0.199992912 * r4
				-0.333333116 * r2
				+1.0
			;
			return sum * r;
		}

		static double AsinRaw(double r)
		{
			if (r < -Const.Sqrt2o2 || r > Const.Sqrt2o2) {
				throw new ArgumentOutOfRangeException();
			}

			//arcsin(x) = x*sum(n=0,inf,An*Tn(2*x^2) [-sqrt(2)/2 <= x <= sqrt(2)/2]
			//A0 = 1.051231959
			//A1 = 0.054946487
			//A2 = 0.004080631
			//A3 = 0.000407890
			//A4 = 0.000046985
			//A5 = 0.000005881
			//A6 = 0.000000777
			//A7 = 0.000000107
			//A8 = 0.000000015
			//A9 = 0.000000002

			double r2 = r * r;
			double r4 = r2 * r2;
			double r6 = r4 * r2;
			double r8 = r4 * r4;
			double r10 = r6 * r4;
			double r12 = r6 * r6;
			double r14 = r8 * r6;
			double r16 = r8 * r8;
			double r18 = r10 * r8;

			double sum =
				+0.134217728 * r18
				-0.176160768 * r16
				+0.143654912 * r14
				-0.033161216 * r12
				+0.034242560 * r10
				+0.028669952 * r8
				+0.044792576 * r6
				+0.074992448 * r4
				+0.166666844 * r2
				+1.0
			;
			return r * sum;
		}

		static double ExpPosRaw(double x)
		{
			if (x < 0.0 || x > 1.0) {
				throw new ArgumentOutOfRangeException();
			}

			//e^x = sum(n=0,inf,An*Tn(x)) [0 <= x <= 1]

			//A0 = 1.753387654
			//A1 = 0.850391654
			//A2 = 0.105208694
			//A3 = 0.008722105
			//A4 = 0.000543437
			//A5 = 0.000027115
			//A6 = 0.000001128
			//A7 = 0.000000040
			//A8 = 0.000000001

			double x2 = x * x;
			double x3 = x2 * x;
			double x4 = x2 * x2;
			double x5 = x3 * x2;
			double x6 = x3 * x3;
			double x7 = x4 * x3;
			double x8 = x4 * x4;

			double sum =
				+0.000032768 * x8
				+0.000196608 * x7
				+0.001376256 * x6
				+0.008349184 * x5
				+0.041658752 * x4
				+0.166668352 * x3
				+0.499999920 * x2
				+0.999999988 * x
				+1.0
			;
			return sum;
		}

		static double ExpNegRaw(double x)
		{
			if (x < 0.0 || x > 1.0) {
				throw new ArgumentOutOfRangeException();
			}

			//e^-x = sum(n=0,inf,An*Tn(x)) [0 <= x <= 1]
			//A0 =  0.645035270
			//A1 = -0.312841606
			//A2 =  0.038704116
			//A3 = -0.003208683
			//A4 =  0.000199919
			//A5 = -0.000009975
			//A6 =  0.000000415
			//A7 = -0.000000015

			double x2 = x * x;
			double x3 = x2 * x;
			double x4 = x2 * x2;
			double x5 = x3 * x2;
			double x6 = x3 * x3;
			double x7 = x4 * x3;

			double sum =
				-0.000122880 * x7
				+0.001280000 * x6
				-0.008248320 * x5
				+0.041629312 * x4
				-0.166657600 * x3
				+0.499998872 * x2
				-0.999999942 * x
				+1.0
			;
			return sum;
		}

		static double LogP1Raw(double x)
		{
			if (x < 0.0 || x > 1.0) {
				throw new ArgumentOutOfRangeException();
			}

			// log(1+x) = sum(n=0,inf,An*Tn(x)) [0 <= x <= 1]
			//A0  =  0.031540613
			//A1  = -0.214616183
			//A2  =  0.004336620
			//A3  = -0.266203654
			//A4  =  0.306125520
			//A5  = -0.136388770
			//A6  =  0.034347540
			//A7  = -0.005698082
			//A8  =  0.000677504
			//A9  = -0.000060947
			//A10 =  0.000004309
			//A11 = -0.000000246
			//A12 =  0.000000012

			double x2 = x * x;
			double x3 = x2 * x;
			double x4 = x2 * x2;
			double x5 = x3 * x2;
			double x6 = x3 * x3;
			double x7 = x4 * x3;
			double x8 = x4 * x4;
			double x9 = x5 * x4;
			double x10 = x5 * x5;
			double x11 = x6 * x5;
			double x12 = x6 * x6;

			double sum =
				+ 0.1006632960 * x12
				- 1.1198791680 * x11
				+ 6.6820503140 * x10
				- 28.413919232 * x9
				+ 93.482516480 * x8
				-240.103317504 * x7
				+470.892816384 * x6
				-678.155865600 * x5
				+678.166631936 * x4
				-434.027625984 * x3
				+156.249992936 * x2
				- 24.999999868 * x
				+ 1.0
			;
			return sum;
		}

		static double GammaP1(double x)
		{
			if (x < 0.0 || x > 1.0) {
				throw new ArgumentOutOfRangeException();
			}
			// gamma(1+x) = sum(n=0,inf,An*Tn(x)) [0 <= x <= 1]
			// A0  =  0.941785598
			// A1  =  0.004415381
			// A2  =  0.056850437
			// A3  = -0.004219835
			// A4  =  0.001326808
			// A5  = -0.000189303
			// A6  =  0.000036069
			// A7  = -0.000006057
			// A8  =  0.000001056
			// A9  = -0.000000181
			// A10 =  0.000000031
			// A11 = -0.000000005
			// A12 =  0.000000001

			double x2 = x * x;
			double x3 = x2 * x;
			double x4 = x2 * x2;
			double x5 = x3 * x2;
			double x6 = x3 * x3;
			double x7 = x4 * x3;
			double x8 = x4 * x4;
			double x9 = x5 * x4;
			double x10 = x5 * x5;
			double x11 = x6 * x5;
			double x12 = x6 * x6;

			double sum =
				+0.008388608 * x12
				-0.060817408 * x11
				+0.206045179 * x10
				-0.441188352 * x9
				+0.688390144 * x8
				-0.864878592 * x7
				+0.951980032 * x6
				-0.972704768 * x5
				+0.980290688 * x4
				-0.907338592 * x3
				+0.989048568 * x2
				-0.577215512 * x
				+1.0
			;
			return sum;
		}

		static double BesselFirstJ0(double x)
		{
			if (x < -10.0 || x > 10.0) {
				throw new ArgumentOutOfRangeException();
			}
			// J0(x) = sum(n=0,inf,An*Tn(x^2/100)) [-10 <= x <= 10]
			//A0  =  0.031540613
			//A1  = -0.214616183
			//A2  =  0.004336620
			//A3  = -0.266203654
			//A4  =  0.306125520
			//A5  = -0.136388770
			//A6  =  0.034347540
			//A7  = -0.005698082
			//A8  =  0.000677504
			//A9  = -0.000060947
			//A10 =  0.000004309
			//A11 = -0.000000246
			//A12 =  0.000000012

			double x2 = x * x;
			double x4 = x2 * x2;
			double x6 = x4 * x2;
			double x8 = x4 * x4;
			double x10 = x6 * x4;
			double x12 = x6 * x6;
			double x14 = x8 * x6;
			double x16 = x8 * x8;
			double x18 = x10 * x8;
			double x20 = x10 * x10;
			double x22 = x12 * x10;
			double x24 = x12 * x12;

			double sum =
				+1.00663296000E-25 * x24
				-1.11987916800E-22 * x22
				+6.68205031400E-20 * x20
				-2.84139192320E-17 * x18
				+9.34825164800E-15 * x16
				-2.40103317504E-12 * x14
				+4.70892816384E-10 * x12
				-6.78155865600E-8  * x10
				+6.78166631936E-6  * x8
				-4.34027625984E-4  * x6
				+0.01562499929360  * x4
				-0.24999999868000  * x2
				+1.0
			;
			return sum;
		}

		static double BesselFirstJ1(double x)
		{
			if (x < -10.0 || x > 10.0) {
				throw new ArgumentOutOfRangeException();
			}
			// J1(x) = x*sum(n=0,inf,An*Tn(x^2/100)) [-10 <= x <= 10]
			//A0  =  0.0694243523
			//A1  = -0.1155779057
			//A2  =  0.1216794099
			//A3  = -0.1148840465
			//A4  =  0.0577905331
			//A5  = -0.0169238801
			//A6  =  0.0032350252
			//A7  = -0.0004370609
			//A8  =  0.0000440991
			//A9  = -0.0000034583
			//A10 =  0.0000002172
			//A11 = -0.0000000112
			//A12 =  0.0000000005

			double x2 = x * x;
			double x4 = x2 * x2;
			double x6 = x4 * x2;
			double x8 = x4 * x4;
			double x10 = x6 * x4;
			double x12 = x6 * x6;
			double x14 = x8 * x6;
			double x16 = x8 * x8;
			double x18 = x10 * x8;
			double x20 = x10 * x10;
			double x22 = x12 * x10;
			double x24 = x12 * x12;

			double sum =
				+4.19430400000E-27 * x24
				-4.86539264000E-24 * x22
				+3.09120193600E-21 * x20
				-1.42909112320E-18 * x18
				+5.20178565120E-16 * x16
				-1.50119137280E-13 * x14
				+3.36376045568E-11 * x12
				-5.65136932352E-9  * x10
				+6.78167970048E-7  * x8
				-5.42534689792E-5  * x6
				+0.00260416665432  * x4
				-0.06249999998200  * x2
				+0.5
			;
			return x * sum;
		}
	}
}

//T0 : 1
//T1 : (2*r1 -1)
//T2 : (8*r2 -8*r1 +1)
//T3 : (32*r3 -48*r2 +18*r1 -1)
//T4 : (128*r4 -256*r3 +160*r2 -32*r1 +1)
//T5 : (512*r5 -1280*r4 +1120*r3 -400*r2 +50*r1 -1)
//T6 : (2048*r6 -6144*r5 +6912*r4 -3584*r3 +840*r2 -72*r1 +1)
//T7 : (8192*r7 -28672*r6 +39424*r5 -26880*r4 +9408*r3 -1568*r2 +98*r1 -1)
//T8 : (32768*r8 -131072*r7 +212992*r6 -180224*r5 +84480*r4 -21504*r3 +2688*r2 -128*r1 +1)
//T9 : (131072*r9 -589824*r8 +1105920*r7 -1118208*r6 +658944*r5 -228096*r4 +44352*r3 -4320*r2 +162*r1 -1)
//T10: (524288*r10 -2621440*r9 +5570560*r8 -6553600*r7 +4659200*r6 -2050048*r5 +549120*r4 -84480*r3 +6600*r2 -200*r1 +1)
//T11: (2097152*r11 -11534335*r10 +27394048*r9 -36765696*r8 +30638080*r7 -16400384*r6 +5637632*r5 -1208064*r4 +151008*r3 -9680*r2 +242*r1 -1)
//T12: (8388608*r12 -50331648*r11 +132120576*r10 -199229440*r9 +190513152*r8 -120324096*r7 +50692096*r6 -14057472*r5 +2471040*r4 -256256*r3 +13728*r2 -288*r1 +1)

//recurence relation
//Tn+1(x) = 2*(2*x-1)*Tn(x) - Tn-1(x)
