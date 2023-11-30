using System.Net;
using Andavies.MonoGame.Network.Server;
using Autofac;
using AutofacSerilogIntegration;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;
using static System.Int32;

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
		INetworkServer networkServer = Container.Resolve<INetworkServer>();

		TryParseArgs(args, out IPAddress? ipAddress, out int port, out string worldName);

		if (ipAddress == null)
			ipAddress = IPAddress.Any;
		
		networkServer.Start(ipAddress, port, 10);
	}

	private static void RegisterTypes(ContainerBuilder container)
	{
		container.RegisterLogger(); // Registers ILogger
		container.RegisterType<NetworkServer>().As<INetworkServer>().SingleInstance();
	}

	private static bool TryParseArgs(string[] args, out IPAddress? ipAddress, out int port, out string worldName)
	{
		ipAddress = IPAddress.Any;
		port = 0;
		worldName = string.Empty;
		
		if (args.Length != 3)
			return false;
		
		// IPAddress
		if (!IPAddress.TryParse(args[0], out ipAddress))
			ipAddress = IPAddress.Any;
		
		// Port
		if (!TryParse(args[1], out port))
			Logger?.Warning("Unable to parse port argument to int. {portArg}", args[1]);
		
		// WorldName
		worldName = args[2];
		
		return true;
	}
}