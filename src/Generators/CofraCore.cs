using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;

namespace SequenceGen.Generators
{
	//Choose one from to use
	//StartWithP
	// p0 / (q0 + (p1 / (q1 + p2 / (q2 + ... ))))
	//StartWithQ
	// q0 + p0 / (q1 + p1 / (q2 + p2 / ( ... )))

	public enum PickForm
	{
		StartWithP,
		StartWithQ
	}

	public interface ICofraConfig
	{
		BigInteger P(BigInteger k);
		BigInteger Q(BigInteger k);
		PickForm Form { get; }
	}

	// http://www.cs.utsa.edu/~wagner/pi/ruby/pi_works.html
	// https://en.wikipedia.org/wiki/Generalized_continued_fraction

	// Continued Fractions spigot engine
	public class CofraCore : IGenerator, ICanHasBases
	{
		public CofraCore(ICofraConfig config)
		{
			this.config = config;
			Reset();
		}

		public Digit Next { get {
			return GetNext();
		}}

		public int Base {
			get { return (int)_base; }
			set { _base = value; }
		}

		// qf + p0 / (q0 + p1 / ( ... ))
		// q0 + p0 / (q1 + p1 / ( ... ))
		public void Reset()
		{
			k = 2;
			state = 0;
			var q0 = config.Q(0);
			var q1 = config.Q(1);
			var p0 = config.P(0);
			var p1 = config.P(1);

			if (config.Form == PickForm.StartWithQ) {
				a = q0;
				b = 1;
				a1 = q0 * q1 + p1;
				b1 = q1;
			}
			else if (config.Form == PickForm.StartWithP) {
				a = p0;
				b = q0;
				a1 = p0 * q1;
				b1 = q0 * q1 + p1;
			}
		}

		BigInteger k,a,b,a1,b1;
		int state = 0;
		ICofraConfig config;
		BigInteger _base = 10;

		// could have built an IEnumerator, but meh..
		Digit GetNext()
		{
			BigInteger d = 0,d1 = 0;
			while(true)
			{
				if (state == 0)
				{
					BigInteger p,q;
					(p, q, k) = (config.P(k), config.Q(k), k+1);
					(a, b, a1, b1) = (a1, b1, p*a+q*a1, p*b+q*b1);
					(d, d1) = (a/b, a1/b1);
					// Console.WriteLine($"k={k} d={d} d1={d1} a={a} b={b} a1={a1} b1={b1}");
					state = 1;
				}
				else if (state == 1)
				{
					if (d == d1) {
						state = 2;
						return new Digit((int)d,(int)_base);
					}
					state = 0;
				}
				else if (state == 2)
				{
					(a, a1) = (_base*(a%b), _base*(a1%b1));
					(d, d1) = (a/b, a1/b1);
					state = 1;
				}
			}
		}
	}
}
