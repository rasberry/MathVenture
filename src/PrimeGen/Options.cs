using System;
using System.Diagnostics;
using System.Globalization;
using System.Numerics;
using System.Text;

namespace MathVenture.PrimeGen
{
	public static class Options
	{
		public static void Usage(StringBuilder sb)
		{
			string name = Aids.AspectName(PickAspect.PrimeGen);

			sb
				.WL()
				.WL(0,$"{name} (gen|bits|bitsimg) [options]")
				.WL(1,"--debug"    ,"Show debug messages (implies -v)")
				.WL(1,"-v"         ,"Show extra information")
				.WL()
				.WL(0,"gen"        ,"Generates a sequence of prime numbers and outputs one per line")
				.WL(1,"-t (type)"  ,"Type of generator to use (leave empty to list the types)")
				.WL(1,"-s (number)","Starting number for the generator (default 2)")
				.WL(1,"-e (number)","Ending number for the generator (default 100)")
				.WL(1,"-f (file)"  ,"Optional text file to store primes")
				//.WL()
				//.WL(0,"bits"       ,"Generate array of bits with primes as 1s and composites as 0s")
				//.WL(1,"-t (type)"  ,"Type of generator to use (leave empty to list the types)")
				//.WL(1,"-s (number)","Starting number for the generator (default 2)")
				//.WL(1,"-e (number)","Ending number for the generator (default 100) or use -l")
				//.WL(1,"-l (size)"  ,"Target size of file. can specify K/M/G/T/E suffixes or use -e")
				//.WL(1,"-f (file)"  ,"File to store the bits")
				//.WL()
				//.WL(0,"bitsimg"    ,"Generate an image using the prime number bit array process")
				//.WL(1,"-t (type)"  ,"Type of generator to use (leave empty to list the types)")
				//.WL(1,"-s (number)","Starting number for the generator (default 2)")
				//.WL(1,"-d (w) (h)" ,"Dimensions of the image (width and height)")
				//.WL(1,"-c (number)","Bits per color between 1 and 48 (default 1)")
				//.WL(1,"-p (file)"  ,"Color palette file to use for coloring")
				//.WL(1,"-f (file)"  ,"File to store image")
			;
		}

		static void ShowAllTypes()
		{
			Log.Message("\nTypes for 'gen':");
			ShowTypes<GenType>("gen");
			Log.Message("\nTypes for 'bits':");
			ShowTypes<BitsType>("bits");
			Log.Message("\nTypes for 'bitsimg':");
			ShowTypes<BitsImgType>("bitsimg");
		}

		static void ShowTypes<T>(string name) where T:struct
		{
			foreach(string n in Enum.GetNames(typeof(T))) {
				if (n == "None") { continue; }
				Log.Message(" "+n);
			}
		}

		public static ActionType Action = ActionType.None;
		public static GenType WhichGen = GenType.None;
		public static BitsType WhichBits = BitsType.None;
		public static BitsImgType WhichBitsImg = BitsImgType.None;
		public static BigInteger Start = 2;
		public static BigInteger End = 0;
		public static long TargetSize = 0;
		public static string OutputFile = null;
		public static SizeL Dimensions = new SizeL(0,0);
		public static int BitsPerColor = 1;
		public static string PrimeStoreFile = "primes.sqlite3"; //TODO make this configurable

