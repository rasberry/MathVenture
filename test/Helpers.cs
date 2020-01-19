using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using SequenceGen;

namespace test
{
	public static class Helpers
	{
		public static IEnumerable<Digit> ReadDigitsFromFile(string file, int @base = 10)
		{
			var fs = File.Open(file,FileMode.Open,FileAccess.Read,FileShare.Read);
			using (fs) {
				while(true) {
					int b = fs.ReadByte();
					if (b < 0) { break; }
					int num = 0;
					if (b >= 0x30 && b <= 0x39) { //0-9
						num = b - 0x30;
					}
					else if (b >= 0x41 && b <= 0x5A) { //A-Z
						num = b - 0x37;
					}
					else if (b >= 0x61 && b <= 0x7A) { //a-z
						num = b - 0x57;
					}
					else {
						continue; //skip unrecognized values
					}
					if (num < 0 || num >= @base) {
						throw new ArgumentOutOfRangeException($"numeric value '{num}' out of range");
					}
					Digit d = new Digit(num,@base);
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