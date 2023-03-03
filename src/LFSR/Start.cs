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
		for(int i=2; i<=9; i++) {
			TestLFSR.TryAll(i);
		}

		//int bits = 63;

		//PrimitivePolyOne.FindAll((ulong)bits);
		//Log.Debug(new string('=',80));

		/*
		var n2b = new necklace2bitpol(9);

		for(ulong a = 0; a < 512; a++) {
			bool w = PrimitivePoly.TryPrim(a,9);
			//Log.Debug($"a = {a} w = {w}");
			if (w) {
				var sb = new StringBuilder();
				ulong p = n2b.poly(a) >> 1;
				Bits.bitpol_print_coeff(sb,"",p);
				Log.Debug($"p = {p} coeff = {sb}");
				//Log.Debug(sb.ToString());
			}
		}
		*/

		/*
		for(int i=0; i < 10; i++) {
			var t = PrimitivePoly.GetRandomTaps(bits);

			var sb = new StringBuilder();
			Bits.bitpol_print_coeff(sb,"",t);

			Log.Debug($"taps = {t} coeff = {sb}");
		}
		*/

		for(int i = 0; i < 50; i++) {
			var taps = PrimitivePoly.GetRandomTaps(9);
			var seq = TestLFSR.SequenceBits(1,taps);

			ulong count = 0;
			foreach(var n in seq) {
				count++;
			}
			Log.Debug($"taps={taps} B:{TestLFSR.ToBinary(taps)} count={count}");
		}
	}
}