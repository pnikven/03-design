using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Threading;
using Ninject;

namespace battleships
{
	public class Program
	{
		private static void Main(string[] args)
		{
			Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
			if (args.Length == 0)
			{
				Console.WriteLine("Usage: {0} <ai.exe>", Process.GetCurrentProcess().ProcessName);
				return;
			}
			var aiPath = args[0];
			if (!File.Exists(aiPath)){
				Console.WriteLine("No AI exe-file " + aiPath);
				return;
			}

			var standardKernel = new StandardKernel(new ProgramModule(aiPath));
			standardKernel.Get<IAiTester>().TestAi();
		}
	}
}