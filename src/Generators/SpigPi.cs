using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;

namespace SequenceGen.Generators
{
	//an =   4 1^2 2^2 3^2 4^2 ...
	//bn = 0 1 3   5   7   9 ...

	public class SpigPi : AbstractSpigot
	{
		public SpigPi() : base()
		{}

		public override BigInteger P(BigInteger k)
		{
			if (k == 0) { return 4; }
			return k*k;
		}

		public override BigInteger Q(BigInteger k)
		{
			return 2 * k + 1;
		}
	}
}
