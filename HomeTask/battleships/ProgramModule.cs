using System;
using System.IO;
using Ninject;
using Ninject.Modules;

namespace battleships
{
	class ProgramModule : NinjectModule
	{
		private readonly string aiPath;

		public ProgramModule(string aiPath)
		{
			this.aiPath = aiPath;
		}

		public override void Load()
		{
			Bind<TextWriter>().ToConstant(Console.Out);
			Bind<TextReader>().ToConstant(Console.In);
			Bind<Settings>().ToSelf().InSingletonScope()
				.WithConstructorArgument("settingsFilename", "settings.txt");
			Bind<ILoggerFactory>().To<LoggerFactory>()
				.WithConstructorArgument("loggerName", "results");
			var settings =  Kernel.Get<Settings>();
			Bind<IMapGenerator>().To<MapGenerator>()
				.WithConstructorArgument("random", new Random(settings.RandomSeed));
			Bind<IGameVisualizer>().To<GameVisualizer>();
			Bind<IProcessMonitor>().To<ProcessMonitor>()
				.WithConstructorArgument("timeLimit", TimeSpan.FromSeconds(settings.TimeLimitSeconds * settings.GamesCount))
				.WithConstructorArgument("memoryLimit", (long)settings.MemoryLimit);
			Bind<IAiFactory>().To<AiFactory>()
				.WithConstructorArgument("aiExePath", aiPath);
			Bind<IGameFactory>().To<GameFactory>();
			Bind<IAiTester>().To<AiTester>();
		}
	}
}
