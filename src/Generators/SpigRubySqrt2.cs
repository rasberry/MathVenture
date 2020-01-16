using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;

namespace SequenceGen.Generators
{
	// an =   1 1 1 1 ...
	// bn = 1 2 2 2 2 ...

	public class SpigRubySqrt2 : AbstractSpigot
	{
		public SpigRubySqrt2() : base()
		{}

		public override BigInteger P(BigInteger k)
		{
			return 1;
		}

		public override BigInteger Q(BigInteger k)
		{
			return 2;
		}
	}
}

/*
#!/usr/local/bin/ruby
k, a, b, a1, b1 = 2, 3, 2, 7, 5
loop do
  p, q, k = 1, 2, k+1
  a, b, a1, b1 = a1, b1, p*a+q*a1, p*b+q*b1
  d = a / b
  d1 = a1 / b1
  while d == d1
    print d
    $stdout.flush
    a, a1 = 10*(a%b), 10*(a1%b1)
    d, d1 = a/b, a1/b1
  end
end
*/