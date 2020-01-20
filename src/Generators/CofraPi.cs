using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;

namespace SequenceGen.Generators
{
	//k  =  0  1  2  3  4
	//pn =  4  1  4  9 16 ... k*k
	//qn =  1  3  5  7  9 ... 2*k+1

	public class CofraPi : AbstractCofra
	{
		public CofraPi() : base()
		{}

		readonly BigInteger ZERO = 0;
		readonly BigInteger ONE = 1;
		readonly BigInteger TWO = 2;
		readonly BigInteger FOUR = 4;

		public override BigInteger P(BigInteger k)
		{
			if (k == ZERO) { return FOUR; }
			return k * k;
		}

		public override BigInteger Q(BigInteger k)
		{
			return TWO * k + ONE;
		}

		public override PickForm Form { get {
			return PickForm.StartWithP;
		}}
	}
}
