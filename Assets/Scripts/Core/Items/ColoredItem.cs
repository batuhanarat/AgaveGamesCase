using UnityEngine;

public class ColoredItem : ItemBase
{
    public ItemColor Color { get; private set; }

    public void Configure(ItemColor color, Sprite sprite)
    {
        Color = color;
        spriteRenderer.sprite = sprite;
    }

    public void HighlightForLink()
    {
        // Highlight the item
    }

    public void RemoveHighlightForLink()
    {
        // Remove highlight of the item
    }
}