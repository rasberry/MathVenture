using System;
using System.Collections;
using System.Numerics;

namespace MathVenture.PrimeGen.Methods
{
	public class PascalMod2 : IPrimeSource, IPrimeTest
	{
		static BigInteger TWO = 2;
		static BigInteger ONE = 1;
		static BigInteger ZERO = 0;

		/*
		public bool IsPrime(BigInteger test)
		{
			BigInteger mid = test / TWO;
			BigInteger last = test;
			for(BigInteger p = TWO; p < mid; p++)
			{
				//https://en.wikipedia.org/wiki/Pascal's_triangle#Calculating_a_row_or_diagonal_by_itself
				BigInteger num = last * (test + ONE - p) / p;
				//wrong -- BigInteger num = last * ((test + 1) / p - 1);
				//slow  -- BigInteger num = (test + 1 - p) * last / p;
				//hangs -- BigInteger num = last / p * (test + 1 - p);
				last = num;

				BigInteger rem;
				BigInteger.DivRem(num,test,out rem);

				if (rem != ZERO) {
					return false;
				}
			}
			return true;
		}
		*/

		public bool IsPrime(BigInteger test)
		{
			return false;
		}

		public BigInteger NextPrime(BigInteger number)
		{
			if (number == ONE) {
				return TWO;
			}

			//find nearest power of 2
			int bits = (int)Math.Ceiling(BigInteger.Log(number,2));
			BigInteger check = BigInteger.Pow(TWO,bits - 1) - ONE;
			BigInteger next = check;

			while(true) {
				next <<= 1;
				next ^= check;

				int unSet = CountUnSetBits(next);
				Console.WriteLine($"num={number} check={check} next={next} unSet={unSet}");
				if (unSet % 2 != 0) {
					return next;
				}
				check = next;
			}

			//Console.WriteLine($"num={number} bits={bits} 2^(n+1)-1 = {row}");

			/*
			int front = bits;
			var ba2N = new BitArray(row.ToByteArray());
			var baCheck = new BitArray(2 * bits, false);
			var baNext = new BitArray(2 * bits, false);
			baCheck.Or(ba2N);

			while(++front < 2 * bits) {
				baNext.LeftShift(1);
				baNext.Xor(baCheck);

				if (baNext[0] != true) {
					throw new Exception("zero bit should be one");
				}
				if (baNext[front] != true) {
					throw new Exception($"{front} bit should be one");
				}
				int count = 0;
				bool isPrime = true;
				for(int b=1; b<front; b++) {
					if (baNext[b] == false) {
						count++;
					}
					else {
						if (count % 2 == 0) {
							isPrime = false;
							break;
						}
						count = 0;
					}
				}
				if (isPrime) {
					int byteCount = IntCeil(bits,8);
					var buffer = new byte[byteCount];
					int bTemp = 0;
					for(int b=0; b < baNext.Length; b++) {
						int pos = b % 8;
						if (baNext[b] == true) {
							bTemp |= 1<<pos;
						}
						if (pos == 7) {
							buffer[b/8] = (byte)bTemp;
							bTemp = 0;
						}
					}
					return new BigInteger(buffer);
				}
				baCheck = new BitArray(baNext);
			}
			*/
			return BigInteger.Zero;
		}

		// based on Brian Kernighan’s Algorithm
		static int CountSetBits(BigInteger n)
		{
			int count = 0;
			while (n > 0) {
				n &= (n - ONE);
				count++;
			}
			return count;
		}

		static int CountUnSetBits(BigInteger n)
		{
			int setCount = CountSetBits(n);
			int total = (int)Math.Floor(BigInteger.Log(n,2)) + 1;
			return total - setCount;
		}

		static int IntCeil(int num,int den)
		{
			return 1 + ((num - 1) / den);
		}
	}
}
