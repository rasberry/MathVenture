using System;
using System.Collections.Generic;
using System.Text;

namespace MathVenture
{
	public static class Options
	{
		public static void Usage(PickAspect aspect = PickAspect.None)
		{
			StringBuilder sb = new StringBuilder();
			string name = nameof(MathVenture);
			sb
				.WL(0,$"Usage {name} (aspect) [options]")
				.WL(0,"Options:")
				.WL(1,"-h / --help","Show full help")
				.WL(1,"(aspect) -h","Aspect specific help")
				.WL(1,"--aspects"  ,"List possible aspects")
			;
			
			if (ShowFullHelp)
			{
				foreach(PickAspect a in Aids.EnumAll<PickAspect>()) {
					IMain func = Registry.Map(a);
					func.Usage(sb);
				}
			}
			else if (aspect != PickAspect.None)
			{
				IMain func = Registry.Map(aspect);
				func.Usage(sb);
			}
			else
			{
				if (ShowHelpAspects) {
					sb
						.WL()
						.WL(0,"Aspects:")
						.PrintEnum<PickAspect>(1)
					;
				}
			}

			Log.Message(sb.ToString());
		}

		public static bool Parse(string[] args, out string[] prunedArgs)
		{
			prunedArgs = null;
			var pArgs = new List<string>();

			int len = args.Length;
			for(int a=0; a<len; a++) {
				string curr = args[a];
				if (curr == "-h" || curr == "--help") {
					if (Aspect == PickAspect.None) {
						ShowFullHelp = true;
					}
					else {
						ShowHelpAspects = true;
					}
				}
				else if (curr == "--aspects") {
					ShowHelpAspects = true;
				}
				else if (Aspect == PickAspect.None) {
					PickAspect which;
					if (!Aids.TryParse<PickAspect>(curr,out which)) {
						Log.Error("unkown action \""+curr+"\"");
						return false;
					}
					Aspect = which;
				}
				else {
					pArgs.Add(curr);
				}
			}

			if (ShowFullHelp || ShowHelpAspects) {
				Usage(Aspect);
				return false;
			}

			if (Aspect == PickAspect.None) {
				Log.Error("aspect was not specified");
				return false;
			}

			prunedArgs = pArgs.ToArray();
			return true;
		}

		public static bool ShowFullHelp = false;
		public static bool ShowHelpAspects = false;
		public static PickAspect Aspect = PickAspect.None;

	}
}