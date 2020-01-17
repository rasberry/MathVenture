using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;

namespace SequenceGen.Generators
{
	//pn =  4  1  9 25 49 ... (2*(k-1)+1)^2
	//qn =  1  2  2  2  2 ... 2

	public class SpigPi2 : AbstractSpigot
	{
		public SpigPi2() : base()
		{}

		//TODO this one locks up .. ?
		public override BigInteger P(BigInteger k)
		{
			if (k == 0) { return 4; }
			var one = 2*(k-1)+1;
			return one * one;
		}

		public override BigInteger Q(BigInteger k)
		{
			if (k == 0) { return 1; }
			return 2;
		}

		public override BigInteger QFirst { get { return 0; }}
	}
}
