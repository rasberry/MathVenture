using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;

namespace SequenceGen.Generators
{
	// https://www.cs.ox.ac.uk/jeremy.gibbons/publications/spigot.pdf
	// https://rosettacode.org/wiki/Pi
	public class GibPi : IGenerator
	{
		public GibPi() : base()
		{
			Reset();
		}

		public Digit Next { get {
			var next = GetNext();
			return new Digit((int)next);
		}}

		public void Reset()
		{
			q=1;r=0;t=1;k=1;n=3;l=3;
		}

		BigInteger q,r,t,k,n,l;

		BigInteger GetNext()
		{
			while(true)
			{
				// Console.WriteLine($"q={q} r={r} t={t} k={k} n={n} l={l} if {4*q+r-t} < {n*t}");
				if (4*q+r-t < n*t) {
					var a = n; //needed a temp var to make order work out
					//Note: replacing 10 with another number changes the base
					n = 10*(3*q+r)/t - (10*n); // q r t n
					r = 10*(r-a*t); //r a t
					q = 10*q;
					return a;
				}
				else {
					n = (q*(7*k+2)+r*l) / (t*l); // q k r l t
					t = t*l; // t l
					r = (2*q+r)*l; // q r l
					l = l+2; // l
					q = q*k; //q k
					k = k+1; // k
				}
			}
		}
	}
}