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
			game.LogMessageHandler += info =>
			{
				if (logger == null) return;
				logger.Log(info);
			};
			return game;
		}
	}
}
