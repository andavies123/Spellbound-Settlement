using System.ComponentModel;
using Andavies.MonoGame.Network.Extensions;
using Andavies.MonoGame.Utilities;
using LiteNetLib.Utils;

namespace Andavies.SpellboundSettlement.GameWorld.Wizards;

public abstract class WizardData : INetSerializable, IChangeTracking
{
	private Guid _id = Guid.NewGuid();
	private string _name = "Andrew Davies";
	private float _rotation;
	private Vector3Int _position;
	
	public Guid Id
	{
		get => _id;
		set
		{
			if (_id == value)
				return;
			
			IsChanged = true;
			_id = value;
		}
	}
	
	public string Name
	{
		get => _name;
		set
		{
			if (_name == value)
				return;
			
			IsChanged = true;
			_name = value;
		}
	}

	public float Rotation
	{
		get => _rotation;
		set
		{
			if (Math.Abs(_rotation - value) <= 0.0001f)
				return;
			
			IsChanged = true;
			_rotation = value;
		}
	}

	public Vector3Int Position
	{
		get => _position;
		set
		{
			if (_position == value)
				return;
			
			IsChanged = true; 
			_position = value;
		}
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

	// IChangeTracking Implementation
	public bool IsChanged { get; private set; }
	
	public void AcceptChanges()
	{
		IsChanged = false;
	}
}