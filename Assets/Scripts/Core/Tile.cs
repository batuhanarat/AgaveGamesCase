using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private ParticleSystem ParticleSystem;
    [SerializeField] private SpriteRenderer SpriteRenderer;

    public Vector2Int Index;
    public bool HasItem { get => Item != null; }
    public ItemBase Item { get; private set; }

    private Color _defaultColor;

    public void Start()
    {
        _defaultColor = SpriteRenderer.color;
    }

    public void PlayParticleEffect(Sprite itemSprite)
    {
        ParticleSystem.TextureSheetAnimationModule tsam = ParticleSystem.textureSheetAnimation;
                tsam.mode = ParticleSystemAnimationMode.Sprites;
        for (int i = tsam.spriteCount - 1; i >= 0; i--) {
                    tsam.RemoveSprite(i);
                }
        tsam.AddSprite(itemSprite);
        ParticleSystem.Play();
    }

    public void SetSize(float width, float height)
    {
        Vector2 spriteSize = SpriteRenderer.sprite.bounds.size;

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
        SpriteRenderer.color = Color.yellow;
    }

    public void Unhighlight()
    {
        SpriteRenderer.color = _defaultColor;
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