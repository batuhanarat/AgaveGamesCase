using System.Collections.Generic;
using UnityEngine;

public class Grid : IProvidable
{
    public Tile[,] Tiles;
    private int _rows;
    private int _columns;

    public Grid()
    {
        ServiceProvider.Register(this);
    }

    public void BuildBoard(int rows, int columns)
    {
        _rows = rows;
        _columns = columns;
        Tiles = new Tile[columns, rows];
        GridRenderer.Instance.Initialize(rows, columns,Tiles);
    }

    public void AddToGrid(ItemBase item, Vector2Int coord)
    {
        Tile tile = Tiles[coord.x,coord.y];
        tile.SetItem(item,true);
    }

    public Tile GetTileFromIndex(int column, int row)
    {
        return Tiles[column,row];
    }

    public Tile GetTileFromIndex(Vector2Int index)
    {
        return Tiles[index.x,index.y];
    }

    public bool CheckTilesAreAdjacent(Tile tile1, Tile tile2)
    {
        var xDiff = Mathf.Abs(tile1.Index.x - tile2.Index.x);
        var yDiff = Mathf.Abs(tile1.Index.y - tile2.Index.y);
        var manhattanDistance = xDiff + yDiff;
        return manhattanDistance == 1;
    }

    public bool TryGetRightTile(int column, int row, out Tile rightTile)
    {
        if(column + 1 >= _columns) {
            rightTile = default;
            return false;
        }
        rightTile = Tiles[column+1,row];

        return rightTile.HasItem;
    }

    public bool TryGetLeftTile(int column, int row, out Tile leftTile)
    {
        if(column - 1 < 0) {
            leftTile = default;
            return false;
        }
        leftTile = Tiles[column-1,row];

        return leftTile.HasItem;
    }
    public bool TryGetUpperTile(int column, int row, out Tile upperTile)
    {
        if(row + 1 >= _rows) {
            upperTile = default;
            return false;
        }
        upperTile = Tiles[column,row+1];

        return upperTile.HasItem;
    }

    public bool TryGetBellowTile(int column, int row, out Tile belowTile)
    {
        if(row - 1 < 0) {
            belowTile = default;
            return false;
        }
        belowTile = Tiles[column,row-1];

        return belowTile.HasItem;
    }

    public List<Tile> GetNeighborsFromIndexesWithItem(int column, int row)
    {
        List<Tile> neighbors = new();

        if (TryGetUpperTile(column, row, out Tile upperTile)) neighbors.Add(upperTile);
        if (TryGetBellowTile(column, row, out Tile bellowTile)) neighbors.Add(bellowTile);
        if (TryGetRightTile(column, row, out Tile rightTile)) neighbors.Add(rightTile);
        if (TryGetLeftTile(column, row, out Tile leftTile)) neighbors.Add(leftTile);

        return neighbors;
    }

}