using Andavies.SpellboundSettlement.GameWorld.Repositories;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Serilog;

namespace Andavies.SpellboundSettlement.GameWorld;

public class TileLoader : ITileLoader
{
	private readonly ILogger _logger;

	public TileLoader(ILogger logger)
	{
		_logger = logger ?? throw new ArgumentNullException(nameof(logger));
	}
	
	public void LoadTilesFromJson(string tileDetailsJson, ITileRepository tileRepository)
	{
		TileDetailsListContainer? tileDetailsContainer = JsonConvert.DeserializeObject<TileDetailsListContainer>(tileDetailsJson, new JsonSerializerSettings
		{
			Converters = { new TileDetailsListJsonConverter() }
		});

		if (tileDetailsContainer == null)
		{
			_logger.Warning("Unable to deserialize tile JSON");
			return;
		}

		// Once converted to C#, loop through and add it to the tile repository to be used everywhere else
		foreach (ITileDetails tileDetails in tileDetailsContainer.Tiles)
		{
			tileRepository.TryAddTileDetails(tileDetails.TileId, tileDetails);
		}
	}
	
	/// <summary>
	/// Custom class used for converting Tile Details list from json to c#
	/// </summary>
	[Serializable]
	private class TileDetailsListContainer
	{
		public List<ITileDetails> Tiles { get; set; } = new();
	}

	/// <summary>
	/// Custom class for converting a List of ITileDetails from Json to c# object 
	/// </summary>
	private class TileDetailsListJsonConverter : JsonConverter<List<ITileDetails>>
	{
		public override void WriteJson(JsonWriter writer, List<ITileDetails>? value, JsonSerializer serializer)
		{
			throw new NotImplementedException();
		}

		public override List<ITileDetails> ReadJson(JsonReader reader, Type objectType, List<ITileDetails>? existingValue, bool hasExistingValue, JsonSerializer serializer)
		{
			JArray array = JArray.Load(reader);
			List<ITileDetails> tileDetailsContainer = new();

			foreach (JToken token in array)
			{
				string meshType = token["meshType"]?.ToString() ?? string.Empty;

				switch (meshType)
				{
					case "NonVisible":
						tileDetailsContainer.Add(token["details"]?.ToObject<NonVisibleTileDetails>(serializer) ?? throw new NullReferenceException());
						break;
					case "Terrain":
						tileDetailsContainer.Add(token["details"]?.ToObject<TerrainTileDetails>(serializer) ?? throw new NullReferenceException());
						break;
					case "Model":
						tileDetailsContainer.Add(token["details"]?.ToObject<ModelTileDetails>(serializer) ?? throw new NullReferenceException());
						break;
					default:
						throw new JsonSerializationException($"Unknown mesh type: {meshType}");
				}
			}

			return tileDetailsContainer;
		}
	}
}