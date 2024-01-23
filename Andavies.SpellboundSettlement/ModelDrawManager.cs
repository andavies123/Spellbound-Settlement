using System;
using Andavies.MonoGame.Utilities;
using Andavies.SpellboundSettlement.CameraObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Serilog;

namespace Andavies.SpellboundSettlement;

public class ModelDrawManager : IModelDrawManager
{
	private readonly ILogger _logger;
	private readonly Camera _camera;
	
	public ModelDrawManager(ILogger logger, Camera camera)
	{
		_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		_camera = camera ?? throw new ArgumentNullException(nameof(camera));
	}

	public void DrawModel(Model model, ModelDetails modelDetails, Vector3 position, float scale, float rotationInRadians)
	{
		if (model == null)
		{
			_logger.Warning("Unable to draw a null model");
			return;
		}
		
		foreach (ModelMesh modelMesh in model.Meshes)
		{
			foreach (var effect1 in modelMesh.Effects)
			{
				BasicEffect effect = (BasicEffect) effect1;
				effect.EnableDefaultLighting();
				
				effect.View = _camera.ViewMatrix;
				effect.Projection = _camera.ProjectionMatrix;
		
				Matrix rotationMatrix = Matrix.CreateRotationY(rotationInRadians);
				Matrix translationMatrix = Matrix.CreateTranslation(modelDetails.PostScaleOffset + position);
				Matrix scaleMatrix = Matrix.CreateScale(modelDetails.ModelScale * scale);
				
				effect.World = scaleMatrix * rotationMatrix * translationMatrix; // Translation needs to be last
			}
			
			modelMesh.Draw();
		}
	}
}

public interface IModelDrawManager
{
	/// <summary>
	/// Draws a model given specific details on how to draw it
	/// </summary>
	/// <param name="model">The model that will be drawn. Should not be null</param>
	/// <param name="modelDetails">Details on how to setup the model to be drawn</param>
	/// <param name="position">Where to draw the model in the world</param>
	/// <param name="scale">The scale of the model</param>
	/// <param name="rotationInRadians">The rotation of the model in radians. 0 = ??, Pi/2 = ??</param>
	void DrawModel(Model model, ModelDetails modelDetails, Vector3 position, float scale, float rotationInRadians);
}