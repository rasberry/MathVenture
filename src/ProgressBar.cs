using System;
using System.Text;
using System.Threading;

// https://gist.github.com/DanielSWolf/0ab6a96899cc5377bf54
namespace SequenceGen
{
	/// <summary>
	/// An ASCII progress bar
	/// </summary>
	public class ProgressBar : IDisposable, IProgress<double> {

		public void Dispose() {
			lock (AnimTimer) {
				Disposed = true;
				UpdateText(string.Empty);
			}
		}

		public ProgressBar() {
			AnimTimer = new Timer(TimerHandler);

			//// A progress bar is only for temporary display in a console window.
			//// If the console output is redirected to a file, draw nothing.
			//// Otherwise, we'll end up with a lot of garbage in the target file.
			//if (!Console.IsOutputRedirected) {
			ResetTimer();
			//}
		}

		public void Report(double value) {
			Interlocked.Exchange(ref CurrentProgress, value);
		}

		public string Prefix { get; set; } = null;
		public Func<double,string> SuffixCallback { get; set; } = null;

		void TimerHandler(object state) {
			lock (AnimTimer) {
				if (Disposed) return;

				// Make sure value is in [0..1] range
				double value = Math.Max(0, Math.Min(1, CurrentProgress));

				int progressBlockCount = (int) (value * BlockCount);
				int percent = (int) (value * 100);
				if (SuffixCallback != null) {
					Suffix = SuffixCallback(value);
				}
				string text = string.Format("{4}[{0}{1}] {2,3}% {3}{5}",
					new string('#', progressBlockCount), new string('-', BlockCount - progressBlockCount),
					percent,
					AnimGliphs[AnimationIndex++ % AnimGliphs.Length],
					Prefix ?? "",
					Suffix ?? ""
				);
				UpdateText(text);
				ResetTimer();
			}
		}

		void UpdateText(string text) {
			// Get length of common portion
			int commonPrefixLength = 0;
			int commonLength = Math.Min(CurrentText.Length, text.Length);
			while (commonPrefixLength < commonLength && text[commonPrefixLength] == CurrentText[commonPrefixLength]) {
				commonPrefixLength++;
			}

			// Backtrack to the first differing character
			StringBuilder outputBuilder = new StringBuilder();
			outputBuilder.Append('\b', CurrentText.Length - commonPrefixLength);

			// Output new suffix
			outputBuilder.Append(text.Substring(commonPrefixLength));

			// If the new text is shorter than the old one: delete overlapping characters
			int overlapCount = CurrentText.Length - text.Length;
			if (overlapCount > 0) {
				outputBuilder.Append(' ', overlapCount);
				outputBuilder.Append('\b', overlapCount);
			}

			Console.Error.Write(outputBuilder);
			CurrentText = text;
		}

		void ResetTimer() {
			AnimTimer.Change(AnimInterval, TimeSpan.FromMilliseconds(-1));
		}

		const int BlockCount = 10;
		const string AnimGliphs = @"|/-\";
		readonly TimeSpan AnimInterval = TimeSpan.FromSeconds(1.0 / 8);
		readonly Timer AnimTimer;
		double CurrentProgress = 0;
		string CurrentText = string.Empty;
		bool Disposed = false;
		int AnimationIndex = 0;
		string Suffix = "";
	}
}