using Andavies.MonoGame.Network.Extensions;
using Andavies.MonoGame.Utilities;
using LiteNetLib.Utils;

namespace Andavies.SpellboundSettlement.GameWorld.Wizards;

public abstract class WizardData : ChangeTracker, INetSerializable
{
	private Guid _id = Guid.NewGuid();
	private string _name = "Andrew Davies";
	private float _rotation;
	private Vector3Int _position;
	
	public Guid Id
	{
		get => _id;
		set => SetAndFlagChanged(value, ref _id);
	}
	
	public string Name
	{
		get => _name;
		set => SetAndFlagChanged(value, ref _name);
	}

	public float Rotation
	{
		get => _rotation;
		set => SetAndFlagChanged(value, ref _rotation);
	}

	public Vector3Int Position
	{
		get => _position;
		set => SetAndFlagChanged(value, ref _position);
	}
	
	// INetSerializable Implementation
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