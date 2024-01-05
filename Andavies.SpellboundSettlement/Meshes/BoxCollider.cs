using Microsoft.Xna.Framework;

namespace Andavies.SpellboundSettlement.Meshes;

public class BoxCollider
{
	public BoxCollider(Vector3 position, Vector3 size)
	{
		Collider = new BoundingBox(position, position + size);
	}

	public bool IsActive { get; set; } = true;
	public BoundingBox Collider { get; set; }
}