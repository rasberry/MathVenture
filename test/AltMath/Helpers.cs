using System;
using System.Collections.Generic;
using System.IO;

namespace test.AltMath
{
	public static class Helpers
	{

		public static void Log(string messsage)
		{
			EnsureLogOpen();
			FileWriter.WriteLine(messsage);
			FileWriter.Flush();
		}

		static void EnsureLogOpen()
		{
			if (FileWriter != null) { return; }

			string filename = Path.Combine(Environment.CurrentDirectory,"test-log.txt");
			var fs = File.Open(filename,FileMode.Create,FileAccess.Write,FileShare.Read);
			FileWriter = new StreamWriter(fs);
			string start = "\n= Log Started at " + DateTime.Now.ToString("yyyyMMdd-HHmmss") + " ";
			FileWriter.WriteLine(start + new string('=',80 - start.Length));
		}

		static StreamWriter FileWriter = null;
	}

	public class TestItem
	{
		public Delegate Method;
		public double Delta;
		public string Name;
	}

	public interface ITestItemProvider
	{
		double SpeedTest(TestItem testItem);
		IEnumerable<(TestItem,Func<TestItem,double>)> GetItems();
	}
}
