namespace Andavies.SpellboundSettlement.Server;

public interface IServerConfigFileManager
{
	/// <summary>Read the config file that is stored on the local computer</summary>
	/// <returns>Returns a ServerSettings object with the serialized data. Default object if no file exists</returns>
	ServerSettings ReadConfigFile();
	
	/// <summary>Saves the given ServerSettings object to the server config file</summary>
	/// <param name="serverSettings">Object that holds all the server settings to be saved</param>
	void SaveConfigFile(ServerSettings serverSettings);
}