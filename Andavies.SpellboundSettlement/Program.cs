﻿using Andavies.MonoGame.Inputs;
using Andavies.MonoGame.Inputs.InputListeners;
using Andavies.MonoGame.Network.Client;
using Andavies.MonoGame.Network.Extensions;
using Andavies.MonoGame.Network.Server;
using Andavies.MonoGame.UI.StateMachines;
using Andavies.MonoGame.Utilities;
using Andavies.SpellboundSettlement.CameraObjects;
using Andavies.SpellboundSettlement.GameStates;
using Andavies.SpellboundSettlement.GameWorld;
using Andavies.SpellboundSettlement.GameWorld.Repositories;
using Andavies.SpellboundSettlement.Globals;
using Andavies.SpellboundSettlement.Inputs;
using Andavies.SpellboundSettlement.Meshes;
using Andavies.SpellboundSettlement.Repositories;
using Andavies.SpellboundSettlement.UIStates.Gameplay;
using Andavies.SpellboundSettlement.UIStates.MainMenu;
using Andavies.SpellboundSettlement.UIStates.PauseMenu;
using Autofac;
using Autofac.Core;
using AutofacSerilogIntegration;
using Microsoft.Xna.Framework;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;
using IDrawable = Andavies.MonoGame.Utilities.Interfaces.IDrawable;
using IUpdateable = Andavies.MonoGame.Utilities.Interfaces.IUpdateable;

namespace Andavies.SpellboundSettlement;

public static class Program
{
	// Parameter Names
	private const string InitOrderParameterName = "initOrder";
	private const string UpdateOrderParameterName = "updateOrder";
	private const string DrawOrderParameterName = "drawOrder";
	
	// Init Orders
	private const int GameStateManagerInitOrder = 1;
	private const int InputManagerInitOrder = 2;
	private const int CameraControllerInitOrder = 5;
	
	// Update Orders
	private const int FpsCounterUpdateOrder = -1;
	private const int InputManagerUpdateOrder = 1;
	private const int GameStateManagerUpdateOrder = 2;
	private const int CameraControllerUpdateOrder = 3;
	
	// Draw Orders
	private const int GameStateManagerDrawOrder = 1;
	
	private static IContainer Container { get; set; }
	
	public static void Main(string[] args)
	{
		// Init Logger
		Log.Logger = new LoggerConfiguration()
			.Enrich.WithProperty("SourceContext", null)
			.Enrich.WithComputed("SourceContextClass", "Substring(SourceContext, LastIndexOf(SourceContext, '.') + 1)")
			.MinimumLevel.Verbose()
			.WriteTo.Console(
				outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {Message} ({SourceContextClass:l}){NewLine}{Exception}", 
				theme: AnsiConsoleTheme.Code)
			.CreateLogger();
		
		// Todo: Add argument for setting this value
		NetworkLoggerExtensions.LogPackets = false;
		
		// Init Autofac
		ContainerBuilder builder = new();
		RegisterTypes(builder);
		Container = builder.Build();

		using ILifetimeScope scope = Container.BeginLifetimeScope();
		GameManager gameManager = Container.Resolve<GameManager>();
		gameManager.Run();
	}

