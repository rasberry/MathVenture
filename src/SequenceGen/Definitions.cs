using System;

namespace MathVenture.SequenceGen
{
	public interface IGenerator
	{
		Digit Next { get; }
		void Reset();
	}

	public interface ICanHasBases
	{
		int Base { get; set; }
	}

	public class GeneratorInfo
	{
		public string Name;
		public string Info;
		public Func<IGenerator> Make;
		public int Index;
	}
}