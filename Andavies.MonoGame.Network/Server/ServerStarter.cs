using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using Serilog;

namespace Andavies.MonoGame.Network.Server;

public class ServerStarter : IServerStarter
{
	private readonly ILogger _logger;

	public ServerStarter(ILogger logger)
	{
		_logger = logger;
	}
	
	public void StartServer(string arguments, string processName)
	{
		foreach (Process process in Process.GetProcessesByName(processName))
		{
			_logger.Debug("Stopping: {processName}", process.ProcessName);
			process.Kill();
			process.WaitForExit();
			process.Dispose();
		}
		
		ProcessStartInfo startInfo = new()
		{
			FileName = GetServerExecutablePath(processName),
			UseShellExecute = true,
			CreateNoWindow = false,
			Arguments = arguments
		};

		_logger.Information("Starting server...");
		Process.Start(startInfo);
	}

	private string GetServerExecutablePath(string processName)
	{
		string assemblyLocation = Assembly.GetExecutingAssembly().Location;
		string assemblyDirectory = Path.GetDirectoryName(assemblyLocation) ?? string.Empty;

		string process;
		if (OperatingSystem.IsWindows())
		{
			process = Path.Combine(assemblyDirectory, processName + ".exe");
			_logger.Information("Starting Windows process: {process}", process);
		}
		else if (OperatingSystem.IsMacOS())
		{
			process = Path.Combine(assemblyDirectory, processName);
			_logger.Information("Starting MacOS process: {process}", process);
		}
		else
		{
			_logger.Error("Unable to start server on this operating system. {operatingSystem}", RuntimeInformation.OSDescription);
			throw new Exception();
		}

		return Path.Combine(assemblyDirectory, process);
	}
}