	private static void RegisterTypes(ContainerBuilder builder)
	{
		builder.RegisterLogger(); // Registers ILogger
		builder.RegisterType<GameManager>().As<Game>().AsSelf().SingleInstance();
		builder.RegisterType<Camera>().AsSelf().SingleInstance();
		builder.RegisterType<WorldMesh>().AsSelf().SingleInstance();
		builder.RegisterType<ChunkDrawManager>().As<IChunkDrawManager>().SingleInstance();
		builder.RegisterType<ChunkMeshBuilder>().As<IChunkMeshBuilder>().SingleInstance();
		builder.RegisterType<TileMouseHoverHandler>().As<ITileHoverHandler>().SingleInstance();
		builder.RegisterType<ModelDrawManager>().As<IModelDrawManager>().SingleInstance();
		builder.RegisterType<TileRegister>().As<ITileRegister>().SingleInstance();
		builder.RegisterType<ClientWorldManager>().As<IClientWorldManager>().SingleInstance();
		builder.RegisterType<WorldInteractionManager>().As<IWorldInteractionManager>().SingleInstance();

		builder.RegisterType<GameStateManager>()
			.As<IGameStateManager>()
			.As<IGameObject>()
			.As<IUpdateable>()
			.As<IDrawable>()
			.SingleInstance()
			.WithParameters(new Parameter[]
			{
				new NamedParameter(InitOrderParameterName, GameStateManagerInitOrder),
				new NamedParameter(UpdateOrderParameterName, GameStateManagerUpdateOrder),
				new NamedParameter(DrawOrderParameterName, GameStateManagerDrawOrder)
			});
		
		builder.RegisterType<WorldViewCameraController>()
			.As<ICameraController>()
			.As<IGameObject>()
			.As<IUpdateable>()
			.SingleInstance()
			.WithParameters(new Parameter[]
			{
				new NamedParameter(InitOrderParameterName, CameraControllerInitOrder),
				new NamedParameter(UpdateOrderParameterName, CameraControllerUpdateOrder)
			});
		
		builder.RegisterType<InputManager>()
			.As<IInputManager>()
			.As<IGameObject>()
			.As<IUpdateable>()
			.SingleInstance()
			.WithParameters(new Parameter[]
			{
				new NamedParameter(InitOrderParameterName, InputManagerInitOrder),
				new NamedParameter(UpdateOrderParameterName, InputManagerUpdateOrder)
			});
		
		builder.RegisterType<FpsCounter>()
			.As<IUpdateable>()
			.SingleInstance()
			.WithParameter(UpdateOrderParameterName, FpsCounterUpdateOrder);
		
		// Repositories
		builder.RegisterType<TileRegistry>().As<ITileRegistry>().SingleInstance();
		builder.RegisterType<FontRepository>().As<IFontRepository>().SingleInstance();
		
		// Server
		builder.RegisterType<ServerStarter>().As<IServerStarter>().SingleInstance();
		builder.RegisterType<NetworkClient>().As<INetworkClient>().SingleInstance();
		
		// Collections
		builder.RegisterType<UIStyleRepository>().As<IUIStyleRepository>().SingleInstance();

		// Game States
		builder.RegisterType<MainMenuGameState>().As<IGameState>().AsSelf().SingleInstance();
		builder.RegisterType<LoadGameState>().As<IGameState>().AsSelf().SingleInstance();
		builder.RegisterType<GameplayGameState>().As<IGameState>().AsSelf().SingleInstance();
		builder.RegisterType<PauseMenuGameState>().As<IGameState>().AsSelf().SingleInstance();
		
		// State Machines
		builder.RegisterType<InputStateMachine>().As<IInputStateMachine>().AsSelf().SingleInstance();
		builder.RegisterType<UIStateMachine>().As<IUIStateMachine>().AsSelf().SingleInstance();
		
		// Input States
		builder.RegisterType<GameplayInputState>().As<IInputState>().AsSelf().SingleInstance();
		builder.RegisterType<PauseMenuInputState>().As<IInputState>().AsSelf().SingleInstance();
		
		// Text Listeners
		builder.RegisterType<NumberDecimalInputListener>().As<IInputListener>().Keyed<IInputListener>(nameof(NumberDecimalInputListener)).InstancePerDependency();
		
		// UI States
		builder.RegisterType<MainMenuMainUIState>().As<IUIState>().AsSelf().SingleInstance();
		builder.RegisterType<MainMenuPlayUIState>().As<IUIState>().AsSelf().SingleInstance();
		builder.RegisterType<MainMenuNewGameUIState>().As<IUIState>().AsSelf().SingleInstance();
		builder.RegisterType<MainMenuJoinServerUIState>().As<IUIState>().AsSelf().SingleInstance();
		builder.RegisterType<MainMenuCreateServerUIState>().As<IUIState>().AsSelf().SingleInstance();
		builder.RegisterType<MainMenuOptionsUIState>().As<IUIState>().AsSelf().SingleInstance();
		builder.RegisterType<GameplayUIState>().As<IUIState>().AsSelf().SingleInstance();
		builder.RegisterType<PauseMenuUIState>().As<IUIState>().AsSelf().SingleInstance();
	}
}