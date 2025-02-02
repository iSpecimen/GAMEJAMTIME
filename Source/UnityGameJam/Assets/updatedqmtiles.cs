using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Tilemap))]
[RequireComponent(typeof(TilemapCollider2D))]
[RequireComponent(typeof(CompositeCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class TilemapCollisionGenerator : MonoBehaviour
{
    private void Start()
    {
        SetupTilemapCollision();
    }

    void SetupTilemapCollision()
    {
        // Get components
        TilemapCollider2D tilemapCollider = GetComponent<TilemapCollider2D>();
        CompositeCollider2D compositeCollider = GetComponent<CompositeCollider2D>();
        Rigidbody2D rb = GetComponent<Rigidbody2D>();

        // Make sure Rigidbody2D is Static
        rb.bodyType = RigidbodyType2D.Static;

        // Ensure TilemapCollider2D is using Composite
        tilemapCollider.usedByComposite = true;

        // Force Collider Refresh
        tilemapCollider.enabled = false;
        tilemapCollider.enabled = true;

        // Set CompositeCollider2D to use Polygons
        compositeCollider.geometryType = CompositeCollider2D.GeometryType.Polygons;

        // Regenerate the collider shape
        compositeCollider.GenerateGeometry();
    }
}
