using System;
using System.IO;
using System.Text;
using MathVenture.SequenceGen.Generators;

namespace MathVenture.SequenceGen
{
	public static class Options
	{
		public static void Usage(StringBuilder sb)
		{
			string name = Aids.AspectName(PickAspect.SequenceGen);

			sb
				.WL()
				.WL(0,$"{name} (sequence) [options]")
				.WL(1,"A set of algorithms that can continuously produce digits of infinite series")
				.WL(0,"Options:")
				.WL(1,"-b (number)","Output number in given base (if generator supports it)")
				.WL(1,"-d (number)","Number of digits to print (default 1000)")
				.WL(1,"-d-",        "Keep printing numbers until process is killed")
				.WL(1,"-f (file)",  "Write digits to a file instead of standard out")
				.WL(1,"-p",         "Show progress bar and stats")
				.WL(1,"-n",         "Insert newlines periodically")
				.WL(1,"-nw",        "Number of characters between newlines (default 80)")
				.WL()
				.WL(0,"Sequences:")
			;
			AppendSequences(sb);
		}

		static void AppendSequences(StringBuilder sb)
		{
			foreach(var gi in Registry.InfoList())
			{
				sb.WL(1,$"{gi.Index}. {gi.Name}",gi.Info);
			}
		}

		public static bool Parse(string[] args)
		{
			int len = args.Length;
			for(int a=0; a<len; a++)
			{
				string curr = args[a];

				if (curr == "-b" && ++a < len) {
					if (!int.TryParse(args[a], out int @base)) {
						Log.Error($"Cannot parse '{args[a]}' as a number");
						return false;
					}
					if (@base < 2) {
						Log.Error("Base cannot be less than 2");
						return false;
					}
					Base = @base;
				}
				else if (curr == "-d-") {
					NoLimit = true;
				}
				else if (curr == "-d" && ++a < len) {
					if (!long.TryParse(args[a], out long limit)) {
						Log.Error($"Cannot parse '{args[a]}' as a number");
						return false;
					}
					if (limit < 1) {
						Log.Error("Limit must be greater than zero");
						return false;
					}
					Limit = limit;
				}
				else if (curr == "-n") {
					OutputWidth = 80;
				}
				else if (curr == "-nw" && ++a < len) {
					if (!int.TryParse(args[a],out int w)) {
						Log.Error($"Cannot parse '{args[a]}' as a number");
						return false;
					}
					if (w < 1) {
						Log.Error("Width must be greater than zero");
						return false;
					}
					OutputWidth = w;
				}
				else if (curr == "-f" && ++a < len) {
					if (String.IsNullOrWhiteSpace(args[a])) {
						Log.Error("Invalid file name");
						return false;
					}
					OutputFile = args[a];
				}
				else if (curr == "-p") {
					ShowStats = true;
				}
				else if (Selected == null) {
					GeneratorInfo gi = null;
					if (int.TryParse(curr,out int index)) {
						gi = Registry.FindByIndex(index);
					}
					else {
						gi = Registry.FindByName(curr);
					}
					if (gi == null) {
						Log.Error($"Cannot find sequence generator '{curr}'");
						return false;
					}
					Selected = gi;
				}
			}

			if (Selected == null) {
				Log.Error("No generator selected");
				return false;
			}

			return true;
		}

		public static int Base = 10;
		public static bool NoLimit = false;
		public static long Limit = 1000;
		public static GeneratorInfo Selected = null;
		public static int OutputWidth = -1;
		public static string OutputFile = null;
		public static bool ShowStats = false;
	}
}