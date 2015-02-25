using System;
using System.Diagnostics;
using NUnit.Framework;
using FakeItEasy;

namespace battleships
{
	[TestFixture]
	class AiTester_should
	{
		[Test]
		public void register_process_for_each_ai()
		{
			var gamesCount = new Random().Next(1, 10);
			var settings = new Settings()
			{
				GamesCount = gamesCount,
				CrashLimit = gamesCount,
				Ships = new[] { 1 },
				Width = 1,
				Height = 1
			};
			var processMonitor = A.Fake<IProcessMonitor>();
			var aiTester = new AiTester(settings, processMonitor);

			aiTester.TestSingleFile(A.Dummy<string>());

			A.CallTo(() => processMonitor.Register(A<Process>.Ignored))
				.MustHaveHappened(Repeated.Exactly.Times(settings.GamesCount));
		}
	}
}
