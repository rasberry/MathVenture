using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;

namespace SequenceGen.Generators
{
	// an =   1 1 1 1 ...
	// bn = 1 2 2 2 2 ...

	public class SpigRubySqrt2 : AbstractSpigot
	{
		public SpigRubySqrt2() : base()
		{}

		public override BigInteger P(BigInteger k)
		{
			return 1;
		}

		public override BigInteger Q(BigInteger k)
		{
			return 2;
		}
	}
}
