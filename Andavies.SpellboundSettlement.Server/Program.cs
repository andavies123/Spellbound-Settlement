using System.Net;
using Andavies.MonoGame.Network.Server;
using Andavies.MonoGame.Utilities;
using Autofac;
using AutofacSerilogIntegration;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;

namespace Andavies.SpellboundSettlement.Server;

public static class Program
{
	private static IContainer? Container { get; set; }
	private static ILogger? Logger { get; set; }
	
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
		Logger = Container.Resolve<ILogger>();
		GameServer networkServer = Container.Resolve<GameServer>();

		CommandLineParser commandLineParser = new(Logger);
		commandLineParser.ParseArgs(args);

		commandLineParser.TryGetArg(ServerCommandLineUtility.IpCommandLineArgKey, out string? ip);
		commandLineParser.TryGetArg(ServerCommandLineUtility.PortCommandLineArgKey, out string? port);
		commandLineParser.TryGetArg(ServerCommandLineUtility.WorldNameCommandLineArgKey, out string? worldName);

		if (!IPAddress.TryParse(ip, out IPAddress? ipAddress))
			ipAddress = IPAddress.Any;
		if (port == null || !int.TryParse(port, out int parsedPort))
			parsedPort = 5555;
		
		networkServer.Start(ipAddress, parsedPort, 10);
	}

	private static void RegisterTypes(ContainerBuilder container)
	{
		container.RegisterLogger(); // Registers ILogger
		container.RegisterType<GameServer>().AsSelf().SingleInstance();
		container.RegisterType<NetworkServer>().As<INetworkServer>().SingleInstance();
	}
}