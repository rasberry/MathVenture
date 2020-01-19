using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;

namespace SequenceGen.Generators
{
	//wraps the CofraCore 
	public abstract class AbstractCofra : IGenerator, ICofraConfig, ICanHasBases
	{
		public AbstractCofra()
		{
			Core = new CofraCore(this);
			Reset();
		}

		public Digit Next { get {
			return Core.Next;
		}}

		public void Reset() {
			Core.Reset();
		}

		public int Base {
			get { return Core.Base; }
			set { Core.Base = value; }
		}

		CofraCore Core;

		public abstract BigInteger P(BigInteger k);
		public abstract BigInteger Q(BigInteger k);
		public abstract BigInteger QFirst { get; }
	}
}