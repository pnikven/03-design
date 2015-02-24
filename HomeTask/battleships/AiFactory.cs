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
			return new Ai(aiExePath, processMonitor);
		}
	}
}
