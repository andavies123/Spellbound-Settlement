using System;
using Andavies.SpellboundSettlement.Server;
using Andavies.MonoGame.Network.Client;
using Andavies.SpellboundSettlement.Inputs;
using Andavies.SpellboundSettlement.Server.Interfaces;

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

	public override IInputManager InputState { get; } = null;

	public override void Start()
	{
		base.Start();
		
		// Initialize server
		_serverStarter.StartServer("localhost");

		_networkClient.Start();
	}

	public override void Update(float deltaTimeSeconds)
	{
		base.Update(deltaTimeSeconds);

		while (!_networkClient.IsConnected)
		{
			_networkClient.TryConnect();
		}
		
		if (_networkClient.IsConnected)
			GameLoaded?.Invoke();
		else
			UnableToLoadGame?.Invoke();
	}
}