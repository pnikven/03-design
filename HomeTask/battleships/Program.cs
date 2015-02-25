using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
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
			var settings = new Settings("settings.txt");
			var processMonitor = new ProcessMonitor(
				TimeSpan.FromSeconds(settings.TimeLimitSeconds * settings.GamesCount), settings.MemoryLimit);
			processMonitor.LogMessageHandler += logEventInfo => Logger.Log(logEventInfo);
			var aiFactory = new AiFactory(aiPath, processMonitor, Logger);
			var gameFactory = new GameFactory(Logger);
			var tester = new AiTester(settings, aiFactory, gameFactory);
			tester.LogMessageHandler += logEventInfo => LogManager.GetLogger(settings.ResultsLoggerName).Log(logEventInfo);
			if (File.Exists(aiPath))
				tester.TestAi(aiPath);
			else
				Console.WriteLine("No AI exe-file " + aiPath);
		}
	}
}