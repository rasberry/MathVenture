using System;
using System.Numerics;

namespace MathVenture.PrimeGen
{
	public class PrimeSundaram : IPrimeSource
	{
		public BigInteger NextPrime(BigInteger number)
		{
			if (number <= 2) {
				return 2;
			}

			// n0 <= x+y+2xy <= n1
			// n0-x <= y(1+2x)
			// y >= (n0-x)/(2x+1)

			BigInteger max = (number - 1) / 3;
			for(BigInteger i=1; i<=max; i++) {
				BigInteger jmax = (number - i) / (2*i+1);
				for(BigInteger j=1; j<=jmax; j++) {
					// TODO
				}
			}

			return BigInteger.Zero;
		}
	}
}

/*
max = (n-1)/3

y>=(1000-1)/(2*1+1) = 999/3
y>=(1000-2)/(2*2+1) = 998/5
y>=(1000-3)/(2*3+1) = 997/7
...
y>=(1000-333)/(2*333+1) = 667/667

n0-x = 2x+1
n-x=2x+1
n=2x+1+x
n=3x+1
1000-333 = 2*333+1

n=100
01:1-33
02:1-20 (100-02)/(2*02+1) = 98/5
03:1-14 (100-03)/(2*03+1) = 97/7
04:1-11 (100-04)/(2*04+1) = 96/9
05:1-09 (100-05)/(2*05+1) = 95/11
06:1-08 (100-06)/(2*06+1) = 94/13
07:1-07 (100-07)/(2*07+1) = 93/15
08:1-06 (100-08)/(2*08+1) = 92/17
09:1-05 (100-09)/(2*09+1) = 91/19
10:1-05 (100-10)/(2*10+1) = 90/21
11:1-04 (100-11)/(2*11+1) = 89/23
12:1-04 (100-12)/(2*12+1) = 88/25
13:1-04 (100-13)/(2*13+1) = 87/27
14:1-03 (100-14)/(2*14+1) = 86/29
15:1-03 (100-15)/(2*15+1) = 85/31
16:1-03 (100-16)/(2*16+1) = 84/33
17:1-03 (100-17)/(2*17+1) = 83/35
18:1-03 (100-18)/(2*18+1) = 82/37
19:1-03 (100-19)/(2*19+1) = 81/39
20:1-02 (100-20)/(2*20+1) = 80/41
21:1-02 (100-21)/(2*20+1) = 79/43
22:1-02 (100-22)/(2*20+1) = 78/45
23:1-02 (100-23)/(2*20+1) = 77/47
24:1-02 (100-24)/(2*20+1) = 76/49
25:1-02 (100-25)/(2*20+1) = 75/51
26:1-02 (100-26)/(2*20+1) = 74/53
27:1-02 (100-27)/(2*20+1) = 73/55
28:1-02 (100-28)/(2*20+1) = 72/57
29:1-02 (100-29)/(2*20+1) = 71/59
30:1-02 (100-30)/(2*20+1) = 70/61
31:1-02 (100-31)/(2*20+1) = 69/63
32:1-02 (100-32)/(2*20+1) = 68/65
33:1-01 (100-33)/(2*20+1) = 67/67
*/