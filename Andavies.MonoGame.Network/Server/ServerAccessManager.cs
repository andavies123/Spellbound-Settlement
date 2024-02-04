using Serilog;

namespace Andavies.MonoGame.Network.Server;

public class ServerAccessManager : IServerAccessManager
{
	private readonly ILogger _logger;
	private readonly HashSet<string> _whiteList = new();
	private readonly HashSet<string> _blackList = new();

	public ServerAccessManager(ILogger logger)
	{
		_logger = logger ?? throw new ArgumentNullException(nameof(logger));
	}

	public bool WhiteListEnabled { get; set; } = false;
	public bool BlackListEnabled { get; set; } = false;
	public IReadOnlySet<string> WhiteList => _whiteList;
	public IReadOnlySet<string> BlackList => _blackList;

	public void AddToWhiteList(string userName)
	{
		_logger.Information("Adding {user} to server whitelist", userName);
		_whiteList.Add(userName);
	}
	
	public void AddToBlackList(string userName)
	{
		_logger.Information("Adding {user} to server blacklist", userName);
		_blackList.Add(userName);
	}

	public void RemoveFromWhiteList(string userName)
	{
		_logger.Information("Removing {user} from server whitelist", userName);
		_whiteList.Remove(userName);
	}

	public void RemoveFromBlackList(string userName)
	{
		_logger.Information("Removing {user} from server blacklist", userName);
		_blackList.Remove(userName);
	}

	public void ClearWhiteList()
	{
		_logger.Information("Clearing all {userCount} user(s) from the whitelist", _whiteList.Count);
		_whiteList.Clear();
	}

	public void ClearBlackList()
	{
		_logger.Information("Clearing all {userCount} user(s) from the blacklist", _blackList.Count);
		_blackList.Clear();
	}

	public bool IsAllowed(string userName)
	{
		bool allowed = true;
		if (WhiteListEnabled)
			allowed = _whiteList.Contains(userName);
		if (BlackListEnabled)
			allowed = !_blackList.Contains(userName);
		return allowed;
	}
}