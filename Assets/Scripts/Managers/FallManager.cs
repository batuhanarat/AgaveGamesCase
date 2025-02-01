using System.Collections.Generic;
using UnityEngine;

public class FallManager : IProvidable
{

    private int _rows;
    private int _columns;
    private ItemFactory _itemFactory;
    private const float NewItemStartHeight = 2f;


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
                if (!ServiceProvider.GameGrid._grid[x, y].HasItem && emptyY == -1)
                {
                    emptyY = y;
                }
                else if (ServiceProvider.GameGrid._grid[x, y].HasItem && emptyY != -1)
                {
                    var item = ServiceProvider.GameGrid._grid[x, y].Item;
                    Vector3 startPos = ServiceProvider.GameGrid._grid[x, y].transform.position;
                    Vector3 endPos = ServiceProvider.GameGrid._grid[x, emptyY].transform.position;
                    itemsToFall.Add((item, startPos, endPos));

                    ServiceProvider.GameGrid._grid[x, emptyY].SetItem(item,false);
                    ServiceProvider.GameGrid._grid[x, y].RemoveItem();
                    item.UpdateCoordinate(new Vector2Int(x, emptyY));
                    emptyY++;

                }
            }

            if (emptyY != -1)
            {
                for (int y = emptyY; y < _rows; y++)
                {
                    ItemBase newItem = _itemFactory.GetRandomColoredItem();
                    Vector3 startPos = ServiceProvider.GameGrid._grid[x, _rows - 1].transform.position + Vector3.up * NewItemStartHeight;
                    Vector3 endPos = ServiceProvider.GameGrid._grid[x, y].transform.position;
                    itemsToFall.Add((newItem, startPos, endPos));

                    ServiceProvider.GameGrid._grid[x, y].SetItem(newItem,true);
                    newItem.UpdateCoordinate(new Vector2Int(x, y));
                }
            }
        }

        ServiceProvider.AnimationManager.StartFallingItemsAnimation(itemsToFall);

    }



}