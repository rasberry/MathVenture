using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;

namespace SequenceGen.Generators
{
	//pn =   1 1 2 3 ... k
	//qn = 2 1 2 3 4 ... k+1

	public class CofraE : AbstractCofra
	{
		public CofraE() : base()
		{}

		readonly BigInteger ZERO = 0;
		readonly BigInteger ONE = 1;
		readonly BigInteger TWO = 2;

		public override BigInteger P(BigInteger k)
		{
			if (k == ZERO) { return ONE; }
			return k;
		}

		public override BigInteger Q(BigInteger k)
		{
			return k + ONE;
		}

		public override BigInteger QFirst { get { return TWO; }}
	}
}
