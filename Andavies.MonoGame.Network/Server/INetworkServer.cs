using System.Net;
using LiteNetLib;
using LiteNetLib.Utils;

namespace Andavies.MonoGame.Network.Server;

public interface INetworkServer
{
	/// <summary>Raised when the server has been initialized and is now running</summary>
	event Action? ServerStarted;
	/// <summary>Raised when a new client connected to the server</summary>
	public event Action<NetPeer>? ClientConnected;
	/// <summary>Raised when a client disconnects from the server</summary>
	public event Action<NetPeer>? ClientDisconnected;
	
	/// <summary>Call this to create the server and start the current server loop</summary>
	/// <param name="ipAddress">The ip address the server will run on</param>
	/// <param name="port">The port the server will run on</param>
	/// <param name="maxAllowedUsers">The max amount of users that can join the server</param>
	void Start(IPAddress ipAddress, int port, int maxAllowedUsers);

	/// <summary>Call this to have the server poll events on received messages</summary>
	void Update();
	
	/// <summary>Call this to stop the current server loop and close the server</summary>
	void Stop();
	
	/// <summary>Call this to send a packet to the client</summary>
	/// <param name="client">The client that will receive the packet</param>
	/// <param name="packet">The packet that will be sent to the client</param>
	/// <typeparam name="T">Packet object that extends from INetSerializable</typeparam>
	void SendPacket<T>(NetPeer client, T packet) where T : INetSerializable;
	
	/// <summary></summary>
	/// <param name="onReceivedCallback"></param>
	/// <typeparam name="T"></typeparam>
	void AddSubscription<T>(Action<INetSerializable, NetPeer> onReceivedCallback) where T : INetSerializable, new();
	
	/// <summary></summary>
	/// <param name="onReceivedCallback"></param>
	/// <typeparam name="T"></typeparam>
	void RemoveSubscription<T>(Action<INetSerializable, NetPeer> onReceivedCallback) where T : INetSerializable, new();
}