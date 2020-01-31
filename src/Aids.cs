using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MathVenture
{
	public static class Aids
	{
		public static string AspectName(PickAspect a)
		{
			return ((int)a).ToString() + ". " + a.ToString();
		}

		public static IEnumerable<T> EnumAll<T>(bool includeZero = false)
			where T : struct
		{
			foreach(T a in Enum.GetValues(typeof(T))) {
				int v = (int)((object)a);
				if (!includeZero && v == 0) { continue; }
				yield return a;
			};
		}

		public static void PrintEnum<T>(this StringBuilder sb, int level, bool nested = false, Func<T,string> descriptionMap = null,
			Func<T,string> nameMap = null) where T : struct
		{
			var allEnums = EnumAll<T>().ToList();
			int numLen = 1 + (int)Math.Floor(Math.Log10(allEnums.Count));
			foreach(T e in allEnums) {
				int inum = (int)((object)e);
				string pnum = inum.ToString();
				string npad = pnum.Length < numLen ? new string(' ',numLen - pnum.Length) : "";
				if (nested) { npad = " "+npad; }
				string pname = nameMap == null ? e.ToString() : nameMap(e);
				string ppad = new string(' ',(nested ? 24 : 26) - pname.Length);
				string pdsc = descriptionMap == null ? "" : descriptionMap(e);
				sb.WL(level,$"{npad}{pnum}. {pname}{ppad}{pdsc}");
			}
		}

		public static bool TryParse<V>(string sub, out V val) where V : IConvertible
		{
			val = default(V);
			TypeCode tc = val.GetTypeCode();
			Type t = typeof(V);

			if (t.IsEnum) {
				if (Enum.TryParse(t,sub,true,out object o)) {
					val = (V)o;
					return Enum.IsDefined(t,o);
				}
				return false;
			}

			switch(tc)
			{
			case TypeCode.Double: {
				if (double.TryParse(sub,out double b)) {
					val = (V)((object)b); return true;
				} break;
			}
			case TypeCode.Int32: {
				if (int.TryParse(sub,out int b)) {
					val = (V)((object)b); return true;
				} break;
			}
			//add others as needed
			}
			return false;
		}

		const int ColumnOffset = 30;
		public static StringBuilder WL(this StringBuilder sb, int level, string def, string desc)
		{
			int pad = level;
			return sb
				.Append(' ',pad)
				.Append(def)
				.Append(' ',ColumnOffset - def.Length - pad)
				.Append(desc)
				.AppendLine();
		}

		public static StringBuilder WL(this StringBuilder sb, int level, string def)
		{
			int pad = level;
			return sb
				.Append(' ',pad)
				.Append(def)
				.AppendLine();
		}

		public static StringBuilder WL(this StringBuilder sb, string s = null)
		{
			return s == null ? sb.AppendLine() : sb.AppendLine(s);
		}
	}
}