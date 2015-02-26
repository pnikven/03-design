using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using NLog;

namespace battleships
{
	public class Program
	{
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();		

		private static void Main(string[] args)
		{
			Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
			if (args.Length == 0)
			{
				Console.WriteLine("Usage: {0} <ai.exe>", Process.GetCurrentProcess().ProcessName);
				return;
			}
			var aiPath = args[0];
			if (!File.Exists(aiPath))
			{
				Console.WriteLine("No AI exe-file " + aiPath);
				return;
			}

			var settings = new Settings("settings.txt");
			var resultsLogger = LogManager.GetLogger(settings.ResultsLoggerName);
			var processMonitor = new ProcessMonitor(
				TimeSpan.FromSeconds(settings.TimeLimitSeconds * settings.GamesCount), settings.MemoryLimit);
			processMonitor.LogMessageHandler += logEventInfo => Logger.Log(logEventInfo);
			var mapGenerator = new MapGenerator(settings, new Random(settings.RandomSeed));
			var gameMaps = Enumerable.Range(0, settings.GamesCount).Select(x => mapGenerator.GenerateMap());
			var aiFactory = new AiFactory(aiPath, processMonitor, Logger);
			var ai = aiFactory.CreateAi();
			var gameFactory = new GameFactory(Logger);
			var games = gameMaps.Select(map => gameFactory.CreateGame(map, ai));
			var aiTester = new AiTester(settings);
			var gameVisualizer = new GameVisualizer();
			aiTester.VisualizeGameHandler += game => gameVisualizer.Visualize(game);
			aiTester.LogMessageHandler += logEventInfo => resultsLogger.Log(logEventInfo);
			IEnumerable<GameStatistics> gameStatistics = aiTester.TestAi(ai, games);
			var summaryGameStatisticsCalculator = new SummaryGameStatisticsCalculator(settings);
			summaryGameStatisticsCalculator.LogMessageHandler += logEventInfo => resultsLogger.Log(logEventInfo);
			summaryGameStatisticsCalculator.PrintSummaryGameStatistics(ai.Name, gameStatistics);
			ai.Dispose();
		}
	}
}