using System;
using System.Text;
using SequenceGen.Generators;

namespace SequenceGen
{
	class Program
	{
		static void Main(string[] args)
		{
			if (args.Length < 1) {
				Options.Usage();
				return;
			}

			if (!Options.Parse(args)) {
				return;
			}

			var gen = Options.Selected.Make();
			ICanHasBases ichb = gen as ICanHasBases;
			if (ichb != null) {
				ichb.Base = Options.Base;
			}
			
			long count = 0;
			int ow = Options.OutputWidth;

			while(true)
			{
				var dig = gen.Next;
				Console.Write(dig.ToString());
				count++;
				if (ow > 0) {
					if (count % ow == 0) {
						Console.WriteLine();
					}
				}
				if (!Options.NoLimit && count > Options.Limit) {
					break;
				}
			}
			Console.WriteLine();
		}
	}
}
