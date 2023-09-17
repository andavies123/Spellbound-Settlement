using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpellboundSettlement.Global;
using SpellboundSettlement.Inputs;

namespace SpellboundSettlement;

public class MainGame : Game
{
	private readonly GraphicsDeviceManager _graphics;
	private readonly GameplayInputManager _gameplayInput = new();
	private SpriteBatch _spriteBatch;

	public MainGame()
	{
		_graphics = new GraphicsDeviceManager(this);
		Content.RootDirectory = "Content";
		IsMouseVisible = true;

		_graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
		_graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
		_graphics.IsFullScreen = false;
		_graphics.ApplyChanges();
	}

	protected override void Initialize()
	{
		GameServices.AddService(this);
		GameServices.AddService(_graphics);
		GameServices.AddService(_gameplayInput);

		base.Initialize();
	}

	protected override void LoadContent()
	{
		_spriteBatch = new SpriteBatch(GraphicsDevice);

		// TODO: use this.Content to load your game content here
	}

	protected override void Update(GameTime gameTime)
	{
		_gameplayInput.UpdateInput();
		// if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
		// 	Exit();

		// TODO: Add your update logic here

		base.Update(gameTime);
	}

	protected override void Draw(GameTime gameTime)
	{
		GraphicsDevice.Clear(Color.CornflowerBlue);
		
		DrawSquare();

		// TODO: Add your drawing code here

		base.Draw(gameTime);
	}

	private void DrawSquare()
	{
		Matrix worldMatrix = Matrix.CreateTranslation(new Vector3(0, 0, 0));
		Matrix viewMatrix = Matrix.CreateLookAt(new Vector3(0, 10, 10), Vector3.Zero, Vector3.Up);
		Matrix projectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, GraphicsDevice.Viewport.AspectRatio, 1f, 100f);
		Effect effect = Content.Load<Effect>("TestShader");
		
		VertexPositionColor[] vertices =
		{
			new(new Vector3(0, 0, 0), Color.Aqua),
			new(new Vector3(1, 0, 0), Color.Red),
			new(new Vector3(0, 1, 0), Color.Yellow),
			new(new Vector3(1, 1, 0), Color.Green),
		};

		short[] indices =
		{
			2, 1, 0,
			3, 1, 2
		};

		VertexBuffer vertexBuffer = new(GraphicsDevice, typeof(VertexPositionColor), vertices.Length, BufferUsage.None);
		vertexBuffer.SetData(vertices);

		IndexBuffer indexBuffer = new(GraphicsDevice, IndexElementSize.SixteenBits, indices.Length, BufferUsage.None);
		indexBuffer.SetData(indices);
		
		GraphicsDevice.SetVertexBuffer(vertexBuffer);
		GraphicsDevice.Indices = indexBuffer;
		
		// effect.Parameters["World"].SetValue(worldMatrix);
		// effect.Parameters["View"].SetValue(viewMatrix);
		// effect.Parameters["Projection"].SetValue(projectionMatrix);
		effect.Parameters["WorldViewProjection"].SetValue(worldMatrix * viewMatrix * projectionMatrix);
		
		effect.CurrentTechnique.Passes[0].Apply();
		
		GraphicsDevice.DrawIndexedPrimitives(
			PrimitiveType.TriangleList,
			0,
			0,
			indexBuffer.IndexCount / 3);
	}
}