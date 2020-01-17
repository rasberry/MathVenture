using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;

namespace SequenceGen.Generators
{
	//pn =   1 1 2 3 ... k
	//qn = 2 1 2 3 4 ... k+1

	public class SpigE : AbstractSpigot
	{
		public SpigE() : base()
		{}

		public override BigInteger P(BigInteger k)
		{
			if (k == 0) { return 1; }
			return k;
		}

		public override BigInteger Q(BigInteger k)
		{
			return k + 1;
		}

		public override BigInteger QFirst { get { return 2; }}
	}
}
