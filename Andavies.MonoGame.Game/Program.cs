using Autofac;
using Microsoft.Xna.Framework;
using SpellboundSettlement.CameraObjects;
using SpellboundSettlement.GameStates;
using SpellboundSettlement.Inputs;
using SpellboundSettlement.UIStates;
using Andavies.MonoGame.UI.StateMachines;

namespace SpellboundSettlement;

public static class Program
{
	public static IContainer Container { get; set; }
	
	public static void Main(string[] args)
	{
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
		builder.RegisterType<GameManager>().As<Game>().AsSelf().SingleInstance();
		builder.RegisterType<Camera>().AsSelf().SingleInstance();

		// Game States
		builder.RegisterType<GameStateManager>().As<IGameStateManager>().SingleInstance();
		builder.RegisterType<MainMenuGameState>().As<IGameState>().AsSelf().SingleInstance();
		builder.RegisterType<GameplayGameState>().As<IGameState>().AsSelf().SingleInstance();
		builder.RegisterType<PauseMenuGameState>().As<IGameState>().AsSelf().SingleInstance();
		
		// State Machines
		builder.RegisterType<InputStateMachine>().As<IInputStateMachine>().AsSelf().SingleInstance();
		builder.RegisterType<UIStateMachine>().As<IUIStateMachine>().AsSelf().SingleInstance();
		
		// Input Managers
		builder.RegisterType<GameplayInputManager>().As<IInputManager>().AsSelf().SingleInstance();
		builder.RegisterType<PauseMenuInputManager>().As<IInputManager>().AsSelf().SingleInstance();
		
		// UI States
		builder.RegisterType<MainMenuUIState>().As<IUIState>().AsSelf().SingleInstance();
		builder.RegisterType<GameplayUIState>().As<IUIState>().AsSelf().SingleInstance();
		builder.RegisterType<PauseMenuUIState>().As<IUIState>().AsSelf().SingleInstance();
		
		// Camera Controllers
		builder.RegisterType<WorldViewCameraController>().As<ICameraController>().AsSelf().SingleInstance();
	}
}