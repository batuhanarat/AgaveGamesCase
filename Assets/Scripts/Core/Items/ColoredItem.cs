using UnityEngine;

public class ColoredItem : ItemBase
{
    public ItemColor Color { get; private set; }

    public void Configure(ItemColor color, Sprite sprite)
    {
        Color = color;
        spriteRenderer.sprite = sprite;
    }

    public void TryExplode()
    {
        ServiceProvider.ItemFactory.ReturnToPool(this);
    }

    public void HighlightForLink()
    {
        ServiceProvider.GameGrid._grid[_coord.x,_coord.y].MockHighlight();
    }

    public void RemoveHighlightForLink()
    {
        ServiceProvider.GameGrid._grid[_coord.x,_coord.y].MockUnhighlight();
    }
}