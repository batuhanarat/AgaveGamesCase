using System.Collections.Generic;

public class Link
{
    #region Private Fields

        private readonly LinkedList<ColoredItem> _linkedItems = new();
        private readonly HashSet<ColoredItem> _linkedSet = new();
        private bool _isInitialized;
        private ItemColor _targetColor;
        private const int VALID_LINK_COUNT = 3;

    #endregion

    public bool IsInitialized { get  => _isInitialized; }

    public void Initialize(ColoredItem item)
    {
        if (_isInitialized) return;

        _targetColor = item.Color;
        AddToLink(item);
        _isInitialized = true;
    }

    public bool TryAdd(ColoredItem item)
    {
        if (!_isInitialized || item.Color != _targetColor || !CheckItemsAreAdjacent(item)) return false;

        if(_linkedSet.Contains(item))
        {
            if(item != _linkedItems.Last.Previous?.Value) return false;

            RemoveFromLink();
            return false;
        }

        AddToLink(item);
        return true;
    }

    public bool TryExplodeLink()
    {
        if (!_isInitialized ) return false;

        var count = _linkedItems.Count;
        bool isValidLink = count >= VALID_LINK_COUNT;

        while(_linkedItems.Count > 0)
        {
            ColoredItem item = _linkedItems.First.Value;
            item.RemoveHighlightForLink();

            if (isValidLink)
            {
                item.TryExplode();
            }
            _linkedItems.RemoveFirst();
        }

        if (isValidLink)
        {
            ServiceProvider.ScoreManager.IncrementScore(count);
            ServiceProvider.MoveManager.MakeMove();
            return true;
        }
        return false;
    }

    public void Reset()
    {
        _linkedItems.Clear();
        _linkedSet.Clear();
        _isInitialized = false;
    }

    private void AddToLink(ColoredItem item)
    {
        _linkedItems.AddLast(item);
        _linkedSet.Add(item);
        item.HighlightForLink();
    }

    private void RemoveFromLink()
    {
        var removedItem = _linkedItems.Last.Value;
        removedItem.RemoveHighlightForLink();
        _linkedSet.Remove(removedItem);
        _linkedItems.RemoveLast();
    }

    private bool CheckItemsAreAdjacent(ColoredItem item)
    {
        var grid = ServiceProvider.GameGrid;
        var itemTile = grid.Tiles[item.Index.x, item.Index.y];
        var itemTileInLink = grid.Tiles[_linkedItems.Last.Value.Index.x,_linkedItems.Last.Value.Index.y];
        return grid.CheckTilesAreAdjacent(itemTile, itemTileInLink);
    }

}