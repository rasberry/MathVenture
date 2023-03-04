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
				BigInteger sqr = p.Sqrt();
				int i=0;
				do {
					var check = _store[i];
					BigInteger.DivRem(p, check, out BigInteger rem);
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
	}
}
