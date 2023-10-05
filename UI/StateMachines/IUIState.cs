﻿using Microsoft.Xna.Framework.Graphics;

namespace UI.StateMachines;

public interface IUIState
{
	void Init();
	void LateInit();
	void Update(float deltaTimeSeconds);
	void Draw(SpriteBatch spriteBatch);
}