using LiteNetLib;
using LiteNetLib.Utils;

namespace Andavies.MonoGame.Network.Utilities;

public interface IPacketBatchSender
{
	/// <summary>Adds a packet to the internal queue to be sent when SendBatch is called</summary>
	/// <param name="client">The client to send the packet to</param>
	/// <param name="packet">The packet that will be sent</param>
	/// <typeparam name="T">Type of the packet. Should be class extending from INetSerializable</typeparam>
	void AddPacket<T>(NetPeer client, T packet) where T : INetSerializable;
	
	/// <summary>Sends a packet now. This packet will not be added to the internal batch</summary>
	/// <param name="client">The client to send the packet to</param>
	/// <param name="packet">The packet that will be sent</param>
	/// <typeparam name="T">Type of the packet. Should be class extending from INetSerializable</typeparam>
	void SendPacketNow<T>(NetPeer client, T packet) where T : INetSerializable;
	
	/// <summary>Send out each packet stored in the batch. Should be called every tick</summary>
	void SendBatch();
	
	/// <summary>Clears out the current batch. No packets will be sent</summary>
	void ClearBatch();
}