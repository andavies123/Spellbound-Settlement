using Andavies.MonoGame.Network.Extensions;
using Andavies.MonoGame.Utilities;
using LiteNetLib.Utils;

namespace Andavies.SpellboundSettlement.Wizards;

public abstract class Wizard : INetSerializable
{
	public Guid Id { get; private set; } = Guid.NewGuid();
	public string Name { get; set; } = "Andrew Davies";
	
	public Vector3Int WorldPosition { get; set; }
	
	public virtual void Serialize(NetDataWriter writer)
	{
		writer.Put(Id.ToString());
		writer.Put(Name);
		writer.Put(WorldPosition);
	}

	public virtual void Deserialize(NetDataReader reader)
	{
		Id = Guid.Parse(reader.GetString());
		Name = reader.GetString();
		WorldPosition = reader.GetVector3Int();
	}
}