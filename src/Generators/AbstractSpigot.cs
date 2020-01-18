using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;

namespace SequenceGen.Generators
{
	public abstract class AbstractSpigot : IGenerator, ISpigotConfig
	{
		public AbstractSpigot()
		{
			Core = new CofraCore(this);
			Reset();
		}

		public Digit Next { get {
			return GetNext();
		}}

		public void Reset() {
			Core.Reset();
		}

		CofraCore Core;

		Digit GetNext() {
			return Core.Next;
		}

		public abstract BigInteger P(BigInteger k);
		public abstract BigInteger Q(BigInteger k);
		public abstract BigInteger QFirst { get; }
	}
}