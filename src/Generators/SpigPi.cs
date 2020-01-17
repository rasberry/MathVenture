using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;

namespace SequenceGen.Generators
{
	//pn =  4  1  4  9 16 ... k*k
	//qn =  1  3  5  7  9 ... 2*k+1

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

		public override BigInteger QFirst { get { return 0; }}
	}
}
