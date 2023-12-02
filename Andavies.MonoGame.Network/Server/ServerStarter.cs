using System.Diagnostics;
using System.Reflection;
using Serilog;

namespace Andavies.MonoGame.Network.Server;

public class ServerStarter : IServerStarter
{
	// Todo: Everything in this class should be general
	private const string WindowsServerPath = "Andavies.SpellboundSettlement.Server.exe";
	private const string MacServerPath = "Andavies.SpellboundSettlement.Server";

	private readonly ILogger _logger;

	public ServerStarter(ILogger logger)
	{
		_logger = logger;
	}
	
	public void StartServer(string arguments)
	{
		foreach (Process process in Process.GetProcessesByName("Andavies.SpellboundSettlement.Server"))
		{
			_logger.Debug("Stopping: {processName}", process.ProcessName);
			process.Kill();
			process.WaitForExit();
			process.Dispose();
		}
		
		ProcessStartInfo startInfo = new()
		{
			FileName = GetServerExecutablePath(),
			UseShellExecute = true,
			CreateNoWindow = false,
			Arguments = arguments
		};

		_logger.Information("Starting server...");
		Process.Start(startInfo);
	}

	private static string GetServerExecutablePath()
	{
		string assemblyLocation = Assembly.GetExecutingAssembly().Location;
		string assemblyDirectory = Path.GetDirectoryName(assemblyLocation) ?? string.Empty;
		
		//Todo: Find better way to get the assembly path, this should be temporary
		string path = Path.Combine(assemblyDirectory, WindowsServerPath);
		if (File.Exists(path))
			return path;
		path = Path.Combine(assemblyDirectory, MacServerPath);
		if (File.Exists(path))
			return path;

		return string.Empty;
	}
}