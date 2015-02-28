using System;
using System.IO;
using Ninject.Modules;
using Ninject.Extensions.Conventions;

namespace battleships
{
	class ProgramModule : NinjectModule
	{
		public override void Load()
		{
			Bind<TextWriter>().ToConstant(Console.Out);
			Bind<TextReader>().ToConstant(Console.In);
			Bind<Settings>().ToSelf().InSingletonScope()
				.WithConstructorArgument("settings.txt");
			Bind<ILoggerFactory>().To<LoggerFactory>()
				.WithConstructorArgument("results");
			Kernel.Bind(x => x.FromThisAssembly().SelectAllClasses().BindAllInterfaces());
		}
	}
}
