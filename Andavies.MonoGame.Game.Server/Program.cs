using Andavies.MonoGame.Game.Server.Interfaces;
using Autofac;

namespace Andavies.MonoGame.Game.Server;

public static class Program
{
	private static IContainer? Container { get; set; }
	
	private static void Main(string[] args)
	{
		// Init Autofac
		ContainerBuilder builder = new();
		RegisterTypes(builder);
		Container = builder.Build();
		
		using ILifetimeScope scope = Container.BeginLifetimeScope();
		INetworkServer networkServer = Container.Resolve<INetworkServer>();
		networkServer.Start(9580, 10);
	}

	private static void RegisterTypes(ContainerBuilder container)
	{
		container.RegisterType<NetworkServer>().As<INetworkServer>().SingleInstance();
	}
}