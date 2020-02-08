using System;
using System.Numerics;

namespace MathVenture.PrimeGen
{
	public class BitsEratosthenes : IPrimeSource, IDisposable
	{
		public BigInteger NextPrime(BigInteger number)
		{
			Init();
			//TODO starting from anwhere is kind of a problem
			//need to still mark off all of the previous primes < Start
			//.. so where to put that data ?
			//i think you have run through all the previous primes.. so
			//alterntive (to storing everyting) would be to record primes and replay them on each block
			//

			return BigInteger.Zero;
		}

		public void FillPrimes(BigInteger start)
		{
			Init();

			//TODO ignoring start for now
			if (Current < 2) {
				Current = 2;
			}

			long p = (long)Current;
			while(p < dba.Length)
			{
				long index = p + p;
				while(index < dba.Length) {
					dba[index] = false; //cross off every composite
					index += p;
				}
				
				long next = p + 1;
				while(next < dba.Length && dba[next] == false) {
					next++;
				}

				if (next >= dba.Length) {
					return;
				} else {
					p = next;
				}
			}
		}

		void Init()
		{
			if (dba != null) { return; }
			long size = Options.TargetSize;
			if (size < 1) {
				size = (long)(Options.End - Options.Start);
			}

			dba = new DiskBitArray(Options.OutputFile,size);
			dba.ResetAllTo(true); //everything is 'prime' at first
			Current = Options.Start;
		}

		DiskBitArray dba = null;
		BigInteger Current = BigInteger.Zero;
		PrimeStore _store = PrimeStore.Self;

		public void Dispose()
		{
			if (dba != null) {
				dba.Dispose();
			}
		}
	}
}
