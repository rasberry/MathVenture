using System;
using System.Text;

namespace MathVenture.AltMath
{
	public static class Options
	{
		public static void Usage(StringBuilder sb)
		{
			string name = Aids.AspectName(PickAspect.SequenceGen);

			sb
				.WL()
				.WL(0,$"{name} (function) [number] [options]")
				.WL(1,"Implementations of some math functions using various numeric methods")
				.WL(0,"Options:")
				.WL(1,"-b (number)","Series start number")
				.WL(1,"-e (number)","Series end number")
				.WL(1,"-s"         ,"Series step amount")
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
				sb.WL(1,$"{gi.Index}. {gi.Name}",gi.Info);
			}
		}

	}
}