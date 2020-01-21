using System;

namespace SequenceGen
{
	public struct Digit : IComparable<Digit>, IEquatable<Digit>
	{
		public Digit(int number, int @base = 10)
		{
			Base = @base;
			if (number < 0 || number > @base - 1) {
				throw new ArgumentOutOfRangeException($"Digit must be 0-{@base-1}");
			}
			Value = number;
		}

		readonly int Value;
		readonly int Base;

		public int CompareTo(Digit other) {
			EnsureSameBase(other);
			return this.Value - other.Value;
		}
		public bool Equals(Digit other) {
			EnsureSameBase(other);
			return this.Value.Equals(other.Value);
		}
		public static implicit operator int(Digit d) {
			return (int)d.Value;
		}
		public static explicit operator Digit(int b) {
			return new Digit(b);
		}
		public override string ToString() {
			return Value.ToString();
		}

		void EnsureSameBase(Digit other) {
			if (this.Base != other.Base) {
				throw new ArithmeticException(
					$"Cannot compare numbers from different bases ({this.Base} vs {other.Base})"
				);
			}
		}
	}
}
