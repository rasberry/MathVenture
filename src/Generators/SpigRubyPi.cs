
using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;

namespace SequenceGen.Generators
{
	//an =   4 1^2 2^2 3^2 4^2 ...
	//bn = 0 1 3   5   7   9 ...

	public class SpigRubyPi : IGenerator
	{
		public SpigRubyPi()
		{
			Reset();
		}

		public Digit Next { get {
			return GetNext();
		}}

		public void Reset()
		{
			// a/b = 4/1
			// a1/b1 = 4/(1+1/3) = 4/(4/3) = 4*3/4 = 12/4
			(k,a,b,a1,b1) = (2,4,1,12,4);
			// a1 = 4 * 0 + 1 * 
		}

		BigInteger k,a,b,a1,b1;
		int state = 0;

		Digit GetNext()
		{
			BigInteger d = 0,d1 = 0;
			while(true)
			{
				if (state == 0)
				{
					BigInteger p,q;
					(p, q, k) = (k*k, 2*k+1, k+1);
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
}


/*
#!/usr/local/bin/ruby
k, a, b, a1, b1 = 2, 4, 1, 12, 4
loop do
  p, q, k = k*k, 2*k+1, k+1
  a, b, a1, b1 = a1, b1, p*a+q*a1, p*b+q*b1
  d, d1 = a/b, a1/b1
  while d == d1
    print d
    $stdout.flush
    a, a1 = 10*(a%b), 10*(a1%b1)
    d, d1 = a/b, a1/b1
  end
end
*/