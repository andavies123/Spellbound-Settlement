namespace Andavies.MonoGame.Utilities.GameEvents;

public interface IGameEventSystem
{
	void Publish<T>(T gameEvent) where T : IGameEvent;
	void Subscribe<T>(Action<T> callback) where T : IGameEvent;
}