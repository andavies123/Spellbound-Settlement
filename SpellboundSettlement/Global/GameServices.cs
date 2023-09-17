using Microsoft.Xna.Framework;

namespace SpellboundSettlement.Global;

public static class GameServices
{
	private static GameServiceContainer s_ServiceContainer;

	public static GameServiceContainer Instance => s_ServiceContainer ??= new GameServiceContainer();

	public static void AddService<T>(T service) => Instance.AddService(service);
	public static void RemoveService<T>() => Instance.RemoveService(typeof(T));
	public static T GetService<T>() => (T) Instance.GetService(typeof(T));
}