using System.Collections.Concurrent;

namespace Andavies.MonoGame.Utilities.GameEvents;

public class GameEventSystem : IGameEventSystem
{
	private readonly ConcurrentDictionary<Type, List<Delegate>> _subscriptions = new();

	public void Subscribe<T>(Action<T> callback) where T : IGameEvent
	{
		if (!_subscriptions.TryGetValue(typeof(T), out List<Delegate>? callbacks))
		{
			callbacks = new List<Delegate>();
			_subscriptions[typeof(T)] = callbacks;
		}
		
		callbacks.Add(callback);
	}

	public void Publish<T>(T gameEvent) where T : IGameEvent
	{
		if (!_subscriptions.TryGetValue(typeof(T), out List<Delegate>? callbacks))
			return;

		callbacks.ForEach(callback =>
		{
			if (callback is Action<T> genericCallback)
				genericCallback.Invoke(gameEvent);
		});
	}
}