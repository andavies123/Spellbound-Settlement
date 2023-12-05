﻿using System;
using System.Net;
using Andavies.MonoGame.Network.Client;
using Andavies.MonoGame.Network.Server;
using Andavies.SpellboundSettlement.Inputs;
using Andavies.SpellboundSettlement.Server;

namespace Andavies.SpellboundSettlement.GameStates;

public class LoadGameState : GameState
{
	private readonly IServerStarter _serverStarter;
	private readonly INetworkClient _networkClient;

	public LoadGameState(IServerStarter serverStarter, INetworkClient networkClient)
	{
		_serverStarter = serverStarter;
		_networkClient = networkClient;
	}

	public event Action GameLoaded;
	public event Action UnableToLoadGame;

	public override IInputState InputState { get; } = null;

	public override void Start()
	{
		base.Start();
		
		// Initialize server
		string arguments = ServerCommandLineUtility.BuildArgs(IPAddress.Any, 5678, "New World");
		_serverStarter.StartServer(arguments, "Andavies.SpellboundSettlement.Server");

		_networkClient.Start();
	}

	public override void Update(float deltaTimeSeconds)
	{
		base.Update(deltaTimeSeconds);

		while (!_networkClient.IsConnected)
		{
			_networkClient.TryConnect("localhost", 5678);
		}
		
		if (_networkClient.IsConnected)
			GameLoaded?.Invoke();
		else
			UnableToLoadGame?.Invoke();
	}
}