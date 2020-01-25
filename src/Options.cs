using System;
using System.IO;
using System.Text;
using SequenceGen.Generators;

namespace SequenceGen
{
	public static class Options
	{
		public static void Usage()
		{
			var sb = new StringBuilder();
			sb.Append(""
				+"\n"+nameof(SequenceGen) + " (sequence) [options]"
				+"\nOptions:"
				+"\n -h / --help            Show this help"
				+"\n -b (number)            Output number in given base (if generator supports it)"
				+"\n -d (number)            Number of digits to print (default 1000)"
				+"\n -d-                    Keep printing numbers until process is killed"
				+"\n -f (file)              Write digits to a file instead of standard out"
				+"\n -v                     Show progress bar and stats"
				+"\n -n                     Insert newlines periodically"
				+"\n -nw                    Number of characters between newlines (default 80)"
				+"\n"
				+"\nSequences:"
				+"\n"
			);
			AppendSequences(sb);

			Log.Message(sb.ToString());
		}

		static void AppendSequences(StringBuilder sb)
		{
			foreach(var gi in Registry.InfoList())
			{
				int len = gi.Index.ToString().Length + gi.Name.Length;
				string pad = new String(' ',21 - len);
				sb.AppendLine($" {gi.Index}. {gi.Name}{pad}{gi.Info}");
			}
		}

		public static bool Parse(string[] args)
		{
			int len = args.Length;
			for(int a=0; a<len; a++)
			{
				string curr = args[a];
				if (curr == "-h" || curr == "--help") {
					Usage();
					return false;
				}
				else if (curr == "-b" && ++a < len) {
					if (!int.TryParse(args[a], out int @base)) {
						Tw.WriteLine($"Cannot parse '{args[a]}' as a number");
						return false;
					}
					if (@base < 2) {
						Tw.WriteLine("Base cannot be less than 2");
						return false;
					}
					Base = @base;
				}
				else if (curr == "-d-") {
					NoLimit = true;
				}
				else if (curr == "-d" && ++a < len) {
					if (!long.TryParse(args[a], out long limit)) {
						Tw.WriteLine($"Cannot parse '{args[a]}' as a number");
						return false;
					}
					if (limit < 1) {
						Tw.WriteLine("Limit must be greater than zero");
						return false;
					}
					Limit = limit;
				}
				else if (curr == "-n") {
					OutputWidth = 80;
				}
				else if (curr == "-nw" && ++a < len) {
					if (!int.TryParse(args[a],out int w)) {
						Tw.WriteLine($"Cannot parse '{args[a]}' as a number");
						return false;
					}
					if (w < 1) {
						Tw.WriteLine("Width must be greater than zero");
						return false;
					}
					OutputWidth = w;
				}
				else if (curr == "-f" && ++a < len) {
					if (String.IsNullOrWhiteSpace(args[a])) {
						Tw.WriteLine("Invalid file name");
						return false;
					}
					OutputFile = args[a];
				}
				else if (curr == "-v") {
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
						Tw.WriteLine($"Cannot find sequence generator '{curr}'");
						return false;
					}
					Selected = gi;
				}
			}

			if (Selected == null) {
				Tw.WriteLine("No generator selected");
				return false;
			}

			return true;
		}

		static TextWriter Tw = Log.StdErr;

		public static int Base = 10;
		public static bool NoLimit = false;
		public static long Limit = 1000;
		public static GeneratorInfo Selected = null;
		public static int OutputWidth = -1;
		public static string OutputFile = null;
		public static bool ShowStats = false;
	}
}