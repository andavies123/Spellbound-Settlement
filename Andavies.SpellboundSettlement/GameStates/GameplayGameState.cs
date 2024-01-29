using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Andavies.MonoGame.Inputs;
using Andavies.MonoGame.Network.Client;
using Andavies.MonoGame.Utilities;
using Andavies.SpellboundSettlement.GameWorld.Repositories;
using Andavies.SpellboundSettlement.GameWorld.Tiles;
using Andavies.SpellboundSettlement.GameWorld.Wizards;
using Andavies.SpellboundSettlement.Globals;
using Andavies.SpellboundSettlement.Inputs;
using Andavies.SpellboundSettlement.Meshes;
using Andavies.SpellboundSettlement.NetworkMessages.Messages.World;
using Andavies.SpellboundSettlement.UIStates.Gameplay;
using Andavies.SpellboundSettlement.Wizards;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Serilog;

namespace Andavies.SpellboundSettlement.GameStates;

public class GameplayGameState : GameState
{
	private readonly ILogger _logger;
	private readonly INetworkClient _networkClient;
	private readonly ITileRegistry _tileRegistry;
	private readonly IClientWorldManager _clientWorldManager;
	private readonly IChunkDrawManager _chunkDrawManager;
	private readonly IModelDrawManager _modelDrawManager;
	private readonly IWorldInteractionManager _worldInteractionManager;
	private readonly WorldMesh _worldMesh;
	
	private readonly ConcurrentDictionary<Type, WizardDrawDetails> _wizardDrawDetails = new();

	private readonly GameplayUIState _gameplayGameplayUIState;
	
	public GameplayGameState(
		ILogger logger,
		INetworkClient networkClient,
		ITileRegistry tileRegistry,
		IClientWorldManager clientWorldManager,
		IChunkDrawManager chunkDrawManager,
		IModelDrawManager modelDrawManager,
		IWorldInteractionManager worldInteractionManager,
		GameplayUIState gameplayUIState,
		WorldMesh worldMesh,
		GameplayInputState inputState)
	{
		_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		_networkClient = networkClient ?? throw new ArgumentNullException(nameof(networkClient));
		_tileRegistry = tileRegistry ?? throw new ArgumentNullException(nameof(tileRegistry));
		_clientWorldManager = clientWorldManager ?? throw new ArgumentNullException(nameof(clientWorldManager));
		_chunkDrawManager = chunkDrawManager ?? throw new ArgumentNullException(nameof(chunkDrawManager));
		_modelDrawManager = modelDrawManager ?? throw new ArgumentNullException(nameof(modelDrawManager));
		_worldInteractionManager = worldInteractionManager ?? throw new ArgumentNullException(nameof(worldInteractionManager));
		_gameplayGameplayUIState = gameplayUIState ?? throw new ArgumentNullException(nameof(gameplayUIState));
		_worldMesh = worldMesh ?? throw new ArgumentNullException(nameof(worldMesh));
		InputState = inputState ?? throw new ArgumentNullException(nameof(inputState));
		
		UIStates.Add(gameplayUIState);
	}
	
	public event Action PauseGameRequested;

	public override GameplayInputState InputState { get; }

	public override void Start()
	{
		base.Start();
		
		LoadTileModels();
		RegisterWizardDrawDetails();
		
		UIStateMachine.ChangeUIState(_gameplayGameplayUIState);

		List<Vector2Int> chunkPositions = new();
		const int chunkRadius = 5;
		for (int x = 0; x < chunkRadius; x++)
		{
			for (int y = 0; y < chunkRadius; y++)
			{
				chunkPositions.Add(new Vector2Int(x, y));
			}
		}

		_networkClient.SendMessage(new WorldChunkRequestPacket {ChunkPositions = chunkPositions});

		_gameplayGameplayUIState.PauseButtonClicked += OnPauseGameClicked;
		InputState.PauseGame.OnKeyUp += OnPauseGameKeyReleased;
		
		// These connections are made here so WorldInteractionManager doesn't have to worry about when to stop listening
		Input.MouseMoved += _worldInteractionManager.UpdateTileHover;
		Input.LeftMousePressed += _worldInteractionManager.UpdateTileInteract;
	}
	
	public override void Update(float deltaTimeSeconds)
	{
		base.Update(deltaTimeSeconds);
		
		_networkClient.Update();
	}

	public override void Draw3D(GraphicsDevice graphicsDevice)
	{
		base.Draw3D(graphicsDevice);
		
		// Draw world using the IChunkDrawManager
		foreach (ChunkMesh chunkMesh in _worldMesh.ChunkMeshes)
		{
			_chunkDrawManager.DrawChunk(chunkMesh);
		}

		foreach (Wizard wizard in _clientWorldManager.AllWizards.Values)
		{
			DrawWizard(wizard);
		}
	}
	
	public override void End()
	{
		base.End();
		
		_gameplayGameplayUIState.PauseButtonClicked -= OnPauseGameClicked;
		InputState.PauseGame.OnKeyUp -= OnPauseGameKeyReleased;
		
		Input.MouseMoved -= _worldInteractionManager.UpdateTileHover;
		Input.LeftMousePressed -= _worldInteractionManager.UpdateTileInteract;
		
		UnloadTileModels();
	}
	
	private void OnPauseGameKeyReleased() => PauseGameRequested?.Invoke();
	private void OnPauseGameClicked() => PauseGameRequested?.Invoke();

	private void RegisterWizardDrawDetails()
	{
		BasicWizardDrawDetails basicWizardDrawDetails = new();
		
		_wizardDrawDetails.TryAdd(basicWizardDrawDetails.WizardType, basicWizardDrawDetails);
		
		foreach (WizardDrawDetails wizardDrawDetails in _wizardDrawDetails.Values)
		{
			wizardDrawDetails.Model = Global.GameManager.Content.Load<Model>(wizardDrawDetails.ModelDetails.ContentModelPath);
		}
	}

	private void DrawWizard(Wizard wizard)
	{
		if (!_wizardDrawDetails.TryGetValue(wizard.GetType(), out WizardDrawDetails wizardDrawDetails))
		{
			_logger.Warning("Wizard draw details does not exist: {type}", wizard.GetType());
			return;
		}
		
		_modelDrawManager.DrawModel(wizardDrawDetails.Model, wizardDrawDetails.ModelDetails, (Vector3)wizard.Position, 1f, wizard.Rotation);
	}

	private void LoadTileModels()
	{
		foreach (ModelTile modelTile in _tileRegistry.GetAllTilesOfType<ModelTile>())
		{
			modelTile.Model = Global.GameManager.Content.Load<Model>(modelTile.ModelDetails.ContentModelPath);
		}
	}

	private void UnloadTileModels()
	{
		foreach (ModelTile modelTile in _tileRegistry.GetAllTilesOfType<ModelTile>())
		{
			Global.GameManager.Content.UnloadAsset(modelTile.ModelDetails.ContentModelPath);
			modelTile.Model = null;
		}
	}
}