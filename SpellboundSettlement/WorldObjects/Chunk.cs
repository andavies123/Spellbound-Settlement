﻿using Microsoft.Xna.Framework;

namespace SpellboundSettlement.WorldObjects;

public class Chunk
{
	public Chunk(Vector2 chunkPosition, Vector2 worldOffset, (int x, int y, int z) tileCount)
	{
		ChunkPosition = chunkPosition;
		WorldOffset = worldOffset;
		TileCount = tileCount;
		Tiles = new int[TileCount.x, TileCount.y, TileCount.z];
	}
	
	public Vector2 ChunkPosition { get; }
	public Vector2 WorldOffset { get; }
	public (int x, int y, int z) TileCount { get; }
	public int[,,] Tiles { get; }

	public void SetAllTiles(int tileType)
	{
		for (int x = 0; x < Tiles.GetLength(0); x++)
		{
			for (int y = 0; y < Tiles.GetLength(1); y++)
			{
				for (int z = 0; z < Tiles.GetLength(2); z++)
				{
					Tiles[x, y, z] = tileType;
				}
			}
		}
	}
}