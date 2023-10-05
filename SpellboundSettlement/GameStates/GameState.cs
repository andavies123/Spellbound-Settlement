using Microsoft.Xna.Framework.Graphics;
using SpellboundSettlement.Inputs;
using UI.StateMachines;

namespace SpellboundSettlement.GameStates;

public abstract class GameState : IGameState
{
	public abstract IUIState UIState { get; }
	public abstract IInputManager InputState { get; }

	/// <summary>
	/// Called when the game first starts. This is only called once per runtime.
	/// Add initialization code that does not involve using loaded contents
	/// </summary>
	public virtual void Init()
	{
		UIState.Init();
	}

	/// <summary>
	/// Called after content is loaded. This is only called once per runtime.
	/// Add initialization code that involves using loaded content
	/// </summary>
	public virtual void LateInit()
	{
		UIState.LateInit();
	}

	/// <summary>
	/// Called when this game state is set as the current state.
	/// This is called multiple times but only when this state is set as the current state
	/// </summary>
	public virtual void Start()
	{
		
	}

	/// <summary>
	/// Called once per frame to update any logic necessary
	/// </summary>
	/// <param name="deltaTimeSeconds"></param>
	public virtual void Update(float deltaTimeSeconds)
	{
		InputState.UpdateInput();
		UIState.Update(deltaTimeSeconds);
	}

	/// <summary>
	/// Called once per frame to draw anything that should be displayed on the screen
	/// </summary>
	/// <param name="spriteBatch">SpriteBatch object used to draw 2D images</param>
	public virtual void Draw(SpriteBatch spriteBatch)
	{
		UIState.Draw(spriteBatch);
	}

	/// <summary>
	/// Called when this game state is being replaced as the current state.
	/// This can be called multiple times but only when this won't be the current state anymore
	/// </summary>
	public virtual void End()
	{
		
	}
}