using System.Net;

namespace Andavies.MonoGame.Network.Server;

public interface INetworkServer
{
	/// <summary>Call this to create the server and start the current server loop</summary>
	/// <param name="ipAddress">The ip address the server will run on</param>
	/// <param name="port">The port the server will run on</param>
	/// <param name="maxAllowedUsers">The max amount of users that can join the server</param>
	void Start(IPAddress ipAddress, int port, int maxAllowedUsers);
	
	/// <summary>Call this to stop the current server loop and close the server</summary>
	void Stop();
}