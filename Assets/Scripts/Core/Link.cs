using System.Collections.Generic;

public class Link
{
    #region Private Fields
        private readonly Stack<ColoredItem> _linkedStack = new();
        private readonly HashSet<ColoredItem> _linkedSet = new();
        private bool _isInitialized;
        private ItemColor _targetColor;
        private ColoredItem _beforeLastPushed;
        private ColoredItem _lastPushedItem => _linkedStack.Peek();

    # endregion

    public bool IsInitialized { get  => _isInitialized; }

    private const int VALID_LINK_COUNT = 3;

    public void Initialize(ColoredItem item)
    {
        if (_isInitialized) return;

        _targetColor = item.Color;
        AddToLink(item);
        _isInitialized = true;
    }

    public bool TryAdd(ColoredItem item)
    {
        if (!_isInitialized || item.Color != _targetColor) return false;

        if(_linkedSet.Contains(item))
        {
            if(item != _beforeLastPushed) return false;

            RemoveFromLink();

        }
        _beforeLastPushed = _linkedStack.Peek();
        AddToLink(item);
        return true;
    }

    public bool TryExplodeLink()
    {
        UnityEngine.Debug.Log("Stack COUNTU: " + _linkedStack.Count);
        if (!_isInitialized ) return false;

        var count = _linkedStack.Count;
        bool isValidLink = count >= VALID_LINK_COUNT;


        while(_linkedStack.Count > 0)
        {
            ColoredItem item = _linkedStack.Pop();
            item.RemoveHighlightForLink();

            if (isValidLink)
            {
                item.TryExplode();
            }
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
        _linkedStack.Clear();
        _linkedSet.Clear();
        _isInitialized = false;
    }

    private void AddToLink(ColoredItem item)
    {
        _linkedStack.Push(item);
        _linkedSet.Add(item);
        item.HighlightForLink();
    }

    private void RemoveFromLink()
    {
        _lastPushedItem.RemoveHighlightForLink();
        _linkedSet.Remove(_lastPushedItem);
        _linkedStack.Pop();
    }
}