using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpellboundSettlement.Global;
using SpellboundSettlement.Inputs;
using SpellboundSettlement.Meshes;

namespace SpellboundSettlement;

public class MainGame : Game
{
	private readonly GraphicsDeviceManager _graphics;
	private readonly GameplayInputManager _gameplayInput = new();
	
	private SpriteBatch _spriteBatch;
	private Matrix _worldMatrix;
	private Matrix _viewMatrix;
	private Matrix _projectionMatrix;
	private Effect _effect;

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
		
		_worldMatrix = Matrix.CreateTranslation(new Vector3(0, 0, 0));
		_viewMatrix = Matrix.CreateLookAt(new Vector3(5, 5, 10), Vector3.Zero, Vector3.Up);
		_projectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, GraphicsDevice.Viewport.AspectRatio, 1f, 100f);
		_effect = Content.Load<Effect>("TestShader");

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
		CubeMesh cubeMesh = new(Vector3.Zero);
		VertexPositionColor[] vertices = cubeMesh.Vertices.ToArray();
		int[] indices = cubeMesh.Indices.ToArray();

		VertexBuffer vertexBuffer = new(GraphicsDevice, typeof(VertexPositionColor), vertices.Length, BufferUsage.None);
		vertexBuffer.SetData(vertices);

		IndexBuffer indexBuffer = new(GraphicsDevice, IndexElementSize.ThirtyTwoBits, indices.Length, BufferUsage.None);
		indexBuffer.SetData(indices);
		
		GraphicsDevice.SetVertexBuffer(vertexBuffer);
		GraphicsDevice.Indices = indexBuffer;
		
		_effect.Parameters["WorldViewProjection"].SetValue(_worldMatrix * _viewMatrix * _projectionMatrix);
		_effect.CurrentTechnique.Passes[0].Apply();
		
		GraphicsDevice.DrawIndexedPrimitives(
			PrimitiveType.TriangleList,
			0,
			0,
			indexBuffer.IndexCount / 3);
	}
}