using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace MathVenture.AltMath
{
	public class Start : IMain
	{
		public bool ParseArgs(string[] args)
		{
			return Options.Parse(args);
		}

		public void Usage(StringBuilder sb)
		{
			Options.Usage(sb);
		}

		public void Main()
		{
		}
	}
}
