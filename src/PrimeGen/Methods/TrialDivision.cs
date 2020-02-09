using System;
using System.Numerics;

namespace MathVenture.PrimeGen.Methods
{
	public class TrialDivision : IPrimeSource, IPrimeTest
	{
		public BigInteger GetPrime(long index)
		{
			if (index < 0) {
				throw new ArgumentOutOfRangeException("index must be positive");
			}

			Init();

			long count = _store.Count;
			//pull last discovered prime
			BigInteger p = _store[count-1] + 2;

			//generate primes if necessary
			while(count <= index)
			{
				bool isPrime = true;
				BigInteger sqr = Sqrt(p);
				int i=0;
				do {
					var check = _store[i];
					BigInteger rem;
					BigInteger.DivRem(p,check, out rem);
					if (rem == 0) {
						isPrime = false;
						break;
					}
					i++;
				} while (i < sqr);

				if (isPrime) {
					_store.Add(p);
					count++;
				}
				p += 2; //skip evens
			}

			return _store[index];
		}

		public BigInteger NextPrime(BigInteger number)
		{
			Init();

			if (number < 2) {
				return 2;
			}

			var last = _store[_store.Count - 1];
			if (number > last) {
				//generate primes since number is past the end of the list
				long index = _store.Count - 1;
				BigInteger p = _store[index];
				while(p <= number) {
					p = GetPrime(++index);
				}
				return p;
			}
			else {
				//find the prime or nearest prime in the list
				long index = _store.IndexOf(number, out long near);
				if (index > -1) {
					return GetPrime(index + 1);
				}
				else {
					return _store[near];
				}
			}
		}

		public bool IsPrime(BigInteger number)
		{
			var next = NextPrime(number-1);
			return next == number;
		}


		static PrimeStore _store = null;
		static void Init()
		{
			if (_store != null) { return; }
			_store = PrimeStore.Self;
		}

		//BigInteger doesn't have a built-in sqrt function
		static BigInteger Sqrt(BigInteger n)
		{
			if (n == 0 || n == 1) { return n; }
			if (n <= 0) {
				throw new ArithmeticException("NaN");
			}

			int bitLength = (int)Math.Ceiling(BigInteger.Log(n, 2));
			BigInteger root = BigInteger.One << (bitLength / 2);

			BigInteger last0 = 0;
			BigInteger last1 = 1;
			//some times the number oscilates between sqrt+0 and sqrt+1
			while(last1 != root)
			{
				last1 = last0;
				last0 = root;
				root = (root + (n / root)) >> 1;
			}

			return BigInteger.Min(root,last0);
		}
	}
}
