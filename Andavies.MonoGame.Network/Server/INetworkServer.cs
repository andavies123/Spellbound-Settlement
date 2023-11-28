namespace Andavies.MonoGame.Network.Server;

public interface INetworkServer
{
	/// <summary>Call this to create the server and start the current server loop</summary>
	/// <param name="port">The port the server will run on</param>
	/// <param name="maxAllowedUsers">The max amount of users that can join the server</param>
	void Start(int port, int maxAllowedUsers);
	
	/// <summary>Call this to stop the current server loop and close the server</summary>
	void Stop();
}

public interface IPeerReceiver
{
	
}

public interface IPeerSender
{
	
}