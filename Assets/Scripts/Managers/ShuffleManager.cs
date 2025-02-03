
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public struct MovableItems
{
    public ColoredItem CurrentItem;
    public Tile OriginalTile;
}

public interface IShuffleManager
{
    bool TryShuffle();
    void Reset();
}

public class ShuffleManager : IProvidable, IShuffleManager
{
    public ShuffleManager()
    {
        ServiceProvider.Register(this);
    }

    private List<(ItemBase item, Vector3 startPos, Vector3 endPos)> itemsToAnimate = new();

    public bool TryShuffle()
    {
        if(ServiceProvider.MatchManager.IsValidMatchGroupPresent()) return false;
        ServiceProvider.MoveManager.LockMove();
        Shuffle();
        return true;
    }
    public void Reset()
    {
        itemsToAnimate.Clear();
    }
    public void Shuffle()
    {
        UnityEngine.Debug.Log("Deadlock detected");
        Dictionary<ItemColor,int> colorCounts = new();
        var GameGrid = ServiceProvider.GameGrid;

        var tilesToShuffle = new List<Tile>();
        var itemsToShuffle = new List<MovableItems>();

        foreach(var tile in GameGrid.Tiles)
        {
            if (!tile.TryGetColoredItem(out ColoredItem coloredItem)) continue;

            var itemColor = coloredItem.Color;

            tilesToShuffle.Add(tile);
            itemsToShuffle.Add(new MovableItems
            {
                CurrentItem = coloredItem,
                OriginalTile = tile
            });

            if(colorCounts.ContainsKey(itemColor))
            {
                colorCounts[itemColor]++;
            }
            else
            {
                colorCounts.Add(itemColor, 1);
            }
        }

        ItemColor selectedColorForMatch = ItemColor.Red;

        int maxCount = 0;
        foreach(var colorCount in colorCounts)
        {
            if(colorCount.Value > maxCount)
            {
                maxCount = colorCount.Value;
                selectedColorForMatch = colorCount.Key;
            }
        }

        tilesToShuffle = tilesToShuffle.OrderBy(x => Random.value).ToList();

        var matchTiles = new List<Tile>();

        for(int i = 0; i < tilesToShuffle.Count; i++)
        {
            var currentTile = tilesToShuffle[i];
            matchTiles.Clear();
            matchTiles.Add(currentTile);

            bool foundHorizontal = false;
            if(GameGrid.TryGetRightTile(currentTile.Index.x, currentTile.Index.y, out Tile rightTile))
            {
                if(GameGrid.TryGetRightTile(currentTile.Index.x + 1, currentTile.Index.y, out Tile rightRightTile))
                {
                    matchTiles.Add(rightTile);
                    matchTiles.Add(rightRightTile);
                    foundHorizontal = true;
                }
            }

            if(!foundHorizontal)
            {
                matchTiles.Clear();
                matchTiles.Add(currentTile);

                if(GameGrid.TryGetUpperTile(currentTile.Index.x, currentTile.Index.y, out Tile upperTile))
                {
                    if(GameGrid.TryGetUpperTile(currentTile.Index.x, currentTile.Index.y + 1, out Tile upperUpperTile))
                    {
                        matchTiles.Add(upperTile);
                        matchTiles.Add(upperUpperTile);
                        break;
                    }
                }
            }
            else
            {
                break;
            }
        }

        if(matchTiles.Count == 3)
        {
            var itemsOfSelectedColor = itemsToShuffle
                .Where(item => item.CurrentItem.Color == selectedColorForMatch)
                .Take(3)
                .ToList();

            foreach(var tile in matchTiles)
            {
                tilesToShuffle.Remove(tile);
            }
            foreach(var item in itemsOfSelectedColor)
            {
                itemsToShuffle.Remove(item);
            }

            for(int i = 0; i < 3; i++)
            {
                itemsToAnimate.Add((
                    itemsOfSelectedColor[i].CurrentItem,
                    itemsOfSelectedColor[i].OriginalTile.transform.position,
                    matchTiles[i].transform.position
                ));

                var currentItem = itemsOfSelectedColor[i].CurrentItem;
                var tileOfCurrentItem =  itemsOfSelectedColor[i].OriginalTile;

                var targetItem = matchTiles[i].Item;
                var TargetTile = matchTiles[i];

                TargetTile.RemoveItem();
                TargetTile.SetItem(currentItem,false);
                currentItem.UpdateIndexes(TargetTile.Index);
            }

            itemsToShuffle = itemsToShuffle.OrderBy(x => Random.value).ToList();

            for(int i = 0; i < itemsToShuffle.Count; i++)
            {
                if(itemsToShuffle[i].OriginalTile == tilesToShuffle[i])
                {
                    var swapIndex = i > 0 ? i - 1 : i + 1;
                    var temp = tilesToShuffle[i];
                    tilesToShuffle[i] = tilesToShuffle[swapIndex];
                    tilesToShuffle[swapIndex] = temp;
                }
            }

            for(int i = 0; i < itemsToShuffle.Count; i++)
            {

                itemsToAnimate.Add((
                    itemsToShuffle[i].CurrentItem,
                    itemsToShuffle[i].OriginalTile.transform.position,
                    tilesToShuffle[i].transform.position
                ));

                var currentItem =   itemsToShuffle[i].CurrentItem;
                var tileOfCurrentItem =  itemsToShuffle[i].OriginalTile;
                var targetItem = tilesToShuffle[i].Item;
                var targetTile = tilesToShuffle[i];

                targetTile.RemoveItem();
                targetTile.SetItem(currentItem,false);
                currentItem.UpdateIndexes(targetTile.Index);


            }

            ServiceProvider.AnimationManager.StartShuffleItems(itemsToAnimate);

        }


    }


}