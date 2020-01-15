using System;
using System.Collections;
using System.Collections.Generic;

namespace SequenceGen
{
	public static class Helpers
	{
		public static IEnumerable<Digit> ToEnumerable(this IGenerator generator)
		{
			return new DigitEnumerator(generator);
		}

		class DigitEnumerator : IEnumerable<Digit>, IEnumerator<Digit>
		{
			public DigitEnumerator(IGenerator gen)
			{
				Generator = gen;
			}
			IGenerator Generator;
			Digit Curr;

			public Digit Current { get { return Curr; }}
			object IEnumerator.Current { get { return Curr; }}

			public IEnumerator<Digit> GetEnumerator() {
				return new DigitEnumerator(Generator);
			}
			IEnumerator IEnumerable.GetEnumerator() {
				return GetEnumerator();
			}
			public bool MoveNext() {
				Curr = Generator.Next;
				return true;
			}
			public void Reset() {
				Generator.Reset();
			}

			public void Dispose() {
				Generator = null;
			}
		}
	}
}