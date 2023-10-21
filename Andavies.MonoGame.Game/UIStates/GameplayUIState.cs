﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Andavies.MonoGame.UI.Enums;
using Andavies.MonoGame.UI.StateMachines;
using Andavies.MonoGame.UI.Styles;
using Andavies.MonoGame.UI.UIElements;

namespace SpellboundSettlement.UIStates;

public class GameplayUIState : IUIState
{
	private readonly ButtonBuilder _buttonBuilder = new();
	
	private Button _pauseButton;

	public event Action PauseButtonPressed;

	public void Init() { }

	public void LateInit()
	{
		ButtonStyle buttonStyle = new()
		{
			Font = GameManager.Font,
			BackgroundColor = Color.LightSlateGray,
			HoverBackgroundColor = Color.SlateGray,
			MousePressedBackgroundColor = Color.DarkSlateGray,
			DisabledBackgroundColor = Color.Red,
			BackgroundTexture = GameManager.Texture
		};

		_pauseButton = _buttonBuilder
			.SetText("Pause")
			.SetStyle(buttonStyle)
			.SetPositionAndSize(new Point(-20, 20), new Point(75, 25))
			.SetLayoutAnchor(LayoutAnchor.TopRight)
			.Build();
		
		_pauseButton.CalculateBounds(GameManager.Viewport.Bounds.Size);
		_pauseButton.MousePressed += RaisePauseButtonPressed;
	}

	public void Update(float deltaTimeSeconds)
	{
		_pauseButton.Update();
	}
    
	public void Draw(SpriteBatch spriteBatch)
	{
		_pauseButton.Draw(spriteBatch);
	}

	private void RaisePauseButtonPressed() => PauseButtonPressed?.Invoke();
}