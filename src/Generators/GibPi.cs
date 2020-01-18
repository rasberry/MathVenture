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
			q=1; r=0; t=1; k=1; n=3; l=3;
		}

		readonly BigInteger ONE = 1;
		readonly BigInteger TWO = 2;
		readonly BigInteger THREE = 3;
		readonly BigInteger FOUR = 4;
		readonly BigInteger SEVEN = 7;
		readonly BigInteger TEN = 10;

		BigInteger q,r,t,k,n,l;

		BigInteger GetNext()
		{
			while(true)
			{
				// Console.WriteLine($"q={q} r={r} t={t} k={k} n={n} l={l} if {4*q+r-t} < {n*t}");
				if (FOUR*q+r-t < n*t) {
					var a = n; //needed a temp var to make order work out
					//Note: replacing 10 with another number changes the base
					n = TEN*(THREE*q+r)/t - (TEN*n); // q r t n
					r = TEN*(r-a*t); //r a t
					q = TEN*q;
					return a;
				}
				else {
					n = (q*(SEVEN*k+TWO)+r*l) / (t*l); // q k r l t
					t = t*l; // t l
					r = (TWO*q+r)*l; // q r l
					l = l+TWO; // l
					q = q*k; //q k
					k = k+ONE; // k
				}
			}
		}
	}
}