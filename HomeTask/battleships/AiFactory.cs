using System;
using System.Diagnostics;

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

		public AiFactory(string aiExePath, IProcessMonitor processMonitor)
		{
			this.aiExePath = aiExePath;
			this.processMonitor = processMonitor;
		}

		public Ai CreateAi()
		{
			var ai = new Ai(aiExePath);
			ai.ProcessCreated += ai_ProcessCreated;
			return ai;
		}

		void ai_ProcessCreated(Process process)
		{
			processMonitor.Register(process);
		}
	}
}
