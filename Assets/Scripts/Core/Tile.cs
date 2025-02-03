using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;

    public Vector2Int Index;
    public bool HasItem { get => Item != null; }
    public ItemBase Item { get; private set; }

    private Color _defaultColor;

    public void Start()
    {
        _defaultColor = spriteRenderer.color;
    }

    public void SetSize(float width, float height)
    {
        Vector2 spriteSize = spriteRenderer.sprite.bounds.size;

        float scaleX = width / spriteSize.x;
        float scaleY = height / spriteSize.y;

        transform.localScale = new Vector3(scaleX, scaleY, 1f);
    }

    public void SetCoord(Vector2Int index)
    {
        Index = index;
    }

    public void SetItem(ItemBase item, bool  shouldPlaceInTile )
    {
        this.Item = item;
        if(shouldPlaceInTile)
        {
            item.PlaceInTile(this, Index);
        }
    }

    public void RemoveItem()
    {
        Item = null;
    }

    public void Highlight()
    {
        spriteRenderer.color = Color.yellow;
    }

    public void Unhighlight()
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