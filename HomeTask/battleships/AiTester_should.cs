using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using NUnit.Framework;
using FakeItEasy;
using NLog;

namespace battleships
{
	[TestFixture]
	class AiTester_should
	{
		private Settings settings;
		private Logger logger;
		private IGameFactory gameFactory;
		private IEnumerable<Map> gameMaps;

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
			logger = A.Fake<Logger>();
			gameFactory = new GameFactory(logger);
			var mapGenerator = new MapGenerator(settings, new Random(settings.RandomSeed));
			gameMaps = Enumerable.Range(0, settings.GamesCount).Select(x => mapGenerator.GenerateMap());
		}

		[Test]
		public void register_process_for_each_ai()
		{
			var processMonitor = A.Fake<IProcessMonitor>();
			var aiFactory = new AiFactory(A.Dummy<string>(), processMonitor, logger);
			var aiTester = new AiTester(settings, aiFactory, gameFactory);

			aiTester.TestAi(A.Dummy<string>(), gameMaps);

			A.CallTo(() => processMonitor.Register(A<Process>.Ignored))
				.MustHaveHappened(Repeated.Exactly.Times(settings.GamesCount));
		}

		[Test]
		public void restart_ai_by_recreating_it_with_aiFactory()
		{
			var aiFactory = A.Fake<IAiFactory>();
			var aiTester = new AiTester(settings, aiFactory, gameFactory);

			aiTester.TestAi(A.Dummy<string>(), gameMaps);

			A.CallTo(() => aiFactory.CreateAi())
				.MustHaveHappened(Repeated.Exactly.Times(settings.GamesCount + 1));
		}
	}
}
