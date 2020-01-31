using System;

namespace MathVenture
{
	public enum PickAspect
	{
		None = 0,
		SequenceGen = 1,
		AltMat = 2
	}

	public static class Registry
	{
		public static IMain Map(PickAspect aspect)
		{
			switch(aspect)
			{
			case PickAspect.SequenceGen:
				return new SequenceGen.Start();
			case PickAspect.AltMat:
				return new AltMath.Start();
			}

			return null;
		}
	}
}