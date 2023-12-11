namespace Andavies.MonoGame.Network.Server;

public interface IServerAccessManager
{
	public bool WhiteListEnabled { get; set; }
	public bool BlackListEnabled { get; set; }
	public IReadOnlySet<string> WhiteList { get; }
	public IReadOnlySet<string> BlackList { get; }
	
	public void AddToWhiteList(string userName);
	public void AddToBlackList(string userName);
	public void RemoveFromWhiteList(string userName);
	public void RemoveFromBlackList(string userName);
	public void ClearWhiteList();
	public void ClearBlackList();
	public bool IsAllowed(string userName);
}