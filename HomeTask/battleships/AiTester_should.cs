using System;
using System.Diagnostics;
using NUnit.Framework;
using FakeItEasy;

namespace battleships
{
	[TestFixture]
	class AiTester_should
	{
		private Settings settings;

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
		}

		[Test]
		public void register_process_for_each_ai()
		{
			var processMonitor = A.Fake<IProcessMonitor>();
			var aiFactory = new AiFactory(A.Dummy<string>(), processMonitor);
			var aiTester = new AiTester(settings, aiFactory);

			aiTester.TestAi(A.Dummy<string>());

			A.CallTo(() => processMonitor.Register(A<Process>.Ignored))
				.MustHaveHappened(Repeated.Exactly.Times(settings.GamesCount));
		}

		[Test]
		public void restart_ai_by_recreating_it_with_aiFactory()
		{
			var aiFactory = A.Fake<IAiFactory>();
			var aiTester = new AiTester(settings, aiFactory);

			aiTester.TestAi(A.Dummy<string>());

			A.CallTo(() => aiFactory.CreateAi())
				.MustHaveHappened(Repeated.Exactly.Times(settings.GamesCount + 1));
		}
	}
}
