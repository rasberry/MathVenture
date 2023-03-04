using System;
using System.Globalization;
using System.Numerics;
using System.Text;

namespace MathVenture.LFSR;

public static class Options
{
	public static void Usage(StringBuilder sb)
	{
		string name = Aids.AspectName(PickAspect.LFSR);

		sb
			.WL()
			.WL(0,$"{name} [options]")
			.WL(1,"-b (number)" ,"The bit count to produce a 2^(bitcount)-1 sequence [1-63]")
			.WL(1,"-m (number)" ,"Max number to allow for the sequence (-b and -m should not be used together)")
			.WL(1,"-x"          ,"Use Xilinx taps instead of random ones (ignored if -t is specified)")
			.WL(1,"-t (number)" ,"A primitive polynomial for use as taps")
			.WL(1,"-i (number)" ,"The initial state for the LFSR (defaults to 1)")
			.WL()
		;
	}

	public static bool ParseArgs(string[] args)
	{
		int len = args.Length;
		for(int a=0; a<len; a++)
		{
			string curr = args[a];

			if (curr == "-t" && ++a < len) {
				if (!TryParseNumber("taps",args[a],out Taps)) {
					return false;
				}
			}
			else if (curr == "-i" && ++a < len) {
				if (!TryParseNumber("initial state",args[a],out InitialState)) {
					return false;
				}
			}
			else if (curr == "-b" && ++a < len) {
				if (!TryParseNumber("bit count",args[a],out BitCount)) {
					return false;
				}
			}
			else if (curr == "-m" && ++a < len) {
				if (!TryParseNumber("max",args[a],out Max)) {
					return false;
				}
			}
			else if (curr == "-x") {
				UseXilinx = true;
			}
		}

		if (BitCount != null && (BitCount.Value < 1 || BitCount.Value > 63)) {
			Log.Error("bit count must be between 1 and 63");
			return false;
		}

		return true;
	}

	static bool TryParseNumber<T>(string name, string val, out T? num) where T: struct, IBinaryInteger<T>
	{
		num = null;
		if (!T.TryParse(val,NumberStyles.Any,null,out T raw)) {
			Log.Error($"unacceptable value for {name} '{val}'");
			return false;
		}
		num = raw;
		return true;
	}

	public static ulong? InitialState = null;
	public static ulong? Taps = null;
	public static ulong? Max = null;
	public static int? BitCount = null;
	public static bool UseXilinx = false;
}