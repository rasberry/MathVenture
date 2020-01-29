using System;
using System.Text;

namespace MathVenture.AltMath
{
	public static class Options
	{
		public static void Usage(StringBuilder sb)
		{
			string name = Aids.AspectName(PickAspect.AltMat);

			sb
				.WL()
				.WL(0,$"{name} (function) [number] [options]")
				.WL(1,"Implementations of some math functions using various numeric methods")
				.WL(0,"Options:")
				.WL(1,"-b (number)","Series start number")
				.WL(1,"-e (number)","Series end number")
				.WL(1,"-s (number)","Series step amount")
				.WL(1,"-a (number)","Accuracy (for functions that support it)")
				.WL(1,"-io"        ,"Print both input and output")
				.WL()
				.WL(0,"Functions:")
			;
			ListFuntions(sb);

			Log.Message(sb.ToString());
		}

		static void ListFuntions(StringBuilder sb)
		{
			foreach(var gi in Registry.InfoList())
			{
				string pad = gi.Index < 10 ? " " : "";
				sb.WL(1,$"{pad}{gi.Index}. {gi.Name}",gi.Info);
			}
		}

		public static bool Parse(string[] args)
		{
			int len = args.Length;
			double singlenum = 0.0;

			for(int a=0; a<len; a++) {
				string curr = args[a];

				if (curr == "-b" && ++a < len) {
					if (!double.TryParse(args[a],out double num)) {
						Log.Error($"could not parse '{args[a]}' as a number");
						return false;
					}
					SeriesStart = num;
				}
				else if (curr == "-e" && ++a < len) {
					if (!double.TryParse(args[a],out double num)) {
						Log.Error($"could not parse '{args[a]}' as a number");
						return false;
					}
					SeriesEnd = num;
				}
				else if (curr == "-s" && ++a < len) {
					if (!double.TryParse(args[a],out double num)) {
						Log.Error($"could not parse '{args[a]}' as a number");
						return false;
					}
					SeriesStep = num;
				}
				else if (curr == "-io") {
					PrintBothIO = true;
				}
				else if (Pick == null) {
					if (int.TryParse(curr,out int val)) {
						Pick = Registry.MapByIndex(val);
					}
					if (Pick == null) {
						Pick = Registry.MapByName(curr);
					}
				}
				else if (Pick != null && double.TryParse(curr,out singlenum)) {
					SingleValue = singlenum;
				}
			}

			if (Pick == null) {
				Log.Error("no valid function was specified");
				return false;
			}
			bool hasVal = SingleValue != null ||
				(SeriesStart != null && SeriesEnd != null && SeriesStep != null)
			;
			if (!hasVal) {
				Log.Error("you must specify a single value or -b, -e, and -s");
				return false;
			}
			if (SeriesStep != null && Math.Abs(SeriesStep.Value) < double.Epsilon) {
				Log.Error("step value must be not be zero");
				return false;
			}
			return true;
		}

		public static double? SeriesStart = null;
		public static double? SeriesEnd   = null;
		public static double? SeriesStep  = null;
		public static double? SingleValue = null;
		public static FunctionInfo Pick   = null;
		public static bool PrintBothIO    = false;
	}
}