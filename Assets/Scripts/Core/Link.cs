using System.Collections.Generic;

public class Link
{
    #region Private Fields
        private readonly Queue<ColoredItem> linkedItems = new();
        private bool _isInitialized;
        private ItemColor targetColor;
    # endregion

    public bool IsInitialized { get  => _isInitialized; }
    public void Initialize(ColoredItem item)
    {
        if (_isInitialized) return;

        targetColor = item.Color;
        AddToLink(item);
        _isInitialized = true;
    }
    public bool TryAdd(ColoredItem item)
    {
        if (!_isInitialized || item.Color != targetColor) return false;
        AddToLink(item);
        return true;
    }
    public void Reset()
    {
        linkedItems.Clear();
        _isInitialized = false;
    }
    private void AddToLink(ColoredItem item)
    {
        linkedItems.Enqueue(item);
        item.HighlightForLink();
    }
    private void RemoveFromLink(ColoredItem item)
    {
        linkedItems.Dequeue();
        item.RemoveHighlightForLink();
    }
}