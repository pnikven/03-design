using System.IO;
using NLog;

namespace battleships
{
	public interface IAiFactory
	{
		Ai CreateAi();
	}

	class AiFactory : IAiFactory
	{
		private readonly string aiExePath;
		private readonly IProcessMonitor processMonitor;
		private readonly Logger logger;
		private readonly TextWriter textWriter;
		private readonly TextReader textReader;

		public AiFactory(string aiExePath, IProcessMonitor processMonitor, Logger logger, TextWriter textWriter, TextReader textReader)
		{
			this.aiExePath = aiExePath;
			this.processMonitor = processMonitor;
			this.logger = logger;
			this.textWriter = textWriter;
			this.textReader = textReader;
		}

		public Ai CreateAi()
		{
			var ai = new Ai(aiExePath);
			ai.ProcessCreatedHandler += process => processMonitor.Register(process); ;
			ai.LogMessageHandler += loggEventInfo =>
			{
				if (logger == null) return;
				logger.Log(loggEventInfo);
			};
			ai.WriteLineHandler += message => textWriter.WriteLine(message);
			ai.ReadLineHandler += () => textReader.ReadLine();
			return ai;
		}
	}
}
