using System;

namespace MathVenture.AltMath
{
	public static class Helpers
	{
		// return upper bits of a double
		public static int HI(double a) {
			long d = BitConverter.DoubleToInt64Bits(a);
			long mask = (long)uint.MaxValue;
			return (int)(d & mask);
		}

		public static int LO(double a) {
			long d = BitConverter.DoubleToInt64Bits(a);
			long mask = ~((long)uint.MaxValue);
			return (int)((d & mask) >> 32);
		}
	}
}
