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

		readonly BigInteger ONE = 1;
		readonly BigInteger TWO = 2;
		readonly BigInteger THREE = 3;
		readonly BigInteger FIVE = 5;
		readonly BigInteger TEN = 10;
		readonly BigInteger TWELVE = 12;
		readonly BigInteger TWENTYSEVEN = 27;

		BigInteger q,r,t,i;

		//Note: since this version doesn't have the check step (I think) it
		// only supports bases below 10
		BigInteger GetNext()
		{
			BigInteger u,y;
			u = THREE * (THREE * i + ONE) * (THREE * i + TWO); //i
			y = (q * (TWENTYSEVEN * i - TWELVE) + FIVE * r) / (FIVE * t); //q i r t

			r = TEN * u * (q * (FIVE * i - TWO) + r - y * t); // u q i r y t
			q = TEN * q * i * (TWO * i - ONE); // q i
			t = t * u; // t u
			i = i + ONE; // i

			return y;
		}
	}
}