		public static bool ParseArgs(string[] args)
		{
			int len = args.Length;
			for(int a=0; a<len; a++)
			{
				string curr = args[a];
				
				if (curr == "--debug") {
					Trace.Listeners.Add(new TextWriterTraceListener(Console.Out));
					Log.ShowInfo = true;
				}
				else if (curr == "-v") {
					Log.ShowInfo = true;
				}
				else if (curr == "-t") {
					if (++a < len) {
						if (Action == ActionType.Gen
							&& !TryParseType<GenType>(args[a],out WhichGen)) {
							return false;
						}
						else if (Action == ActionType.Bits
							&& !TryParseType<BitsType>(args[a],out WhichBits)) {
							return false;
						}
						else if (Action == ActionType.Bits
							&& !TryParseType<BitsType>(args[a],out WhichBits)) {
							return false;
						}
					}
					else {
						ShowAllTypes();
						return false;
					}
				}
				else if (curr == "-s" && ++a < len) {
					if (!BigInteger.TryParse(args[a],NumberStyles.Any,null,out Start)) {
						Log.Error("cound not parse number '"+args[a]+"'");
						return false;
					}
					if (Start < 2) {
						Log.Warning(Start+" is below 2 so setting start to 2");
						Start = 2;
					}
				}
				else if (curr == "-e" && ++a < len) {
					if (!BigInteger.TryParse(args[a],NumberStyles.Any,null,out End)) {
						Log.Error("cound not parse number '"+args[a]+"'");
						return false;
					}
				}
				else if (curr == "-l" && ++a < len) {
					string val = args[a];
					long multiplier = 1;
					if (!String.IsNullOrWhiteSpace(val)) {
						     if (val.EndsWithIC("k")) { multiplier = 1L<<10; }
						else if (val.EndsWithIC("m")) { multiplier = 1L<<20; }
						else if (val.EndsWithIC("g")) { multiplier = 1L<<30; }
						else if (val.EndsWithIC("t")) { multiplier = 1L<<40; }
						else if (val.EndsWithIC("e")) { multiplier = 1L<<50; }
					}
					if (multiplier != 1) {
						val = val.Substring(0,val.Length-1);
					}
					if (!Int64.TryParse(val,NumberStyles.Any,null,out TargetSize)) {
						Log.Error("could not parse size '"+args[a]+"'");
						return true;
					}
					TargetSize *= multiplier;
				}
				else if (curr == "-f" && ++a < len) {
					OutputFile = args[a];
				}
				else if (curr == "-d" && (a+=2) < len) {
					long w,h;
					if (!Int64.TryParse(args[a-1],NumberStyles.Any,null,out w)) {
						Log.Error("could not parse number '"+args[a-1]+"'");
						return false;
					}
					if (!Int64.TryParse(args[a],NumberStyles.Any,null,out h)) {
						Log.Error("could not parse number '"+args[a]+"'");
						return false;
					}
					Dimensions = new SizeL(w,h);
				}
				else if (curr == "-c" && ++a < len) {
					if (!Int32.TryParse(args[a],NumberStyles.Any,null,out BitsPerColor)) {
						Log.Error("could not parse number '"+args[a]+"'");
						return false;
					}
					if (BitsPerColor < 1 || BitsPerColor > 48) {
						Log.Error("bits per color must be between 1 and 48");
						return false;
					}
				}
				else if (curr == "-p" && ++a < len) {
					//TODO - Color palette file to use for coloring
				}
				else {
					if (Action == ActionType.None) {
						Enum.TryParse<ActionType>(args[a],true,out Action);
					}
				}
			}

			if (!AreInputsSane()) { return false; }
			return true;
		}

		public static bool AreInputsSane()
		{
			if (Action == ActionType.Gen) {
				if (WhichGen == GenType.None) {
					WhichGen = GenType.Division;
				}
				if (End == 0) {
					End = Start + 100;
				}
				if (Start >= End) {
					Log.Error("start must be smaller than end");
					return false;
				}
			}
			else if (Action == ActionType.Bits) {
				if (TargetSize != 0 && End != 0) {
					Log.Warning("-l will not be used since -e is set");
				}
				if (TargetSize == 0 && End == 0) {
					TargetSize = 1024;
				}
				if (String.IsNullOrWhiteSpace(OutputFile)) {
					Log.Error("an output file must be provided");
					return false;
				}
			}
			else if (Action == ActionType.BitsImg) {
				if (Dimensions.Width < 1 || Dimensions.Height < 1) {
					Log.Error("invalid dimensions '"+Dimensions.Width+","+Dimensions.Height+"'");
					return false;
				}
				//TODO figure out color depth for image based on BitsPerColor
			}
			else if (Enum.IsDefined(typeof(ActionType),Action)) {
				//Nothing to do
			}
			else {
				Log.Error("invalid action specified");
				return false;
			}

			return true;
		}

		static bool TryParseType<T>(string inp,out T item) where T:struct
		{
			if (!Enum.TryParse<T>(inp,true,out item)) {
				Log.Error("could not parse type '"+inp+"'");
				return false;
			}
			return true;
		}
	}
}