using System.Collections.Generic;
using UnityEngine;

public interface IFallManager
{
    void StartFall();
}

public class FallManager : IProvidable , IFallManager
{

    private int _rows;
    private int _columns;
    private IItemFactory _itemFactory;
    private const float NewItemStartHeight = 1.5f;


    public FallManager()
    {
        ServiceProvider.Register(this);

        _itemFactory = ServiceProvider.ItemFactory;
        _rows = ServiceProvider.GameConfig.GridRow;
        _columns = ServiceProvider.GameConfig.GridColumn;
    }

    public void StartFall()
    {
        List<(ItemBase item, Vector3 startPos, Vector3 endPos)> itemsToFall = new List<(ItemBase, Vector3, Vector3)>();

        for (int x = 0; x < _columns; x++)
        {
            int emptyY = -1;
            for (int y = 0; y < _rows; y++)
            {
                if (!ServiceProvider.GameGrid.Tiles[x, y].HasItem && emptyY == -1)
                {
                    emptyY = y;
                }
                else if (ServiceProvider.GameGrid.Tiles[x, y].HasItem && emptyY != -1)
                {
                    var item = ServiceProvider.GameGrid.Tiles[x, y].Item;
                    Vector3 startPos = ServiceProvider.GameGrid.Tiles[x, y].transform.position;
                    Vector3 endPos = ServiceProvider.GameGrid.Tiles[x, emptyY].transform.position;
                    itemsToFall.Add((item, startPos, endPos));

                    ServiceProvider.GameGrid.Tiles[x, emptyY].SetItem(item,false);
                    ServiceProvider.GameGrid.Tiles[x, y].RemoveItem();
                    item.UpdateIndexes(new Vector2Int(x, emptyY));
                    emptyY++;

                }
            }

            if (emptyY != -1)
            {
                for (int y = emptyY; y < _rows; y++)
                {
                    ItemBase newItem = _itemFactory.GetRandomColoredItem();

                    int heightMultiplier = 1/_rows - y;

                    Vector3 startPos = ServiceProvider.GameGrid.Tiles[x, _rows - 1].transform.position +
                    ( Vector3.down * GridRenderer.Instance.CellSize * NewItemStartHeight * heightMultiplier);

                    Vector3 endPos = ServiceProvider.GameGrid.Tiles[x, y].transform.position;

                    itemsToFall.Add((newItem, startPos, endPos));

                    ServiceProvider.GameGrid.Tiles[x, y].SetItem(newItem,true);
                    newItem.UpdateIndexes(new Vector2Int(x, y));
                }
            }
        }

        ServiceProvider.AnimationManager.StartAnimateItems(itemsToFall);

    }



}