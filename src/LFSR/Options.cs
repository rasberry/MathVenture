using System.Text;

namespace MathVenture.LFSR;

public static class Options
{
	public static void Usage(StringBuilder sb)
	{
		string name = Aids.AspectName(PickAspect.PrimeGen);

		sb
			.WL()
			.WL(0,$"{name} [options]")
			.WL(1,"--debug"    ,"Show debug messages (implies -v)")
			.WL(1,"-v"         ,"Show extra information")
			.WL()
			//.WL(0,"gen"        ,"Generates a sequence of prime numbers and outputs one per line")
			//.WL(1,"-t (type)"  ,"Type of generator to use (leave empty to list the types)")
			//.WL(1,"-s (number)","Starting number for the generator (default 2)")
			//.WL(1,"-e (number)","Ending number for the generator (default 100)")
			//.WL(1,"-f (file)"  ,"Optional text file to store primes")
		;
	}

	public static bool ParseArgs(string[] args)
	{
		return true;
	}
}