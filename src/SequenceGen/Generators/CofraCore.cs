using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;

namespace MathVenture.SequenceGen.Generators
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
			this.Config = config;
			Reset();
		}

		public Digit Next { get {
			DigitGen.MoveNext();
			return DigitGen.Current;
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
			//State = 0;
			var q0 = Config.Q(0);
			var q1 = Config.Q(1);
			var p0 = Config.P(0);
			var p1 = Config.P(1);

			if (Config.Form == PickForm.StartWithQ) {
				a = q0;
				b = 1;
				a1 = q0 * q1 + p1;
				b1 = q1;
			}
			else if (Config.Form == PickForm.StartWithP) {
				a = p0;
				b = q0;
				a1 = p0 * q1;
				b1 = q0 * q1 + p1;
			}
			DigitGen = GetDigits().GetEnumerator();
		}

		BigInteger k,a,b,a1,b1;
		ICofraConfig Config;
		BigInteger _base = 10;
		IEnumerator<Digit> DigitGen = null;

		IEnumerable<Digit> GetDigits()
		{
			BigInteger d = 0,d1 = 0;
			while(true)
			{
				BigInteger p,q;
				(p, q, k) = (Config.P(k), Config.Q(k), k+1);
				(a, b, a1, b1) = (a1, b1, p*a+q*a1, p*b+q*b1);
				(d, d1) = (a/b, a1/b1);

				while (d == d1) {
					//the initial ratio (k:0,1) may be outside the range of the selected base
					// so need to convert it to a stream of in-base digits
					if (d >= _base) {
						var wDigits = Helpers.ConvertBase((int)d,(int)_base);
						foreach(var wd in wDigits) {
							yield return wd;
						}
					}
					else {
						yield return new Digit((int)d,(int)_base);
					}
					(a, a1) = (_base*(a%b), _base*(a1%b1));
					(d, d1) = (a/b, a1/b1);
				}
			}
		}
	}
}
