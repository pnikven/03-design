using System;
using System.Collections.Generic;
using System.Linq;

namespace battleships
{
	class SummaryGameStatisticsCalculator : Loggable
	{
		private readonly Settings settings;

		public SummaryGameStatisticsCalculator(Settings settings)
		{
			this.settings = settings;
		}

		public void PrintSummaryGameStatistics(string aiName, IEnumerable<GameStatistics> gameStatistics)
		{
			var shots = new List<int>();
			var crashes = 0;
			var badShots = 0;
			var gamesPlayed = 0;
			foreach (var gameStats in gameStatistics)
			{
				if (gameStats.AiCrashed)
					crashes++;
				else
					shots.Add(gameStats.TurnsCount);
				badShots += gameStats.BadShots;
				gamesPlayed++;
			}
			WriteTotal(aiName, shots, crashes, badShots, gamesPlayed);
		}

		private void WriteTotal(string aiName, List<int> shots, int crashes, int badShots, int gamesPlayed)
		{
			if (shots.Count == 0) shots.Add(1000 * 1000);
			shots.Sort();
			var median = shots.Count % 2 == 1 ? shots[shots.Count / 2] : (shots[shots.Count / 2] + shots[(shots.Count + 1) / 2]) / 2;
			var mean = shots.Average();
			var sigma = Math.Sqrt(shots.Average(s => (s - mean) * (s - mean)));
			var badFraction = (100.0 * badShots) / shots.Sum();
			var crashPenalty = 100.0 * crashes / settings.CrashLimit;
			var efficiencyScore = 100.0 * (settings.Width * settings.Height - mean) / (settings.Width * settings.Height);
			var score = efficiencyScore - crashPenalty - badFraction;
			var headers = FormatTableRow(new object[] { "AiName", "Mean", "Sigma", "Median", "Crashes", "Bad%", "Games", "Score" });
			var message = FormatTableRow(new object[] { aiName, mean, sigma, median, crashes, badFraction, gamesPlayed, score });
			Log(LogMessageType.Info, message, settings.ResultsLoggerName);
			Console.WriteLine();
			Console.WriteLine("Score statistics");
			Console.WriteLine("================");
			Console.WriteLine(headers);
			Console.WriteLine(message);
		}

		private string FormatTableRow(object[] values)
		{
			return FormatValue(values[0], 15)
				+ string.Join(" ", values.Skip(1).Select(v => FormatValue(v, 7)));
		}

		private static string FormatValue(object v, int width)
		{
			return v.ToString().Replace("\t", " ").PadRight(width).Substring(0, width);
		}
	}
}
