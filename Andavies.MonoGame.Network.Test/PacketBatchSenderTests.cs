using System.Net;
using System.Reflection;
using Andavies.MonoGame.Network.Utilities;
using LiteNetLib;
using LiteNetLib.Utils;

namespace Andavies.MonoGame.Network.Test;

[TestClass]
public class PacketBatchSenderTests
{
	#region AddPacket Tests

	[TestMethod]
	public void AddPacket_AddsItemToTheInternalCollection()
	{
		// Arrange
		PacketBatchSender packetBatchSender = new(Substitute.For<ILogger>());

		// Act
		packetBatchSender.AddPacket(CreateNetPeer(), Substitute.For<INetSerializable>());
        
		// Assert
		packetBatchSender.PacketBatchCollection.Count.Should().Be(1);
	}

	[TestMethod]
	public void AddPacket_AddsMultipleItemsToInternalCollection()
	{
		// Arrange
		PacketBatchSender packetBatchSender = new(Substitute.For<ILogger>());

		// Act
		packetBatchSender.AddPacket(CreateNetPeer(), Substitute.For<INetSerializable>());
		packetBatchSender.AddPacket(CreateNetPeer(), Substitute.For<INetSerializable>());
		packetBatchSender.AddPacket(CreateNetPeer(), Substitute.For<INetSerializable>());

		// Assert
		packetBatchSender.PacketBatchCollection.Count.Should().Be(3);
	}

	#endregion

	#region SendBatch Tests

	[TestMethod]
	public void SendBatch_RemovesItemFromInternalCollection()
	{
		// Arrange
		PacketBatchSender packetBatchSender = new(Substitute.For<ILogger>());
		
		// Act
		packetBatchSender.AddPacket(CreateNetPeer(), Substitute.For<INetSerializable>());
		packetBatchSender.SendBatch();
		
		// Assert
		packetBatchSender.PacketBatchCollection.Count.Should().Be(0);
	}

	#endregion

	#region SendPacketNow Tests

	[TestMethod]
	public void SendPacketNow_PacketGetsSerialized()
	{
		// Arrange
		INetSerializable packet = Substitute.For<INetSerializable>();
		PacketBatchSender packetBatchSender = new(Substitute.For<ILogger>());

		// Act
		packetBatchSender.SendPacketNow(CreateNetPeer(), packet);

		// Assert
		packet.Received(1).Serialize(Arg.Any<NetDataWriter>());
	}

	#endregion

	#region ClearBatch Tests

	[TestMethod]
	public void ClearBatch_ClearsOutInternalCollection()
	{
		// Arrange
		PacketBatchSender packetBatchSender = new(Substitute.For<ILogger>());
		
		// Act
		packetBatchSender.AddPacket(CreateNetPeer(), Substitute.For<INetSerializable>());
		packetBatchSender.ClearBatch(); // Cleared instead of sending
		
		// Assert
		packetBatchSender.PacketBatchCollection.Count.Should().Be(0);
	}

	#endregion

	private static NetPeer CreateNetPeer()
	{
		ConstructorInfo constructor = typeof(NetPeer)
			.GetConstructors(BindingFlags.NonPublic | BindingFlags.Instance)[0];

		NetManager netManager = new(null);
		IPEndPoint ipEndPoint = new(IPAddress.Any, 1000);
		const int id = 100;
		
		NetPeer netPeer = (NetPeer)constructor.Invoke(new object[] {netManager, ipEndPoint, id});
		
		return netPeer;
	}
}