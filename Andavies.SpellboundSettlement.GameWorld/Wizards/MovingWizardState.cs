﻿using Andavies.MonoGame.Utilities;

namespace Andavies.SpellboundSettlement.GameWorld.Wizards;

public class MovingWizardState : WizardState
{
	private List<(Vector3Int worldPosition, float rotation)> _path = new();
	private int _pathIndex = 0;
	private float _movementTime;
	
	public MovingWizardState(Wizard wizard) : base(wizard) { }

	public event Action? ReachedDestination;

	public World? World { get; set; }
	public Vector3Int MoveToPosition { get; set; }

	public override void Begin()
	{
		_path = World?.GeneratePath(Wizard.Data.Position, MoveToPosition) ?? new List<(Vector3Int, float)>();
		_pathIndex = 0;
	}

	public override void Update(float deltaTime)
	{
		_movementTime += deltaTime;

		if (_movementTime < 1/Wizard.Stats.Speed)
			return;

		_movementTime = 0;
		
		if (_pathIndex == _path.Count)
		{
			ReachedDestination?.Invoke();
			return;
		}

		Wizard.Data.Position = _path[_pathIndex].worldPosition;
		Wizard.Data.Rotation = _path[_pathIndex].rotation;
		_pathIndex++;
	}
}