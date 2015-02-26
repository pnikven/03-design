using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using NUnit.Framework;
using FakeItEasy;

namespace battleships
{
	[TestFixture]
	class AiTester_should
	{
		private Settings settings;
		private IEnumerable<Map> gameMaps;
		private IProcessMonitor processMonitor;
		private AiFactory aiFactory;
		private Ai ai;
		private IGameFactory gameFactory;
		private IEnumerable<Game> games;
		private AiTester aiTester;
		private IGameVisualizer gameVisualizer;

		[SetUp]
		public void Setup()
		{
			var gamesCount = new Random().Next(1, 10);
			settings = new Settings()
			{
				GamesCount = gamesCount,
				CrashLimit = gamesCount,
				Ships = new[] { 1 },
				Width = 1,
				Height = 1
			};
			var mapGenerator = new MapGenerator(settings, new Random(settings.RandomSeed));
			gameMaps = Enumerable.Range(0, settings.GamesCount).Select(x => mapGenerator.GenerateMap());
			processMonitor = A.Fake<IProcessMonitor>();
			aiFactory = new AiFactory(A.Dummy<string>(), processMonitor, null);
			ai = aiFactory.CreateAi();
			gameFactory = new GameFactory(null);
			games = gameMaps.Select(map => gameFactory.CreateGame(map, ai));
			gameVisualizer = A.Fake<IGameVisualizer>();
			aiTester = new AiTester(settings);
		}

		[Test]
		public void register_process_for_each_ai()
		{
			aiTester.TestAi(ai, games);

			A.CallTo(() => processMonitor.Register(A<Process>.Ignored))
				.MustHaveHappened(Repeated.Exactly.Times(settings.GamesCount));
		}

		[Test]
		public void visualize_game_in_interactive_mode()
		{
			settings.Interactive = true;
			aiTester = new AiTester(settings);
			aiTester.VisualizeGameHandler += game => gameVisualizer.Visualize(game);

			aiTester.TestAi(ai, games);

			A.CallTo(() => gameVisualizer.Visualize(A<Game>.Ignored))
				.MustHaveHappened(Repeated.Exactly.Times(settings.GamesCount));
		}
	}
}
