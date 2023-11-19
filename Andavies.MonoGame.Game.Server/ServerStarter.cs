using System.Diagnostics;
using System.Reflection;

namespace Andavies.MonoGame.Game.Server;

public class ServerStarter : IServerStarter
{
	public void StartServer(string ipAddress)
	{
		Console.WriteLine(GetServerExePath());
		ProcessStartInfo startInfo = new()
		{
			FileName = GetServerExePath(),
			UseShellExecute = true,
			CreateNoWindow = false
		};

		Process.Start(startInfo);
	}

	private static string GetServerExePath()
	{
		string assemblyLocation = Assembly.GetExecutingAssembly().Location;
		string assemblyDirectory = Path.GetDirectoryName(assemblyLocation) ?? string.Empty;
		return Path.Combine(assemblyDirectory, "Andavies.MonoGame.Game.Server.exe");
	}
}