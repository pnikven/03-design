using System;
using System.Collections.Generic;

namespace battleships
{
	public class AiTester : Loggable
	{
		public event Action<Game> VisualizeGameHandler;		

		private readonly Settings settings;

		public AiTester(Settings settings)
		{
			this.settings = settings;
		}

		public IEnumerable<GameStatistics> TestAi(Ai ai, IEnumerable<Game> games)
		{
			var crashes = 0;
			var gameIndex = 0;
			foreach (var game in games)
			{
				RunGameToEnd(game);
				if (game.AiCrashed)
				{
					crashes++;
					if (crashes > settings.CrashLimit) break;
					ai.ClearProcess();
				}
				var gameStatistics = new GameStatistics(game.BadShots, game.AiCrashed, game.TurnsCount);
				if (settings.Verbose)
				{
					PrintCurrentGameStatistics(gameIndex++, gameStatistics);
				}
				yield return gameStatistics;
			}			
		}

		private void PrintCurrentGameStatistics(int gameIndex, GameStatistics gameStatistics)
		{
			WriteLineToStdOut(string.Format(
				"Game #{3,4}: Turns {0,4}, BadShots {1}{2}",
				gameStatistics.TurnsCount,
				gameStatistics.BadShots,
				gameStatistics.AiCrashed ? ", Crashed" : "",
				gameIndex));
		}

		private void RunGameToEnd(Game game)
		{
			while (!game.IsOver())
			{
				game.MakeStep();
				if (settings.Interactive)
				{
					if (VisualizeGameHandler != null)
						VisualizeGameHandler(game);
					if (game.AiCrashed)
						WriteLineToStdOut(game.LastError.Message);
					ReadLineFromStdIn();
				}
			}
		}
	}
}