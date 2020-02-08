using System;
using System.Collections.Generic;
using System.Reflection;
using test;
using System.Diagnostics;
using test.AltMath;

namespace bench
{
	class Program
	{
		static void Main(string[] args)
		{
			var tsw = Stopwatch.StartNew();
			foreach(var provider in GetAllITestItemProvider()) {
				Console.WriteLine("\n"+provider.GetType().FullName);
				Console.WriteLine("Name\tAccuracy\tSpeed");
				foreach(var tuple in provider.GetItems()) {
					DoTest(provider,tuple.Item1,tuple.Item2);
				}
			}
			Console.WriteLine("\nTotal Time: "+tsw.ElapsedMilliseconds);
		}

		static void DoTest(ITestItemProvider provider, TestItem item,Func<TestItem,double> test)
		{
			double acc = test == null ? 0.0 : test.Invoke(item);

			var sw = Stopwatch.StartNew();
			provider.SpeedTest(item);
			var time = sw.Elapsed;

			Console.WriteLine(
				String.Format("{0}\t{1}\t{2}",item.Name,acc,time.TotalMilliseconds)
			);
		}

		static IEnumerable<ITestItemProvider> GetAllITestItemProvider()
		{
			var itipType = typeof(ITestItemProvider);
			foreach(var asem in GetAssemblies()) {
				foreach(Type t in asem.GetTypes()) {
					bool keep = t.IsClass && itipType.IsAssignableFrom(t);
					if (keep) {
						var inst = Activator.CreateInstance(t) as ITestItemProvider;
						yield return inst;
					}
				}
			}
		}

		static IEnumerable<Assembly> GetAssemblies()
		{
			var list = new List<string>();
			var stack = new Stack<Assembly>();
			stack.Push(Assembly.GetEntryAssembly());

			do {
				var asm = stack.Pop();
				yield return asm;

				foreach (var reference in asm.GetReferencedAssemblies()) {
					if (!list.Contains(reference.FullName)) {
						stack.Push(Assembly.Load(reference));
						list.Add(reference.FullName);
					}
				}
			}
			while (stack.Count > 0);
		}
	}
}
