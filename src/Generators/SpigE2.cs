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

		readonly BigInteger ONE = 1;
		readonly BigInteger TWO = 2;
		readonly BigInteger THREE = 3;


		public override BigInteger P(BigInteger k)
		{
			return ONE;
		}

		public override BigInteger Q(BigInteger k)
		{
			if (k % THREE == ONE) {
				return TWO * (k + TWO) / THREE;
			}
			return ONE;
		}

		public override BigInteger QFirst { get { return TWO; }}
	}
}
