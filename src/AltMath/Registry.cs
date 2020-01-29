using System;
using System.Collections.Generic;

namespace MathVenture.AltMath
{
	public static class Registry
	{
		public static IEnumerable<FunctionInfo> InfoList()
		{
			int index = 0;
			yield return new FunctionInfo {
				Index = ++index, Name = "CordicSin",
				Args = "(number:angle) [number:accuracy=8]",
				Info = "Sin using CORDIC (for COordinate Rotation DIgital Computer)",
				Signature = PickSignature.DoubleIntDouble,
				Class = PickClass.Sin,
				Func = (Func<double,int,double>)Cordic.Sin
			};
			yield return new FunctionInfo {
				Index = ++index, Name = "CordicCos",
				Args = "(number:angle) [number:accuracy=8]",
				Info = "Cos using CORDIC (for COordinate Rotation DIgital Computer)",
				Signature = PickSignature.DoubleIntDouble,
				Class = PickClass.Cos,
				Func = (Func<double,int,double>)Cordic.Cos
			};
			yield return new FunctionInfo {
				Index = ++index, Name = "SpecrumSin",
				Args = "(number:angle)",
				Info = "ZX Spectrum Calculator SIN X function",
				Signature = PickSignature.DoubleDouble,
				Class = PickClass.Sin,
				Func = (Func<double,double>)SpectrumRom.Sin
			};
			yield return new FunctionInfo {
				Index = ++index, Name = "Sin5",
				Args = "(number:angle) [number:accuracy=8]",
				Info = "Sin using Chebyshev mapping method",
				Signature = PickSignature.DoubleIntDouble,
				Class = PickClass.Sin,
				Func = (Func<double,int,double>)Sine.Sin5
			};
			yield return new FunctionInfo {
				Index = ++index, Name = "Sin6",
				Args = "(number:angle) [number:accuracy=8]",
				Info = "Sin using Geometric Product expansion sin(x) = x*PROD(k=[1,Inf](1-(x/(k*PI)^2)))",
				Signature = PickSignature.DoubleIntDouble,
				Class = PickClass.Sin,
				Func = (Func<double,int,double>)Sine.Sin6
			};
			yield return new FunctionInfo {
				Index = ++index, Name = "SinFdlibm",
				Args = "(number:angle)",
				Info = "Sin function from fdlibm/k_sin.c",
				Signature = PickSignature.DoubleDouble,
				Class = PickClass.Sin,
				Func = (Func<double,double>)Sine.SinFdlibm
			};
			yield return new FunctionInfo {
				Index = ++index, Name = "SinSO",
				Args = "(number:angle)",
				Info = "Sin function from Stack Overflow 'How does C compute sin() and other math functions?'",
				Signature = PickSignature.DoubleDouble,
				Class = PickClass.Sin,
				Func = (Func<double,double>)Sine.SinSO
			};
			yield return new FunctionInfo {
				Index = ++index, Name = "SinSO2",
				Args = "(number:angle)",
				Info = "Sin function from Stack Overflow 'How does C compute sin() and other math functions?'",
				Signature = PickSignature.DoubleDouble,
				Class = PickClass.Sin,
				Func = (Func<double,double>)Sine.SinSO2
			};
			yield return new FunctionInfo {
				Index = ++index, Name = "SinTaylor",
				Args = "(number:angle)",
				Info = "Sin function using trucated taylor series",
				Signature = PickSignature.DoubleDouble,
				Class = PickClass.Sin,
				Func = (Func<double,double>)Sine.SinTaylor
			};
			yield return new FunctionInfo {
				Index = ++index, Name = "SinXupremZero",
				Args = "(number:angle)",
				Info = "Sin function by XupremZero",
				Signature = PickSignature.FloatFloat,
				Class = PickClass.Sin,
				Func = (Func<float,float>)Sine.SinXupremZero
			};
			yield return new FunctionInfo {
				Index = ++index, Name = "CosXupremZero",
				Args = "(number:angle)",
				Info = "Cos function by XupremZero",
				Signature = PickSignature.FloatFloat,
				Class = PickClass.Cos,
				Func = (Func<float,float>)Sine.CosXupremZero
			};
		}

		public static FunctionInfo MapByIndex(int index)
		{
			foreach(var info in InfoList()) {
				if (info.Index == index) { return info; }
			}
			return null;
		}

		public static FunctionInfo MapByName(string name)
		{
			foreach(var info in InfoList()) {
				bool match = info.Name.Equals(name,StringComparison.InvariantCultureIgnoreCase);
				if (match) { return info; }
			}
			return null;
		}
	}
}