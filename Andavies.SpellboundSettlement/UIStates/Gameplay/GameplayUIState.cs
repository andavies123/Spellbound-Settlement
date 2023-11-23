using System;
using Andavies.MonoGame.UI.Interfaces;
using Andavies.MonoGame.UI.StateMachines;
using Andavies.MonoGame.UI.UIElements;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpellboundSettlement.Globals;

namespace SpellboundSettlement.UIStates.Gameplay;

public class GameplayUIState : IUIState
{
	private readonly IUIStyleRepository _uiStyleCollection;
	private Button _pauseButton;
	
	public GameplayUIState(IUIStyleRepository uiStyleCollection)
	{
		_uiStyleCollection = uiStyleCollection;
	}

	public event Action PauseButtonClicked;

	public void Init() { }

	public void LateInit()
	{
		_pauseButton = new Button(new Point(GameManager.Viewport.Width - 100, 50), new Point(75, 25), "Pause", _uiStyleCollection.DefaultButtonStyle);
	}

	public void Start()
	{
		_pauseButton.MouseClicked += OnPauseButtonMouseClicked;
	}

	public void Update(float deltaTimeSeconds)
	{
		_pauseButton.Update(deltaTimeSeconds);
	}
    
	public void Draw(SpriteBatch spriteBatch)
	{
		_pauseButton.Draw(spriteBatch);
	}

	public void Exit()
	{
		_pauseButton.MouseClicked -= OnPauseButtonMouseClicked;
	}
	
	private void OnPauseButtonMouseClicked(IUIElement _) => PauseButtonClicked?.Invoke();
}