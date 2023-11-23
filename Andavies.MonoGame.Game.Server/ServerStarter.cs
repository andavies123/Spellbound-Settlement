using System.Diagnostics;
using System.Reflection;

namespace Andavies.MonoGame.Game.Server;

public class ServerStarter : IServerStarter
{
	private const string WindowsServerPath = "Andavies.MonoGame.Game.Server.exe";
	private const string MacServerPath = "Andavies.MonoGame.Game.Server";
	
	public void StartServer(string ipAddress)
	{
		foreach (Process process in Process.GetProcessesByName("Andavies.MonoGame.Game.Server"))
		{
			Console.WriteLine($"Stopping: {process.ProcessName}");
			process.Kill();
			process.WaitForExit();
			process.Dispose();
		}
		
		ProcessStartInfo startInfo = new()
		{
			FileName = GetServerExecutablePath(),
			UseShellExecute = true,
			CreateNoWindow = false
		};

		Console.WriteLine("Client: Attempting to start server");
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