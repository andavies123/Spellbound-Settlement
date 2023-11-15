using System.Diagnostics;

namespace Andavies.MonoGame.Game.Server;

public class ServerStarter : IServerStarter
{
	private const string ServerExePath = @"C:\Users\Andrew Davies\Desktop\Spellbound Settlement\Andavies.MonoGame.Game.Server\bin\Debug\net6.0\Andavies.MonoGame.Game.Server.exe";
	
	public void StartServer(string ipAddress)
	{
		ProcessStartInfo startInfo = new()
		{
			FileName = ServerExePath,
			UseShellExecute = true,
			CreateNoWindow = false
		};

		Process.Start(startInfo);
	}
}