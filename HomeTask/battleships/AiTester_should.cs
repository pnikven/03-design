using System;
using FakeItEasy;
using NUnit.Framework;

namespace battleships
{
	[TestFixture]
	class AiTester_should
	{
		private ILoggerFactory loggerFactory;
		private Settings settings;
		private IMapGenerator mapGenerator;
		private IGameVisualizer gameVisualizer;
		private IAiFactory aiFactory;
		private IGameFactory gameFactory;

		[SetUp]
		public void Setup()
		{
			loggerFactory = A.Fake<ILoggerFactory>();
			settings = new Settings();
			mapGenerator = A.Fake<IMapGenerator>();
			gameVisualizer = A.Fake<IGameVisualizer>();
			aiFactory = A.Fake<IAiFactory>();
			gameFactory = A.Fake<IGameFactory>();
		}

		[Test]
		public void create_game_by_gameFactory_exactly_GamesCount_times()
		{
			settings.GamesCount = new Random().Next(1, 10);

			new AiTester(settings, loggerFactory, mapGenerator, gameVisualizer, aiFactory, gameFactory).TestAi();

			A.CallTo(() => gameFactory.CreateGame(A<Map>.Ignored, A<Ai>.Ignored))
				.MustHaveHappened(Repeated.Exactly.Times(settings.GamesCount));
		}

		[Test]
		public void create_ai_by_aiFactory_at_least_one_time()
		{
			new AiTester(settings, loggerFactory, mapGenerator, gameVisualizer, aiFactory, gameFactory).TestAi();

			A.CallTo(() => aiFactory.CreateAi()).MustHaveHappened(Repeated.AtLeast.Once);
		}

		[Test]
		public void visualize_each_game_in_Interactive_mode()
		{
			settings.GamesCount = new Random().Next(1, 10);
			settings.CrashLimit = settings.GamesCount;	// Поскольку ai будет фейковый, он каждый раз будет ломаться.
														// Чтобы тестирование не прекратилось после первого же краха ai,
														// устанавливаем лимит крахов ai равным количеству игр
			settings.Interactive = true;
			settings.Ships = new[] { 1 };
			settings.Width = 1;
			settings.Height = 1;
			mapGenerator = new MapGenerator(settings, new Random());
			gameFactory = new GameFactory();

			new AiTester(settings, loggerFactory, mapGenerator, gameVisualizer, aiFactory, gameFactory).TestAi();

			A.CallTo(() => gameVisualizer.Visualize(A<Game>.Ignored))
				.MustHaveHappened(Repeated.Exactly.Times(settings.GamesCount));
		}

		[Test]
		public void create_logger_by_loggerFactory()
		{
			new AiTester(settings, loggerFactory, mapGenerator, gameVisualizer, aiFactory, gameFactory);

			A.CallTo(() => loggerFactory.CreateLogger())
				.MustHaveHappened(Repeated.Exactly.Once);
		}

		[Test]
		public void generate_map_by_mapGenerator_exactly_GamesCount_times()
		{
			settings.GamesCount = new Random().Next(1, 10);

			new AiTester(settings, loggerFactory, mapGenerator, gameVisualizer, aiFactory, gameFactory).TestAi();

			A.CallTo(() => mapGenerator.GenerateMap())
				.MustHaveHappened(Repeated.Exactly.Times(settings.GamesCount));
		}
	}
}
