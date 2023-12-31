﻿using Microsoft.Xna.Framework.Graphics;

namespace Andavies.MonoGame.UI.StateMachines;

public interface IUIState
{
	void Init();
	void LateInit();
	void Start();
	void Update(float deltaTimeSeconds);
	void Draw(SpriteBatch spriteBatch);
	void Exit();
}