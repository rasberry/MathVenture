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
		PrimitivePoly.FindAll(25);
	}
}