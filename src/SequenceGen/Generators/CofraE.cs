using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;

namespace MathVenture.SequenceGen.Generators
{   //k  = 0 1 2 3 4
	//pn =   1 1 2 3 ... k-1
	//qn = 2 1 2 3 4 ... k

	public class CofraE : AbstractCofra
	{
		public CofraE() : base()
		{}

		readonly BigInteger ZERO = 0;
		readonly BigInteger ONE = 1;
		readonly BigInteger TWO = 2;

		public override BigInteger P(BigInteger k)
		{
			//index 0 is skipped
			if (k == ONE) { return ONE; }
			return k - 1;
		}

		public override BigInteger Q(BigInteger k)
		{
			if (k == ZERO) { return TWO; }
			return k;
		}

		public override PickForm Form { get {
			return PickForm.StartWithQ;
		}}
	}
}
