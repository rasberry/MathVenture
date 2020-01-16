using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;

namespace SequenceGen.Generators
{
	// http://www.cs.utsa.edu/~wagner/pi/ruby/pi_works.html

	public class SpigotCore : IGenerator
	{
		public SpigotCore(ISpigotConfig config)
		{
			this.config = config;
			Reset();
		}

		public Digit Next { get {
			return GetNext();
		}}

		public void Reset()
		{
			k = 2;
			state = 0;
			var p0 = a = config.P(0);
			var q0 = b = config.Q(0);
			var p1 = config.P(1);
			var q1 = config.Q(1);
			a1 = p0 * q1;
			b1 = q0 * q1 + p1;
		}

		BigInteger k,a,b,a1,b1;
		int state = 0;
		ISpigotConfig config;

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
					state = 1;
				}
				else if (state == 1)
				{
					if (d == d1) {
						state = 2;
						return new Digit((int)d);
					}
					state = 0;
				}
				else if (state == 2)
				{
					(a, a1) = (10*(a%b), 10*(a1%b1));
					(d, d1) = (a/b, a1/b1);
					state = 1;
				}
			}
		}
	}

	//Generate the continued fraction parts
	// p0 / (q0 + (p1 / (q1 + p2 / (q2 + ... ))))
	// k = 0,1,2 ...
	public interface ISpigotConfig
	{
		BigInteger P(BigInteger k);
		BigInteger Q(BigInteger k);
	}
}
