using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;

namespace SequenceGen.Generators
{
	public class ConstPi : IGenerator
	{
		public ConstPi()
		{
			Reset();
		}

		public Digit Next { get {
			BigInteger n = GetNext();
			return (Digit)(int)n;
		}}

		public void Reset()
		{
			k = BigInteger.One;
			l = new BigInteger(3);
			n = new BigInteger(3);
			q = BigInteger.One;
			r = BigInteger.Zero;
			t = BigInteger.One;
		}

		readonly BigInteger FOUR = new BigInteger(4);
		readonly BigInteger SEVEN = new BigInteger(7);
		readonly BigInteger TEN = new BigInteger(10);
		readonly BigInteger THREE = new BigInteger(3);
		readonly BigInteger TWO = new BigInteger(2);
 
		BigInteger k,l,n,q,r,t;

		// https://www.cs.ox.ac.uk/jeremy.gibbons/publications/spigot.pdf
		// https://rosettacode.org/wiki/Pi
		BigInteger GetNext()
		{
			while(true) {
				if ((FOUR*q + r - t).CompareTo(n*t) == -1) {
					var a = n;
					//Note: replacing TEN with another number changes the base
					n = TEN*(THREE*q + r)/t - (TEN*n);
					r = TEN*(r - (a*t));
					q *= TEN;
					return a;
				}
				else {
					n = (q*(SEVEN*k) + TWO + r*l)/(t*l);
					r = (TWO*q + r)*l;
					q *= k;
					t *= l;
					l += TWO;
					k += BigInteger.One;
				}
			}
		}
	}
}
