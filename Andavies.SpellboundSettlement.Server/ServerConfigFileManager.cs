using System.Text.Json;
using Serilog;

namespace Andavies.SpellboundSettlement.Server;

public class ServerConfigFileManager : IServerConfigFileManager
{
	private readonly ILogger _logger;
	private readonly JsonSerializerOptions _jsonSerializerOptions = new() {WriteIndented = true};
	
	public ServerConfigFileManager(ILogger logger)
	{
		_logger = logger ?? throw new ArgumentNullException();
	}

	private static string ConfigFileDirectory => Path.Combine(Environment.SpecialFolder.ApplicationData.ToString(), "Config");
	private static string ConfigFilePath => Path.Combine(ConfigFileDirectory, "settings.json");

	public ServerSettings ReadConfigFile()
	{
		if (!File.Exists(ConfigFilePath))
			CreateBlankConfigFile();

		string jsonString = File.ReadAllText(ConfigFilePath);
		return JsonSerializer.Deserialize<ServerSettings>(jsonString) ?? new ServerSettings();
	}

	public void SaveConfigFile(ServerSettings serverSettings)
	{
		_logger.Information("Saving server config file at {configFilePath}", ConfigFilePath);
		string jsonString = JsonSerializer.Serialize(serverSettings, _jsonSerializerOptions);
		
		if (!Directory.Exists(ConfigFileDirectory))
			Directory.CreateDirectory(ConfigFileDirectory);
		File.WriteAllText(ConfigFilePath, jsonString);
	}

	private void CreateBlankConfigFile()
	{
		SaveConfigFile(new ServerSettings());
	}
}