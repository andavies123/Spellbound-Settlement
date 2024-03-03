namespace Andavies.SpellboundSettlement.GameWorld.ChunkSubGenerators;

public interface IChunkSubGenerator
{
	void Generate(Chunk chunk, int seed);
}