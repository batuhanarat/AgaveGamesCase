using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Link
{
    #region Private Fields

        private readonly LinkedList<ColoredItem> _linkedItems = new();
        private readonly HashSet<ColoredItem> _linkedSet = new();
        private const int VALID_LINK_COUNT = 3;
        private bool _isInitialized;
        private ItemColor _targetColor;
        private float _delayBetweenExplosions = 0.04f;

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

    public IEnumerator TryExplodeLinkCoroutine()
    {
        if (!_isInitialized ) yield break;
        UnhighlightLastItem();

        var count = _linkedItems.Count;
        bool isValidLink = count >= VALID_LINK_COUNT;

        if(isValidLink)
        {
            while(_linkedItems.Count > 0)
            {
                ColoredItem item = _linkedItems.First.Value;
                item.RemoveHighlightAsLinked();
                item.TryExplode();
                _linkedItems.RemoveFirst();
                yield return new WaitForSeconds(_delayBetweenExplosions);
            }

            ServiceProvider.ScoreManager.IncrementScore(count);
            ServiceProvider.MoveManager.MakeMove();

        }
        else
        {
            while (_linkedItems.Count > 0)
            {
                ColoredItem item = _linkedItems.First.Value;
                item.RemoveHighlightAsLinked();
                _linkedItems.RemoveFirst();
            }
        }
    }

    public void Reset()
    {
        UnhighlightLastItem();
        _linkedItems.Clear();
        _linkedSet.Clear();
        _isInitialized = false;
    }

    private void AddToLink(ColoredItem item)
    {
        UnhighlightLastItem();
        _linkedItems.AddLast(item);
        _linkedSet.Add(item);
        item.HighlightAsLinked();
        HighlightLastItem();
    }

    private void UnhighlightLastItem()
    {
        if(_linkedItems.Last != null)
        {
            var item =  _linkedItems.Last.Value;
            item.RemoveHighlightAsLastInLink();
        }
    }
    private void HighlightLastItem()
    {
        var item = _linkedItems.Last.Value;
        item.HighlightAsLastInLink();
    }

    private void RemoveFromLink()
    {
        var removedItem = _linkedItems.Last.Value;
        UnhighlightLastItem();
        removedItem.RemoveHighlightAsLinked();
        _linkedSet.Remove(removedItem);
        _linkedItems.RemoveLast();
        HighlightLastItem();
    }

    private bool CheckItemsAreAdjacent(ColoredItem item)
    {
        var grid = ServiceProvider.GameGrid;
        var itemTile = grid.Tiles[item.Index.x, item.Index.y];
        var itemTileInLink = grid.Tiles[_linkedItems.Last.Value.Index.x,_linkedItems.Last.Value.Index.y];
        return grid.CheckTilesAreAdjacent(itemTile, itemTileInLink);
    }

}