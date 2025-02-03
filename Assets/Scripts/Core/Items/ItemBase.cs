using UnityEngine;

public abstract class ItemBase : MonoBehaviour
{
    [SerializeField] protected SpriteRenderer spriteRenderer;
    public Vector2Int Index;

    public void PlaceInTile(Tile tile, Vector2Int coord)
    {
        Index = coord;
        transform.position = tile.transform.position;
        transform.localScale = Vector3.one;
        transform.localScale = tile.transform.localScale;
    }

    public virtual void TryExplode()
    {
        Debug.Log("Item Exploded at " + Index);
    }

    public void UpdateIndexes(Vector2Int index)
    {
        Index = index;
    }

    public void Reset()
    {
        Index = Vector2Int.zero;
        transform.position = Vector3.zero;
    }

}