using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;

namespace SequenceGen.Generators
{
	// https://www.cs.ox.ac.uk/jeremy.gibbons/publications/spigot.pdf
	public class GibPi2 : IGenerator
	{
		public GibPi2() : base()
		{
			Reset();
		}

		public Digit Next { get {
			var next = GetNext();
			return new Digit((int)next);
		}}

		public void Reset()
		{
			q = 1; r = 180; t = 60; i = 2;
		}

		BigInteger q,r,t,i;

		BigInteger GetNext()
		{
			BigInteger u,y;
			u = 3*(3*i+1)*(3*i+2); //i
			y = (q*(27*i-12)+5*r) / (5*t); //q i r t

			r = 10*u*(q*(5*i-2)+r-y*t); // u q i r y t
			q = 10*q*i*(2*i-1); // q i
			t = t*u; // t u
			i = i+1; // i

			return y;
		}
	}
}