using System;

namespace SequenceGen
{
	public struct Digit : IComparable<Digit>, IEquatable<Digit>
	{
		public Digit(int number) : this((byte)number)
		{}

		public Digit(byte number)
		{
			if (number < 0 || number > 9) {
				throw new ArgumentOutOfRangeException("Digit must be 0-9");
			}
			Value = number;
		}

		readonly byte Value;

		public int CompareTo(Digit other) {
			return this.Value - other.Value;
		}
		public bool Equals(Digit other) {
			return this.Value.Equals(other.Value);
		}
		public static implicit operator byte(Digit d) {
			return d.Value;
		}
		public static explicit operator Digit(byte b) {
			return new Digit(b);
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
	}
}