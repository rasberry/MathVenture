using System;
using System.Text;

namespace MathVenture
{
	public interface IMain
	{
		void Usage(StringBuilder sb);
		bool ParseArgs(string[] args);
		void Main();
	}
}