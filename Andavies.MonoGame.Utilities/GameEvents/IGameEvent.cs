namespace Andavies.MonoGame.Utilities.GameEvents;

public interface IGameEvent { }

public class WorldCreatedGameEvent : IGameEvent
{
	
}

public class PlayerConnectedGameEvent : IGameEvent
{
	public PlayerConnectedGameEvent(string playerName)
	{
		PlayerName = playerName;
	}

	public string PlayerName { get; }
}

public class PlayerDisconnectedGameEvent : IGameEvent
{
	public PlayerDisconnectedGameEvent(string playerName)
	{
		PlayerName = playerName;
	}
	
	public string PlayerName { get; }
}