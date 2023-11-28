using Andavies.MonoGame.Network.Server;
using Autofac;
using AutofacSerilogIntegration;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;

namespace Andavies.SpellboundSettlement.Server;

public static class Program
{
	private static IContainer? Container { get; set; }
	
	private static void Main(string[] args)
	{
        // Init logger
        Log.Logger = new LoggerConfiguration()
	        .Enrich.WithProperty("SourceContext", null)
	        .MinimumLevel.Verbose()
	        .WriteTo.Console(
		        outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {Message} ({SourceContext:l}){NewLine}{Exception}", 
		        theme: AnsiConsoleTheme.Code)
	        .CreateLogger();
        
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
		container.RegisterLogger(); // Registers ILogger
		container.RegisterType<NetworkServer>().As<INetworkServer>().SingleInstance();
	}
}