using Andavies.MonoGame.Network.Extensions;
using Andavies.MonoGame.Utilities;
using LiteNetLib.Utils;

namespace Andavies.SpellboundSettlement.Wizards;

public abstract class Wizard : INetSerializable
{
	public Guid Id { get; private set; } = Guid.NewGuid();
	public string Name { get; set; } = "Andrew Davies";
	
	/// <summary>
	/// The current world position of this wizard.
	/// This value is in world coordinates
	/// </summary>
	public Vector3Int Position { get; set; }
	
	/// <summary>
	/// The current world rotation of this wizard.
	/// This value is in radians.
	/// 0     => Facing positive X
	/// PI/2  => Facing positive Z
	/// PI    => Facing negative X
	/// 3PI/2 => Facing negative Z
	/// </summary>
	public float Rotation { get; set; }

	public virtual void Serialize(NetDataWriter writer)
	{
		writer.Put(Id.ToString());
		writer.Put(Name);
		writer.Put(Position);
		writer.Put(Rotation);
	}

	public virtual void Deserialize(NetDataReader reader)
	{
		Id = Guid.Parse(reader.GetString());
		Name = reader.GetString();
		Position = reader.GetVector3Int();
		Rotation = reader.GetFloat();
	}
}