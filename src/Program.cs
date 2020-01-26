using System;

namespace MathVenture
{
	class Program
	{
		public static void Main(string[] args)
		{
			if (args.Length < 1) {
				Options.Usage();
				return;
			}

			//parse initial options - determines which aspect to do
			if (!Options.Parse(args, out var pruned)) {
				return;
			}

			//map / parse aspect specific arguments
			IMain func = Registry.Map(Options.Aspect);
			if (!func.ParseArgs(pruned)) {
				return;
			}

			//kick off aspect
			try {
				func.Main();
			}
			catch(Exception e) {
				#if DEBUG
				Log.Error(e.ToString());
				#else
				Log.Error(e.Message);
				#endif
			}
		}
	}
}
