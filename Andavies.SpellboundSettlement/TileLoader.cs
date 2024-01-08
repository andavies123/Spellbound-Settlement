using System;
using System.Collections.Generic;
using System.IO;
using Andavies.SpellboundSettlement.GameWorld;
using Andavies.SpellboundSettlement.Repositories;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Serilog;

namespace Andavies.SpellboundSettlement;

public class TileLoader : ITileLoader
{
	private readonly ILogger _logger;
	private readonly ITileRepository _tileRepository;

	public TileLoader(ILogger logger, ITileRepository tileRepository)
	{
		_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		_tileRepository = tileRepository ?? throw new ArgumentNullException(nameof(tileRepository));
	}
	
	public void LoadTilesFromJson(string filePath, ITileRepository tileRepository)
	{
		if (!File.Exists(filePath))
		{
			_logger.Warning("Unable to load tiles. File path does not exist. Path: {filePath}", filePath);
			return;
		}
		
		// Convert JSON to C#
		string jsonContent = File.ReadAllText(filePath);
		TileDetailsListContainer tileDetailsContainer = JsonConvert.DeserializeObject<TileDetailsListContainer>(jsonContent, new JsonSerializerSettings
		{
			Converters = { new TileDetailsListJsonConverter() }
		});

		// Once converted to C#, loop through and add it to the tile repository to be used everywhere else
		foreach (ITileDetails tileDetails in tileDetailsContainer.Tiles)
		{
			_tileRepository.TryAddTileDetails(tileDetails.TileId, tileDetails);
		}
	}
	
	/// <summary>
	/// Custom class used for converting Tile Details list from json to c#
	/// </summary>
	[Serializable]
	private class TileDetailsListContainer
	{
		public List<ITileDetails> Tiles { get; set; }
	}

	/// <summary>
	/// Custom class for converting a List of ITileDetails from Json to c# object 
	/// </summary>
	private class TileDetailsListJsonConverter : JsonConverter<List<ITileDetails>>
	{
		public override void WriteJson(JsonWriter writer, List<ITileDetails> value, JsonSerializer serializer)
		{
			throw new NotImplementedException();
		}

		public override List<ITileDetails> ReadJson(JsonReader reader, Type objectType, List<ITileDetails> existingValue, bool hasExistingValue, JsonSerializer serializer)
		{
			JArray array = JArray.Load(reader);
			List<ITileDetails> tileDetailsContainer = new();

			foreach (JToken token in array)
			{
				string meshType = token["meshType"]?.ToString();

				switch (meshType)
				{
					case "NonVisible":
						tileDetailsContainer.Add(token["details"]?.ToObject<NonVisibleTileDetails>(serializer));
						break;
					case "Terrain":
						tileDetailsContainer.Add(token["details"]?.ToObject<TerrainTileDetails>(serializer));
						break;
					case "Model":
						tileDetailsContainer.Add(token["details"]?.ToObject<ModelTileDetails>(serializer));
						break;
					default:
						throw new JsonSerializationException($"Unknown mesh type: {meshType}");
				}
			}

			return tileDetailsContainer;
		}
	}
}