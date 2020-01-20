using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;

namespace SequenceGen.Generators
{
	// k  = 1 2 3 4 5
	// pn = 1 1 1 1 1 ... 1
	// qn = 1 1 1 1 1 ... 1

	public class CofraPhi : AbstractCofra
	{
		public CofraPhi() : base()
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

		public override PickForm Form { get {
			return PickForm.StartWithQ;
		}}
	}
}
