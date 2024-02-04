using System.Collections.Concurrent;
using Andavies.MonoGame.Network.Extensions;
using LiteNetLib;
using LiteNetLib.Utils;
using Serilog;

namespace Andavies.MonoGame.Network.Utilities;

public class PacketBatchSender : IPacketBatchSender
{
	private readonly ILogger _logger;
	
	private readonly ConcurrentQueue<Action> _batchQueue = new();
	private readonly NetPacketProcessor _netPacketProcessor = new();
	private readonly NetDataWriter _netDataWriter = new();
    
	public PacketBatchSender(ILogger logger)
	{
		_logger = logger ?? throw new ArgumentNullException(nameof(logger));
	}

	public IReadOnlyCollection<Action> PacketBatchCollection => _batchQueue;

	public void AddPacket<T>(NetPeer client, T packet) where T : INetSerializable
	{
		_batchQueue.Enqueue(() => SendPacketNow(client, packet));
	}

	public void SendPacketNow<T>(NetPeer client, T packet) where T : INetSerializable
	{
		_netDataWriter.Reset();
		_netPacketProcessor.WriteNetSerializable(_netDataWriter, ref packet);
		
		client.Send(_netDataWriter, DeliveryMethod.ReliableOrdered);
		_logger.LogPacketSent(packet, client.EndPoint.ToString());
	}

	public void SendBatch()
	{
		while (_batchQueue.TryDequeue(out Action? sendAction))
		{
			sendAction.Invoke();
		}
	}

	public void ClearBatch()
	{
		_batchQueue.Clear();
	}
}