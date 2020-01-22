﻿using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using SequenceGen.Generators;

namespace SequenceGen
{
	class Program
	{
		static void Main(string[] args)
		{
			if (args.Length < 1) {
				Options.Usage();
				return;
			}

			if (!Options.Parse(args)) {
				return;
			}

			var gen = Options.Selected.Make();
			ICanHasBases ichb = gen as ICanHasBases;
			if (ichb != null) {
				ichb.Base = Options.Base;
			}

			//ss = show stats
			//or = output redirected
			//of = output file
			//ds = do stats
			//ss or of ds
			// 0  X  X  0
			// 1  0  0  0
			// 1  0  1  1
			// 1  1  0  1
			// 1  1  1  1
			
			bool doStats = Options.ShowStats
				&& (Options.OutputFile != null || Console.IsOutputRedirected)
			;
			if (doStats) {
				var prog = new ProgressBar {
					SuffixCallback = PopSuffix
				};
				using(prog) {
					OutputDigits(gen,prog);
				}
			}
			else {
				OutputDigits(gen);
			}
		}

		static void OutputDigits(IGenerator gen, ProgressBar prog = null)
		{
			if (Options.OutputFile != null) {
				var fs = File.Open(Options.OutputFile,FileMode.Create,FileAccess.Write,FileShare.Read);
				var sw = new StreamWriter(fs,Encoding.UTF8);
				using(fs) using(sw) {
					GenerateDigits(gen,sw,prog);
				}
			}
			else {
				GenerateDigits(gen,Console.Out,prog);
			}
		}

		static long digitCount = 0;
		static void GenerateDigits(IGenerator gen,TextWriter tw, ProgressBar prog = null)
		{
			int ow = Options.OutputWidth;

			while(true)
			{
				var dig = gen.Next;
				tw.Write(dig.ToString());
				digitCount++;
				if (prog != null) {
					prog.Report((double)digitCount / Options.Limit);
				}
				if (ow > 0) {
					if (digitCount % ow == 0) {
						tw.WriteLine();
					}
				}
				if (!Options.NoLimit && digitCount > Options.Limit) {
					break;
				}
			}
			tw.WriteLine();
		}

		static Process ThisProc = Process.GetCurrentProcess();
		static Stopwatch TotalTime = Stopwatch.StartNew();
		static string PopSuffix(double val)
		{
			var et = TotalTime.Elapsed;
			var min = Math.Floor(et.TotalMinutes);
			var dps = digitCount / et.TotalSeconds;
			//var mem = System.GC.GetTotalMemory(false) / 1048576.0;
			var mem = ThisProc.PrivateMemorySize64 / 1048576.0;
			var cpu = ThisProc.TotalProcessorTime / et * 100.0;
			return $" {min}m {et.Seconds}s  {dps:0.0}dps  mem {mem:n2}MiB  avg cpu {cpu:n1}%";
		}
	}
}
