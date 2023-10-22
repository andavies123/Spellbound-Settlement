namespace Andavies.MonoGame.Game.Server;

public static class Program
{
	private static void Main(string[] args)
	{
		LiteNetServer server = new(10);
		
		server.Start(9580);
	}
}