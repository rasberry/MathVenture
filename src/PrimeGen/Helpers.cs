using System;
using System.Numerics;

namespace MathVenture.PrimeGen
{
	public static class Helpers
	{
		public static bool EndsWithIC(this string subj,string check)
		{
			if (subj == null) { return false; }
			return subj.EndsWith(check,StringComparison.OrdinalIgnoreCase);
		}

		public static BigInteger Log2(this BigInteger number)
		{
			BigInteger count = 0;
			while(number > 1) {
				number >>= 1;
				count++;
			}
			return count;
		}
	}

	public struct SizeL
	{
		public SizeL(long w,long h) {
			Width = w;
			Height = h;
		}
		public long Width;
		public long Height;
	}
}