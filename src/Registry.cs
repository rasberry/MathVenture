using System;

namespace MathVenture
{
	public enum PickAspect
	{
		None = 0,
		SequenceGen = 1
	}

	public static class Registry
	{
		public static IMain Map(PickAspect aspect)
		{
			switch(aspect)
			{
			case PickAspect.SequenceGen:
				return new SequenceGen.Start();
			}

			return null;
		}
	}
}