using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;

namespace SequenceGen.Generators
{
	//k  = 0 1 2 3 4 5 6 7 8
	//pn =   1 1 1 1 1 1 1 1 ... 1
	//qn = 2 1 2 1 1 4 1 1 6 ... k, k%3==2: 2*(k+1)/3
	public class CofraE2 : AbstractCofra
	{
		public CofraE2() : base()
		{}

		readonly BigInteger ZERO = 0;
		readonly BigInteger ONE = 1;
		readonly BigInteger TWO = 2;
		readonly BigInteger THREE = 3;


		public override BigInteger P(BigInteger k)
		{
			return ONE;
		}

		public override BigInteger Q(BigInteger k)
		{
			if (k == ZERO) { return TWO; }
			if (k % THREE == TWO) {
				return TWO * (k + ONE) / THREE;
			}
			return ONE;
		}

		public override PickForm Form { get {
			return PickForm.StartWithQ;
		}}
	}
}
