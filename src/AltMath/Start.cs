using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace MathVenture.AltMath
{
	public class Start : IMain
	{
		public bool ParseArgs(string[] args)
		{
			return Options.Parse(args);
		}

		public void Usage(StringBuilder sb)
		{
			Options.Usage(sb);
		}

		public void Main()
		{
			var info = Options.Pick;
			if (Options.SingleValue != null) {
				PrintSingleValue(info,Options.SingleValue.Value);
			}
			else {
				double vsta = Options.SeriesStart.Value;
				double vend = Options.SeriesEnd.Value;
				double vstp = Options.SeriesStep.Value;
				if (vstp < 0 && vend > vsta || vstp > 0 && vsta > vend) {
					vstp = -vstp;
				}
				for(double v = vsta; v<=vend; v += vstp) {
					PrintSingleValue(info,v);
				}
			}
		}

		static void PrintSingleValue(FunctionInfo info, double val)
		{
			string sinp = val.ToString();
			string sout = null;

			switch(info.Signature)
			{
			case PickSignature.DoubleDouble: {
				var real = info.Func as Func<double,double>;
				sout = real(val).ToString();
				break;
			}
			case PickSignature.DoubleIntDouble: {
				var real = info.Func as Func<double,int,double>;
				sout = real(val,8).ToString();
				break;
			}
			case PickSignature.FloatFloat: {
				var real = info.Func as Func<float,float>;
				sout = real((float)val).ToString();
				break;
			}
			}

			if (Options.PrintBothIO) {
				Log.Message($"{sinp}\t{sout}");
			}
			else {
				Log.Message(sout ?? "");
			}
		}
	}
}
