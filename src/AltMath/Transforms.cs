using System;

namespace MathVenture.AltMath
{
	public static class Transforms
	{
		// https://rosettacode.org/wiki/Chebyshev_coefficients#Java
		public static double[] ChebyshevTransform1(Func<Double, Double> func, int coefCount = 8, double min = 0,double max = 1)
		{
			var coefs = new double[coefCount];
			int N = coefs.Length;
			for (int i = 0; i < N; i++) {
				double m = rosetta_map(Math.Cos(Math.PI * (i + 0.5) / N), -1, 1, min, max);
				double f = func(m) * 2 / N;

				for (int j = 0; j < N; j++) {
					coefs[j] += f * Math.Cos(Math.PI * j * (i + 0.5) / N);
				}
			}
			return coefs;
		}

		public static double InvChebyshev1(double[] coefs,double val, double min = 0, double max = 1)
		{
			double a = 1, b = rosetta_map(val,min,max,-1,1), c;
			double res = coefs[0] / 2 + coefs[1] * b;
			double xx = 2 * b;
			for(int i = 2; i < coefs.Length; i++) {
				c = xx * b - a;
				res += coefs[i] * c;
				a = b; b = c;
			}
			return res;
		}

		static double rosetta_map(double x, double min_x, double max_x, double min_to,double max_to)
		{
			return (x - min_x) / (max_x - min_x) * (max_to - min_to) + min_to;
		}

		//https://people.sc.fsu.edu/~jburkardt/cpp_src/chebyshev/chebyshev.cpp
		//slightly less accurate than #1
		public static double[] ChebyshevTransform2(Func<Double, Double> func, int coefCount = 8, double min = 0,double max = 1)
		{
			var fx = new double[coefCount];
			for(int i=0; i<coefCount; i++) {
				double angle = (double)(2*i+1) * Math.PI / (double)(2*coefCount);
				double x = Math.Cos(angle);
				x = 0.5 * (min + max) + x * 0.5 * (max - min);
				fx[i] = func(x);
			}

			var c = new double[coefCount];
			for(int i=0; i<coefCount; i++) {
				c[i] = 0.0;
				for(int j=0; j<coefCount; j++) {
					double angle = (double)(i * (2*j+1)) * Math.PI / (double)(2*coefCount);
					c[i] += fx[j] * Math.Cos(angle);
				}
			}

			for(int i=0; i<coefCount; i++) {
				c[i] *= 2.0 / coefCount;
			}

			return c;
		}

		public static double InvChebyshev2(double[] coefs,double val, double min = 0, double max = 1)
		{
			double dip1 = 0.0;
			double di = 0.0;
			double y = (2.0 * val - min - max) / (max - min);
			for(int i=coefs.Length - 1; 1<= i; i--) {
				double dip2 = dip1;
				dip1 = di;
				di = 2.0 * y * dip1 - dip2 + coefs[i];
			}
			return y * di - dip1 + 0.5 * coefs[0];
		}

		//https://github.com/mathnet/mathnet-numerics/blob/4292d6c653aab8b411c60c9579b9bc93f3b9d8e9/src/Numerics/FindRoots.cs
		/// <summary>
		/// Find all roots of the Chebychev polynomial of the first kind.
		/// </summary>
		/// <param name="degree">The polynomial order and therefore the number of roots.</param>
		/// <param name="intervalBegin">The real domain interval begin where to start sampling.</param>
		/// <param name="intervalEnd">The real domain interval end where to stop sampling.</param>
		/// <returns>Samples in [a,b] at (b+a)/2+(b-1)/2*cos(pi*(2i-1)/(2n))</returns>
		public static double[] ChebychevPolynomialFirstKind(int degree, double intervalBegin = -1d, double intervalEnd = 1d)
		{
			if (degree < 1)
			{
				return new double[0];
			}

			// transform to map to [-1..1] interval
			double location = 0.5*(intervalBegin + intervalEnd);
			double scale = 0.5*(intervalEnd - intervalBegin);

			// evaluate first kind chebychev nodes
			double angleFactor = NumericsPi/(2*degree);

			var samples = new double[degree];
			for (int i = 0; i < samples.Length; i++)
			{
				samples[i] = location + scale*Math.Cos(((2*i) + 1)*angleFactor);
			}
			return samples;
		}

		/// <summary>
		/// Find all roots of the Chebychev polynomial of the second kind.
		/// </summary>
		/// <param name="degree">The polynomial order and therefore the number of roots.</param>
		/// <param name="intervalBegin">The real domain interval begin where to start sampling.</param>
		/// <param name="intervalEnd">The real domain interval end where to stop sampling.</param>
		/// <returns>Samples in [a,b] at (b+a)/2+(b-1)/2*cos(pi*i/(n-1))</returns>
		public static double[] ChebychevPolynomialSecondKind(int degree, double intervalBegin = -1d, double intervalEnd = 1d)
		{
			if (degree < 1)
			{
				return new double[0];
			}

			// transform to map to [-1..1] interval
			double location = 0.5*(intervalBegin + intervalEnd);
			double scale = 0.5*(intervalEnd - intervalBegin);

			// evaluate second kind chebychev nodes
			double angleFactor = NumericsPi/(degree + 1);

			var samples = new double[degree];
			for (int i = 0; i < samples.Length; i++)
			{
				samples[i] = location + scale*Math.Cos((i + 1)*angleFactor);
			}
			return samples;
		}

		public static T[] NumericsMap<TA, T>(TA[] points, Func<TA, T> map)
		{
			var res = new T[points.Length];
			for (int i = 0; i < points.Length; i++)
			{
				res[i] = map(points[i]);
			}
			return res;
		}

		const double NumericsPi = 3.1415926535897932384626433832795028841971693993751d;
	}
}
