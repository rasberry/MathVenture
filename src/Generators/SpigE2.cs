using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;

namespace SequenceGen.Generators
{
	//pn =   1 1 1 1 1 1 1 1 ... k
	//qn = 2 1 2 1 1 4 1 1 6 ... k, k%3==2: 2*(k+1)/3

	public class SpigE2 : AbstractSpigot
	{
		public SpigE2() : base()
		{}

		public override BigInteger P(BigInteger k)
		{
			return 1;
		}

		public override BigInteger Q(BigInteger k)
		{
			if (k%3==1) { return 2*(k+2)/3; }
			return 1;
		}

		public override BigInteger QFirst { get { return 2; }}
	}
}
