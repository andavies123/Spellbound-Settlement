using Autofac;
using Microsoft.Xna.Framework;
using SpellboundSettlement.CameraObjects;
using SpellboundSettlement.Inputs;

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
		
		// State Machines
		builder.RegisterType<InputStateMachine>().As<IInputStateMachine>().AsSelf().SingleInstance();
		
		// Input Managers
		builder.RegisterType<GameplayInputManager>().As<IInputManager>().AsSelf().SingleInstance();
		
		// Camera Controllers
		builder.RegisterType<WorldViewCameraController>().As<ICameraController>().AsSelf().SingleInstance();
	}
}