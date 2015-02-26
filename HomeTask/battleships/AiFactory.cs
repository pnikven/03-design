using System;
using System.Diagnostics;
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

		public AiFactory(string aiExePath, IProcessMonitor processMonitor, Logger logger)
		{
			this.aiExePath = aiExePath;
			this.processMonitor = processMonitor;
			this.logger = logger;
		}

		public Ai CreateAi()
		{			
			var ai = new Ai(aiExePath);			
			ai.ProcessCreatedHandler += ai_ProcessCreated;
			ai.LogMessageHandler += ai_LogMessage;
			return ai;
		}

		void ai_ProcessCreated(Process process)
		{
			processMonitor.Register(process);
		}

		void ai_LogMessage(LogEventInfo loggEventInfo)
		{
			if (logger == null) return;
			logger.Log(loggEventInfo);
		}
	}
}
