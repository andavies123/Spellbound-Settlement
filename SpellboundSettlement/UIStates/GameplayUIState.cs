﻿using System;
using System.Diagnostics;
using GameUI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpellboundSettlement.UIStates;

public class GameplayUIState : IUIState
{
	private readonly Button _pauseButton;

	public GameplayUIState()
	{
		_pauseButton = new Button(new Point(-20, 20), new Point(75, 25), "PAUSE")
		{
			BackgroundColor = Color.DarkSlateGray,
			BackgroundTexture = GameManager.Texture,
			Font = GameManager.Font,
			LayoutAnchor = LayoutAnchor.TopRight
		};
		
		_pauseButton.CalculateBounds(GameManager.Viewport.Bounds.Size);

		_pauseButton.MouseEntered += () => Console.WriteLine("Mouse Entered");
		_pauseButton.MouseExited += () => Console.WriteLine("Mouse Exited");
		_pauseButton.ButtonClicked += () => Console.WriteLine("Button Clicked");
	}

	public void Update()
	{
		_pauseButton.CheckMouseEvents();
	}
    
	public void Draw(SpriteBatch spriteBatch)
	{
		_pauseButton.Draw(spriteBatch);
	}
}