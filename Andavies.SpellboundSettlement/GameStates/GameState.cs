using System.Collections.Generic;
using Andavies.MonoGame.UI.StateMachines;
using Andavies.SpellboundSettlement.Inputs;
using Microsoft.Xna.Framework.Graphics;

namespace Andavies.SpellboundSettlement.GameStates;

public abstract class GameState : IGameState
{
	// The current input state
	public abstract IInputManager InputState { get; }
	
	// State Machine for to handle switching between related UI States
	public IUIStateMachine UIStateMachine { get; } = new UIStateMachine();

	// A collection of UIStates used to easily initialize them all in the parent class
	protected List<IUIState> UIStates { get; } = new();

	// Called when the game first starts. This is only called once per runtime
	// Add initialization code that does not involve using loaded contents
	public virtual void Init()
	{
		UIStates.ForEach(uiState => uiState.Init());
	}
	
	// Called after content is loaded. This is only called once per runtime
	// Add initialization code that involves using loaded content
	public virtual void LateInit()
	{
		UIStates.ForEach(uiState => uiState.LateInit());
	}

	// Called when this game state is set as the current state
	// This is called multiple times but only when this state is set as the current state
	public virtual void Start() { }

	// Called once per frame to update any logic
	public virtual void Update(float deltaTimeSeconds)
	{
		InputState?.UpdateInput();
		UIStateMachine.Update(deltaTimeSeconds);
	}

	// Called once per frame to draw any 3d meshes
	// Called before DrawUI
	public virtual void Draw3D(GraphicsDevice graphicsDevice) { }

	// Called once per frame to draw any UI objects
	// Called after Draw3D
	public virtual void DrawUI(SpriteBatch spriteBatch)
	{
		UIStateMachine.Draw(spriteBatch);
	}

	// Called when this game state is being replaced by another game state
	// This can be called multiple times but only when changing states
	public virtual void End() { }
}