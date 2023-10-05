﻿using Microsoft.Xna.Framework.Graphics;
using SpellboundSettlement.Inputs;
using UI.StateMachines;

namespace SpellboundSettlement.GameStates;

public interface IGameState
{
	IUIState UIState { get; }
	IInputManager InputState { get; }

	void Init();
	void LateInit();
	void Start();
	void Update(float deltaTimeSeconds);
	void Draw(SpriteBatch spriteBatch);
	void End();
}