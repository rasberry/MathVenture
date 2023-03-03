using System.Text;

namespace MathVenture.LFSR;

public class Start : IMain
{
	public bool ParseArgs(string[] args)
	{
		return Options.ParseArgs(args);
	}

	public void Usage(StringBuilder sb)
	{
		Options.Usage(sb);
	}

	public void Main()
	{
		//for(int i=2; i<=16; i++) {
		//	TestLFSR.TryAll(i);
		//}

		PrimitivePolyOne.FindAll(9);
		Log.Debug(new string('=',80));

		/*
		var n2b = new necklace2bitpol(9);

		for(ulong a = 0; a < 512; a++) {
			bool w = PrimitivePoly.TryPrim(a,9);
			Log.Debug($"a = {a} w = {w}");
			if (w) {
				var sb = new StringBuilder();
				ulong p = n2b.poly(a);
				Bits.bitpol_print_coeff(sb,"",p);
				//Log.Debug($"p = {p} coeff = {sb}");
				Log.Debug(sb.ToString());
			}
		}
		*/

		for(int i=0; i < 10; i++) {
			var t = PrimitivePoly.GetRandomTaps(9);

			var sb = new StringBuilder();
			Bits.bitpol_print_coeff(sb,"",t);

			Log.Debug($"taps = {t} coeff = {sb}");
		}
	}
}