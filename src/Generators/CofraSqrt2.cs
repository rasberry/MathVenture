using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;

namespace SequenceGen.Generators
{
	// pn =   1 1 1 1 ... 1
	// qn = 1 2 2 2 2 ... 2

	public class CofraSqrt2 : AbstractCofra
	{
		public CofraSqrt2() : base()
		{}

		readonly BigInteger ONE = 1;
		readonly BigInteger TWO = 2;

		public override BigInteger P(BigInteger k)
		{
			return ONE;
		}

		public override BigInteger Q(BigInteger k)
		{
			return TWO;
		}

		public override BigInteger QFirst { get { return ONE; }}
	}
}

/*

1 + 1/(2 + 1/(2 + 1/(2 + ...)))
1 + 1/2 = 3/2
1 + 1/(2 + 1/2) = 7/5

p0 = 3
q0 = 2
p1 = 1
q1 = 2

a0 = 3
b0 = 2
a1 = 7
b1 = 5

a0 = p0
b0 = q0
a1 = p0*q0
b1 = q0*q1+p1

5 = 2*2+1


!/usr/local/bin/ruby
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



/*

p0 = 3
q0 = 1
p1 = 
q1 = 

a0 = 3
b0 = 1
a1 = 8
b1 = 3

a0 = p0
b0 = q0
a1 = p0*q0
b1 = q0*q1+p1

2+1/1 = 3,1
2+1/(1+1/(2+2)) = 2+1/(5/4) 2 + 4/5 = 9/5


#!/usr/local/bin/ruby
k, a, b, a1, b1 = 2, 3, 1, 8, 3
loop do
p, q, k = k, k+1, k+1
a, b, a1, b1 = a1, b1, p*a+q*a1, p*b+q*b1
d = a / b
d1 = a1 / b1
while d == d1
print d
$stdout.flush
a, a1 = 10*(a%b), 10*(a1%b1)
d, d1 = a/b, a1/b1
end
*/