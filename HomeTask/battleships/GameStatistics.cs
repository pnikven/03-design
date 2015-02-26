namespace battleships
{
	public class GameStatistics
	{
		public int BadShots { get; private set; }
		public bool AiCrashed { get; private set; }
		public int TurnsCount { get; private set; }

		public GameStatistics(int badShots, bool aiCrashed, int turnsCount)
		{
			BadShots = badShots;
			AiCrashed = aiCrashed;
			TurnsCount = turnsCount;
		}
	}
}
