using Andavies.MonoGame.Network.Extensions;
using Andavies.MonoGame.Network.Server;
using Andavies.MonoGame.Network.Utilities;
using Andavies.MonoGame.Utilities;
using Autofac;
using AutofacSerilogIntegration;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;

namespace Andavies.SpellboundSettlement.Server;

public static class Program
{
	/// <summary>
	/// This logger is used for debugging only.
	/// Rather than bringing in the logger via Dependency Injection just to solve an issue,
	/// The developer can just call this Logger, so its easier to keep track of which log statements
	/// should probably be removed when it is time to release the game.
	/// For consistent log statements, use Dependency Injection to get the logger
	/// </summary>
	public static ILogger Logger { get; private set; } = null!;

	private static IContainer? Container { get; set; }
	
	private static void Main(string[] args)
	{
		// Init logger
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
		Logger = Container.Resolve<ILogger>();
		GameServer gameServer = Container.Resolve<GameServer>();

		// Read Server Settings
		IServerConfigFileManager configManager = Container.Resolve<IServerConfigFileManager>();
		ServerSettings serverSettings = configManager.ReadConfigFile();
		CommandLineParser commandLineParser = new(Logger);
		commandLineParser.ParseArgs(args);
		OverrideServerSettingsWithCommandLineArgs(commandLineParser, serverSettings);
		
		configManager.SaveConfigFile(serverSettings);
		
		gameServer.Start(serverSettings, 10, 50);
	}

	private static void RegisterTypes(ContainerBuilder container)
	{
		container.RegisterLogger(); // Registers ILogger
		
		container.RegisterType<GameServer>().AsSelf().SingleInstance();
		container.RegisterType<NetworkServer>().As<INetworkServer>().SingleInstance();
		container.RegisterType<PacketBatchSender>().As<IPacketBatchSender>().SingleInstance();
		container.RegisterType<ServerConfigFileManager>().As<IServerConfigFileManager>().SingleInstance();
		container.RegisterType<ServerAccessManager>().As<IServerAccessManager>().SingleInstance();
	}

	private static void OverrideServerSettingsWithCommandLineArgs(CommandLineParser parser, ServerSettings serverSettings)
	{
		if (parser.TryGetArg(ServerCommandLineUtility.IpCommandLineArgKey, out string? ip) && ip != null)
			serverSettings.IpAddress = ip;

		if (parser.TryGetArg(ServerCommandLineUtility.PortCommandLineArgKey, out string? port) && port != null)
			serverSettings.Port = port;
		
		if (parser.TryGetArg(ServerCommandLineUtility.LocalOnlyCommandLineArgKey, out string? _))
			serverSettings.IsLocalOnly = true;
	}
}