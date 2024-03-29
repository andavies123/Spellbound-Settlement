﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Andavies.MonoGame.Network.Client;
using Andavies.MonoGame.Utilities;
using Andavies.SpellboundSettlement.GameWorld;
using Andavies.SpellboundSettlement.GameWorld.Wizards;
using Andavies.SpellboundSettlement.Meshes;
using Andavies.SpellboundSettlement.NetworkMessages.Messages.World;
using LiteNetLib.Utils;

namespace Andavies.SpellboundSettlement.GameStates;

public class ClientWorldManager : IClientWorldManager
{
	private readonly INetworkClient _networkClient;
	private readonly IChunkMeshBuilder _chunkMeshBuilder;
	private readonly WorldMesh _worldMesh;
	
	private readonly ConcurrentDictionary<Vector2Int, ChunkData> _chunks = new();
	private readonly ConcurrentDictionary<Guid, WizardData> _wizards = new();

	public ClientWorldManager(
		INetworkClient networkClient,
		IChunkMeshBuilder chunkMeshBuilder,
		WorldMesh worldMesh)
	{
		_networkClient = networkClient ?? throw new ArgumentNullException(nameof(networkClient));
		_chunkMeshBuilder = chunkMeshBuilder ?? throw new ArgumentNullException(nameof(chunkMeshBuilder));
		_worldMesh = worldMesh ?? throw new ArgumentNullException(nameof(worldMesh));
		
		SubscribeToServerMessages();
	}
	
	public IReadOnlyDictionary<Vector2Int, ChunkData> AllChunks => _chunks;
	public IReadOnlyDictionary<Guid, WizardData> AllWizards => _wizards;
	
	private void SubscribeToServerMessages()
	{
		_networkClient.AddSubscription<WizardUpdatedPacket>(OnWizardUpdatedPacketReceived);
		_networkClient.AddSubscription<WorldChunkResponsePacket>(OnWorldChunkResponsePacketReceived);
	}
	
	private void OnWorldChunkResponsePacketReceived(INetSerializable packet)
	{
		if (packet is not WorldChunkResponsePacket worldChunkResponsePacket)
			return;

		if (worldChunkResponsePacket.ChunkData == null)
			return;

		_chunks[worldChunkResponsePacket.ChunkData.ChunkPosition] = worldChunkResponsePacket.ChunkData;
		
		ChunkMesh chunkMesh = _chunkMeshBuilder.BuildChunkMesh(worldChunkResponsePacket.ChunkData);
		_worldMesh.SetChunkMesh(chunkMesh, worldChunkResponsePacket.ChunkData.ChunkPosition);
	}

	private void OnWizardUpdatedPacketReceived(INetSerializable packet)
	{
		if (packet is not WizardUpdatedPacket wizardUpdatedPacket || wizardUpdatedPacket.WizardData == null)
			return;

		_wizards[wizardUpdatedPacket.WizardData.Id] = wizardUpdatedPacket.WizardData;
	}
}