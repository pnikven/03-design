using System;
using NLog;

namespace battleships
{
	public abstract class Loggable
	{
		public event Action<LogEventInfo> LogMessageHandler;

		protected void Log(LogMessageType logMessageType, string message, string loggerName = null)
		{
			if (LogMessageHandler == null) return;
			var logEventInfo = new LogEventInfo(
				MapLogMessageTypeToLogLevel(logMessageType), 
				loggerName ?? GetType().FullName, 
				message);
			LogMessageHandler(logEventInfo);
		}

		protected LogLevel MapLogMessageTypeToLogLevel(LogMessageType logMessageType)
		{
			switch (logMessageType)
			{
				case LogMessageType.Trace:
					return LogLevel.Trace;
				case LogMessageType.Debug:
					return LogLevel.Debug;
				case LogMessageType.Info:
					return LogLevel.Info;
				case LogMessageType.Warn:
					return LogLevel.Warn;
				case LogMessageType.Error:
					return LogLevel.Error;
				case LogMessageType.Fatal:
					return LogLevel.Fatal;
				default:
					throw new ArgumentOutOfRangeException("logMessageType");
			}
		}
	}
}
