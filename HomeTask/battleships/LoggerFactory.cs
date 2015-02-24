using NLog;

namespace battleships
{
	public interface ILoggerFactory
	{
		Logger CreateLogger();
	}

	class LoggerFactory : ILoggerFactory
	{
		private readonly string loggerName;

		public LoggerFactory(string loggerName)
		{
			this.loggerName = loggerName;
		}

		public Logger CreateLogger()
		{
			return LogManager.GetLogger(loggerName);
		}
	}
}
