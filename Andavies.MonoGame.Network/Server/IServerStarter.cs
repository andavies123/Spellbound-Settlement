namespace Andavies.MonoGame.Network.Server;

public interface IServerStarter
{
	void StartServer(string ipAddress, int port, string worldName);
}