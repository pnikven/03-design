using System;
using System.IO;
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
				.WithConstructorArgument("settings.txt");
			Bind<ILoggerFactory>().To<LoggerFactory>()
				.WithConstructorArgument("results");
			Bind<IMapGenerator>().To<MapGenerator>();
			Bind<IGameVisualizer>().To<GameVisualizer>();
			Bind<IProcessMonitor>().To<ProcessMonitor>();
			Bind<IAiFactory>().To<AiFactory>()
				.WithConstructorArgument("aiExePath", aiPath);
			Bind<IGame>().To<Game>();
			Bind<IAiTester>().To<AiTester>();
		}
	}
}
