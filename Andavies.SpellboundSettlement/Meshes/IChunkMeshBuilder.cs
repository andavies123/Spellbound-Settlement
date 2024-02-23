using Andavies.SpellboundSettlement.GameWorld;

namespace Andavies.SpellboundSettlement.Meshes;

public interface IChunkMeshBuilder
{
	ChunkMesh BuildChunkMesh(ChunkData chunkData);
}