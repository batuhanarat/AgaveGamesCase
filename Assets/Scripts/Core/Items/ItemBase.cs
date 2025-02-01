using UnityEngine;

public abstract class ItemBase : MonoBehaviour
{
    [SerializeField] protected SpriteRenderer spriteRenderer;
    public Vector2Int _coord;

    public void PlaceInTile(Tile tile, Vector2Int coord)
    {
        _coord = coord;
        transform.position = tile.transform.position;
        transform.localScale = Vector3.one;
        transform.localScale = tile.transform.localScale;
        //SetAlphaToNormal();
    }

    public void Reset()
    {
        _coord = Vector2Int.zero;
        transform.position = Vector3.zero;
    }

}