using System;

namespace MathVenture.AltMath
{
	public static class SpectrumRom
	{
		// http://freestuff.grok.co.uk/rom-dis/page222.txt
		static double Generator(double[] A,double Z,int BREG)
		{
			double M0 = 2.0 * Z;
			double M2 = 0.0;
			double T = 0.0;
			double M1 = 0.0;
			for(int I=BREG; I >= 1; I--) {
				M1 = M2;
				double U = T * M0 - M2 + A[BREG - I];
				M2 = T;
				T = U;
			}
			T = T - M1;
			return T;
		}

		// http://freestuff.grok.co.uk/rom-dis/page223.txt
		static double[] Sin3Coefs = new double[] {
			-.000000003,
			0.000000592,
			-.000068294,
			0.004559008,
			-.142630785,
			1.276278962
		};

		public static double Sin(double ang)
		{
			ang %= Const.Math2PI;
			double C = ang * 180.0 / Math.PI;

			double Y = C / 360.0 - Math.Floor(C / 360.0 + 0.5);
			double W = 4.0 * Y;
			if (W > 1.0) { W = 2.0 - W; }
			if (W < -1.0) { W = -W - 2.0; }
			double Z = 2.0 * W * W - 1.0;
			int BREG = 6;
			double T = Generator(Sin3Coefs,Z,BREG);
			double deg = T * W;

			return deg * Math.PI / 180.0;
		}
	}
}