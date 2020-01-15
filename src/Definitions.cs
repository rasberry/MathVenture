using System;
using System.Collections.Generic;

namespace SequenceGen
{
	public interface IGenerator
	{
		Digit Next { get; }
		void Reset();
	}
}