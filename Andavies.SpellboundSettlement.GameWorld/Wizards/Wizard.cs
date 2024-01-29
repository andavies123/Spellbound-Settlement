using Andavies.MonoGame.Network.Extensions;
using Andavies.MonoGame.Utilities;
using Andavies.MonoGame.Utilities.StateMachines;
using LiteNetLib.Utils;
using Microsoft.Xna.Framework;

namespace Andavies.SpellboundSettlement.GameWorld.Wizards;

public abstract class Wizard : INetSerializable
{
	// Todo: Add these constants to a general class 
	public const float North = MathHelper.PiOver2 * 3;
	public const float South = MathHelper.PiOver2;
	public const float East = 0f;
	public const float West = MathHelper.Pi;
	
	private readonly StateMachine<WizardState> _stateMachine = new();
	private readonly LoiterWizardState _loiterState;

	private bool _updated = false;
	
	// Property Backing Fields
	private Vector3Int _position;
	private float _rotation;

	protected Wizard()
	{
		_loiterState = new LoiterWizardState(this);
	}
    
	public event Action<Wizard>? Updated;

	public Guid Id { get; private set; } = Guid.NewGuid();
	public string Name { get; set; } = "Andrew Davies";

	/// <summary>
	/// The current world position of this wizard.
	/// This value is in world coordinates
	/// </summary>
	public Vector3Int Position
	{
		get => _position;
		set
		{
			if (_position == value)
				return;

			_position = value;
			_updated = true;
		}
	}

	/// <summary>
	/// The current world rotation of this wizard.
	/// This value is in radians.
	/// 0     => Facing positive X
	/// PI/2  => Facing positive Z
	/// PI    => Facing negative X
	/// 3PI/2 => Facing negative Z
	/// </summary>
	public float Rotation
	{
		get => _rotation;
		set
		{
			if (Math.Abs(_rotation - value) < float.Epsilon)
				return;

			_rotation = value;
			_updated = true;
		}
	}

	public void Update(float deltaTimeSeconds)
	{
		_stateMachine.UpdateCurrentState(deltaTimeSeconds);
		if (_updated)
		{
			_updated = false;
			Updated?.Invoke(this);
		}
	}

	public void Loiter()
	{
		_loiterState.CenterPosition = Position;
		_loiterState.MinStandingLength = 3;
		_loiterState.MaxStandingLength = 7;
		_stateMachine.SetCurrentState(_loiterState);
	}

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