using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;

namespace SequenceGen.Generators
{
	//an =   4 1^2 2^2 3^2 4^2 ...
	//bn = 0 1 3   5   7   9 ...

	public class SpigPi : AbstractSpigot
	{
		public SpigPi() : base()
		{}

		public override BigInteger P(BigInteger k)
		{
			if (k == 0) { return 4; }
			return k*k;
		}

		public override BigInteger Q(BigInteger k)
		{
			return 2 * k + 1;
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