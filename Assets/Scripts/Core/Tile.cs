using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    private Vector3 position;
    private Vector2Int _coord;
    private ItemBase? item;

    public void SetWorldPosition(Vector3 position)
    {
        this.position = position;
       // transform.position = position;
    }
    public void SetSize(float width, float height)
    {
        Vector2 spriteSize = spriteRenderer.sprite.bounds.size;

        float scaleX = width / spriteSize.x;
        float scaleY = height / spriteSize.y;

        transform.localScale = new Vector3(scaleX, scaleY, 1f);
    }
    public void SetCoord(Vector2Int coord)
    {
        this._coord = coord;
    }

    public void SetItem(ItemBase item)
    {
        this.item = item;
        item.PlaceInTile(this, _coord);
    }
}