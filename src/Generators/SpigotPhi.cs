using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;

namespace SequenceGen.Generators
{
	// pn =   1 1 1 1 ... 1
	// qn = 1 1 1 1 1 ... 1

	public class SpigPhi : AbstractSpigot
	{
		public SpigPhi() : base()
		{}

		readonly BigInteger ONE = 1;

		public override BigInteger P(BigInteger k)
		{
			return ONE;
		}

		public override BigInteger Q(BigInteger k)
		{
			return ONE;
		}

		public override BigInteger QFirst { get { return ONE; }}
	}
}
