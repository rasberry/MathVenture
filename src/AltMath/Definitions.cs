using System;

namespace MathVenture.AltMath
{
	public enum PickSignature
	{
		None = 0,
		DoubleDouble,
		DoubleIntDouble,
		FloatFloat
	}

	public enum PickClass
	{
		None = 0,
		Sin, Cos, Tan,
		Atan
	}

	public class FunctionInfo
	{
		public int Index;
		public string Name;
		public string Args;
		public string Info;
		public PickSignature Signature;
		public object Func;
		public PickClass Class;
	}
}
