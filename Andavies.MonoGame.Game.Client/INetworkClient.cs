namespace Andavies.MonoGame.Game.Client;

public interface INetworkClient
{
	bool IsConnected { get; }
	
	void Start();
	void Update();
	void Stop();
	void TryConnect();
}