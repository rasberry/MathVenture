using System;
using System.Collections;
using System.Collections.Generic;
using SequenceGen.Generators;

namespace SequenceGen
{
	public static class Registry
	{
		public static IEnumerable<GeneratorInfo> InfoList()
		{
			int i=0;
			GeneratorInfo gi;
			while((gi = FindByIndex(++i)) != null) {
				yield return gi;
			}
		}

		public static GeneratorInfo FindByName(string name)
		{
			foreach(var gi in InfoList()) {
				bool match = gi.Name.Equals(name,StringComparison.InvariantCultureIgnoreCase);
				if (match) { return gi; }
			}
			return null;
		}

		public static GeneratorInfo FindByIndex(int index)
		{
			string name = null;
			string info = null;
			Func<IGenerator> make = null;

			switch(index)
			{
			case 1:
				name = "CofraPi";
				info = "Continued fraction expansion of pi";
				make = () => new CofraPi();
				break;
			case 2:
				name = "GibPi";
				info = "Expansion of pi using Gibbons spigot method";
				make = () => new GibPi();
				break;
			case 3:
				name = "GibPi2";
				info = "Expansion of pi using Gibbons spigot method";
				make = () => new GibPi2();
				break;
			case 4:
				name = "CofraE";
				info = "Continued fraction expansion of e";
				make = () => new CofraE();
				break;
			case 5:
				name = "CofraE2";
				info = "Continued fraction expansion of e";
				make = () => new CofraE2();
				break;
			case 6:
				name = "CofraPhi";
				info = "Continued fraction expansion of phi";
				make = () => new CofraPhi();
				break;
			case 7:
				name = "CofraSqrt2";
				info = "Continued fraction expansion of Squre Root of 2";
				make = () => new CofraSqrt2();
				break;
			}

			if (name == null) { return null; }
			return new GeneratorInfo {
				Name = name,
				Info = info,
				Make = make,
				Index = index
			};
		}
	}
}
