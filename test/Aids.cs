using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace test
{
	public static class Aids
	{
		public static string ProjectRoot { get
		{
			if (RootFolder == null) {
				string root = AppContext.BaseDirectory;
				int i=40;
				while(--i > 0) {
					string f = new DirectoryInfo(root).Name;
					if (string.Equals(f,nameof(MathVenture),StringComparison.CurrentCultureIgnoreCase)) {
						break;
					} else {
						root = new Uri(Path.Combine(root,"..")).LocalPath;
					}
				}
				RootFolder = root;
			}
			return RootFolder;
		}}
		static string RootFolder = null;
	}
}
