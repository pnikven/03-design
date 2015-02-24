using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Threading;
using Ninject;

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

			var standardKernel = new StandardKernel();
			standardKernel.Bind<Settings>().To<Settings>().InSingletonScope()
				.WithConstructorArgument("settingsFilename", "settings.txt");
			standardKernel.Bind<ILoggerFactory>().To<LoggerFactory>()
				.WithConstructorArgument("loggerName", "results");
			var settings = standardKernel.Get<Settings>();
			standardKernel.Bind<IMapGenerator>().To<MapGenerator>()
				.WithConstructorArgument("random", new Random(settings.RandomSeed));
			standardKernel.Bind<IGameVisualizer>().To<GameVisualizer>();
			standardKernel.Bind<IProcessMonitor>().To<ProcessMonitor>()
				.WithConstructorArgument("timeLimit", TimeSpan.FromSeconds(settings.TimeLimitSeconds*settings.GamesCount))
				.WithConstructorArgument("memoryLimit", (long)settings.MemoryLimit);
			standardKernel.Bind<IAiFactory>().To<AiFactory>()
				.WithConstructorArgument("aiExePath", aiPath);
			standardKernel.Bind<IGameFactory>().To<GameFactory>();
			standardKernel.Bind<IAiTester>().To<AiTester>();

			standardKernel.Get<IAiTester>().TestAi();
		}
	}
}