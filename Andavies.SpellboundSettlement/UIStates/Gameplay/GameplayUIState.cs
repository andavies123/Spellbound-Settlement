using System;
using Andavies.MonoGame.Inputs;
using Andavies.MonoGame.UI.Interfaces;
using Andavies.MonoGame.UI.StateMachines;
using Andavies.MonoGame.UI.UIElements;
using Andavies.SpellboundSettlement.Globals;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Andavies.SpellboundSettlement.UIStates.Gameplay;

public class GameplayUIState : IUIState
{
	private readonly IInputManager _inputManager;
	private readonly IUIStyleRepository _uiStyleCollection;
	private Button _pauseButton;
	
	public GameplayUIState(IInputManager inputManager, IUIStyleRepository uiStyleCollection)
	{
		_inputManager = inputManager ?? throw new ArgumentNullException(nameof(inputManager));
		_uiStyleCollection = uiStyleCollection ?? throw new ArgumentNullException(nameof(uiStyleCollection));
	}

	public event Action PauseButtonClicked;

	public void Init() { }

	public void LateInit()
	{
		_pauseButton = new Button(_inputManager, new Point(GameManager.Viewport.Width - 100, 50), new Point(75, 25), "Pause", _uiStyleCollection.DefaultButtonStyle);
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