using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace battleships
{
	public class Ai : Loggable
	{
		public event Action<Process> ProcessCreatedHandler;

		private Process process;
		private readonly string exePath;

		public Ai(string exePath)
		{
			this.exePath = exePath;
		}

		public string Name
		{
			get { return Path.GetFileNameWithoutExtension(exePath); }
		}

		public void ClearProcess()
		{
			if (ProcessIsActive)
				process = null;
		}

		public Vector Init(int width, int height, int[] shipSizes)
		{
			if (!ProcessIsActive) process = RunProcess();
			SendMessage("Init {0} {1} {2}", width, height, string.Join(" ", shipSizes));
			return ReceiveNextShot();
		}

		private bool ProcessIsActive { get { return process != null && !process.HasExited; } }

		public Vector GetNextShot(Vector lastShotTarget, ShtEffct lastShot)
		{
			SendMessage("{0} {1} {2}", lastShot, lastShotTarget.X, lastShotTarget.Y);
			return ReceiveNextShot();
		}

		private void SendMessage(string messageFormat, params object[] args)
		{
			var message = string.Format(messageFormat, args);
			process.StandardInput.WriteLine(message);
			Log(LogMessageType.Debug, "SEND: " + message);
		}

		public void Dispose()
		{
			if (process == null || process.HasExited) return;
			Log(LogMessageType.Debug, "CLOSE");
			process.StandardInput.Close();
			if (!process.WaitForExit(500))
				Log(LogMessageType.Info, string.Format("Not terminated {0}", process.ProcessName));
			try
			{
				process.Kill();
			}
			catch
			{
				//nothing to do
			}
			process = null;
		}

		private Process RunProcess()
		{
			var startInfo = new ProcessStartInfo
			{
				FileName = exePath,
				UseShellExecute = false,
				RedirectStandardInput = true,
				RedirectStandardOutput = true,
				RedirectStandardError = true,
				CreateNoWindow = true,
				WindowStyle = ProcessWindowStyle.Hidden
			};
			var aiProcess = new Process { StartInfo = startInfo };
			if (ProcessCreatedHandler != null)
			{
				ProcessCreatedHandler(aiProcess);
			}
			aiProcess.Start();
			return aiProcess;
		}

		private Vector ReceiveNextShot()
		{
			var output = process.StandardOutput.ReadLine();
			Log(LogMessageType.Debug, "RECEIVE " + output);
			if (output == null)
			{
				var err = process.StandardError.ReadToEnd();
				Console.WriteLine(err);
				Log(LogMessageType.Info, err);
				throw new Exception("No ai output");
			}
			try
			{
				var parts = output.Split(' ').Select(int.Parse).ToList();
				return new Vector(parts[0], parts[1]);
			}
			catch (Exception e)
			{
				throw new Exception("Wrong ai output: " + output, e);
			}
		}
	}
}