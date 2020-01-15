using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using SequenceGen;

namespace test
{
	public static class Helpers
	{
		public static IEnumerable<Digit> ReadDigitsFromFile(string file)
		{
			var fs = File.Open(file,FileMode.Open,FileAccess.Read,FileShare.Read);
			using (fs) {
				while(true) {
					int b = fs.ReadByte();
					if (b < 0) { break; }
					if (b < 0x30 || b > 0x39) { continue; }
					int num = b - 0x30;
					Digit d = new Digit(num);
					yield return d;
				}
			}
		}

		public static string ProjectRoot { get
		{
			if (RootFolder == null) {
				string root = AppContext.BaseDirectory;
				int i=40;
				while(--i > 0) {
					string f = new DirectoryInfo(root).Name;
					if (string.Equals(f,nameof(SequenceGen),StringComparison.CurrentCultureIgnoreCase)) {
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