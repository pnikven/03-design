using System;
using NLog;

namespace battleships
{
	public interface IGameFactory
	{
		Game CreateGame(Map map, Ai ai);
	}

	class GameFactory : IGameFactory
	{
		private readonly Logger logger;

		public GameFactory(Logger logger)
		{
			this.logger = logger;
		}

		public Game CreateGame(Map map, Ai ai)
		{
			var game = new Game(map, ai);
			game.LogMessageHandler += game_LogMessageHandler;
			return game;
		}

		void game_LogMessageHandler(LogEventInfo info)
		{
			if (logger == null) return;
			logger.Log(info);
		}
	}
}
