using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    public Vector3 position;
    public Vector2Int _coord;
    public ItemBase? Item { get; private set; }

    private Color _defaultColor;

    public bool HasItem { get => Item != null; }

    public void Start()
    {
        _defaultColor = spriteRenderer.color;
    }

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
        this.Item = item;
        item.PlaceInTile(this, _coord);
    }
    public void RemoveItem()
    {
        Item = null;
    }
    public void MockHighlight()
    {
        spriteRenderer.color = Color.yellow;
    }
    public void MockUnhighlight()
    {
        spriteRenderer.color = _defaultColor;
    }

    public bool TryGetColoredItem(out ColoredItem coloredItem)
    {
        coloredItem = null;
        if(!HasItem) return false;

        if(Item is ColoredItem item)
        {
            coloredItem = item;
            return true;
        }
        return false;
    }
}