using System;
using System.Globalization;
using System.Numerics;
using System.Text;

namespace MathVenture.Factors
{
	public enum PickMethod
	{
		None = 0,
		Basic = 1,
		Sqrt = 2,
		Parallel = 3
	}

	public static class Options
	{
		public static void Usage(StringBuilder sb)
		{
			string name = Aids.AspectName(PickAspect.Factors);

			sb
				.WL()
				.WL(0,$"{name} (number) [options]")
				.WL(1,"-m (method)"    ,"Use another method (default Basic)")
				.WL()
				.WL(0,"Methods:")
				.PrintEnum<PickMethod>(1)
			;
		}

		public static bool ParseArgs(string[] args)
		{
			int len = args.Length;
			for(int a=0; a<len; a++)
			{
				string curr = args[a];

				if (curr == "-m" && ++a < len) {
					if (!TryParseType<PickMethod>(args[a],out WhichMethod)) {
						return false;
					}
				}
				else if (!StartIsSet && !BigInteger.TryParse(curr,
					NumberStyles.Any,NumberFormatInfo.InvariantInfo
					,out Options.StartNumber
				)) {
					Log.Error($"could not parse '{curr}' as a number");
					return false;
				}
			}

			if (Options.WhichMethod == PickMethod.None) {
				Options.WhichMethod = PickMethod.Basic;
			}

			return true;
		}

		static bool TryParseType<T>(string inp,out T item) where T:struct
		{
			if (!Enum.TryParse<T>(inp,true,out item)) {
				Log.Error($"could not parse type '{inp}'");
				return false;
			}
			return true;
		}

		public static bool StartIsSet = false;
		public static BigInteger StartNumber = 0;
		public static PickMethod WhichMethod = PickMethod.None;
	}
}