using Autofac;
using SpellboundSettlement.Inputs;

namespace SpellboundSettlement;

public static class Program
{
	private static IContainer Container { get; set; }
	
	public static void Main(string[] args)
	{
		InitializeAutoFac();

		using ILifetimeScope scope = Container.BeginLifetimeScope();
		
		GameManager gameManager = Container.Resolve<GameManager>();
		gameManager.Run();
	}

	private static void InitializeAutoFac()
	{
		ContainerBuilder builder = new();
		
		// Register types here
		builder.RegisterType<GameManager>().AsSelf();
		builder.RegisterType<GameplayInputManager>().As<IInputManager>().AsSelf();
		
		Container = builder.Build();
	}
}