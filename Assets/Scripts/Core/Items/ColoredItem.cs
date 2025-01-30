public class ColoredItem : ItemBase
{
    public ItemColor Color { get; private set; }

    public void SetColor(ItemColor color)
    {
        Color = color;
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