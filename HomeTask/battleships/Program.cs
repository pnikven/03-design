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
		private static void Main(string[] args)
		{
			Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
			if (args.Length == 0)
			{
				Console.WriteLine("Usage: {0} <ai.exe>", Process.GetCurrentProcess().ProcessName);
				return;
			}
			var aiPath = args[0];
		    if (!File.Exists(aiPath)){
		        Console.WriteLine("No AI exe-file " + aiPath);
		        return;
		    }
			var settings = new Settings("settings.txt");
		    var logger = LogManager.GetLogger("results");
            var mapGenerator = new MapGenerator(settings, new Random(settings.RandomSeed));
            var gameVisualizer = new GameVisualizer();
            var processMonitor = new ProcessMonitor(
                TimeSpan.FromSeconds(settings.TimeLimitSeconds * settings.GamesCount), settings.MemoryLimit);
		    var aiFactory = new AiFactory(aiPath, processMonitor);
		    var gameFactory = new GameFactory();
            var tester = new AiTester(settings, logger, mapGenerator, gameVisualizer, aiFactory, gameFactory);
            tester.TestSingleFile();
		}
	}
}