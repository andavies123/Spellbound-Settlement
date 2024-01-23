using Andavies.MonoGame.Network.Extensions;
using Andavies.MonoGame.Network.Server;
using Andavies.MonoGame.Network.Utilities;
using Andavies.MonoGame.Utilities;
using Andavies.MonoGame.Utilities.GameEvents;
using Andavies.SpellboundSettlement.GameWorld;
using Andavies.SpellboundSettlement.GameWorld.Repositories;
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
		GameServer gameServer = Container.Resolve<GameServer>();

		// Read Server Settings
		IServerConfigFileManager configManager = Container.Resolve<IServerConfigFileManager>();
		ServerSettings serverSettings = configManager.ReadConfigFile();
		CommandLineParser commandLineParser = new(Container.Resolve<ILogger>());
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
		container.RegisterType<GameEventSystem>().As<IGameEventSystem>().SingleInstance();

		container.RegisterType<WorldManager>().As<IWorldManager>().SingleInstance();
		container.RegisterType<World>().SingleInstance();
		container.RegisterType<TileRegistry>().As<ITileRegistry>().SingleInstance();
		container.RegisterType<TileRegister>().As<ITileRegister>().SingleInstance();
		container.RegisterType<WizardManager>().As<IWizardManager>().SingleInstance();
		container.RegisterType<GameEventListener>().As<IGameEventListener>().SingleInstance();
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