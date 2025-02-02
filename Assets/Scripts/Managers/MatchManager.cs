
using System.Collections.Generic;

public class MatchManager : IProvidable
{
    public MatchManager()
    {
        ServiceProvider.Register(this);
    }

    public bool IsThereValidMatchGroupPresent()
    {

        int columns = ServiceProvider.GameConfig.GridColumn;
        int rows = ServiceProvider.GameConfig.GridRow;
        bool[,] visited = new bool[ columns, rows];
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                if (!visited[col, row] && ServiceProvider.GameGrid._grid[col,row].HasItem)
                {
                    if(FindMatchGroup(col, row, visited))
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    private bool FindMatchGroup(int startCol, int startRow, bool[,] visited)
    {
        Tile startTile =  ServiceProvider.GameGrid._grid[startCol, startRow];
        startTile.TryGetColoredItem(out ColoredItem item);
        ItemColor targetColor  = item.Color;

        Stack<(int col, int row)> toVisit = new();
        toVisit.Push((startCol, startRow));
        int counter = 0;

        while (toVisit.Count > 0)
        {
            var (col, row) = toVisit.Pop();
            if (visited[col, row]) continue;

            visited[col, row] = true;
            counter++;

            var neighbors =  ServiceProvider.GameGrid.GetNeighborsFromIndexesWithItem(col, row);
            for (int i = 0; i < neighbors.Count; i++)
            {
                Tile neighbor = neighbors[i];
                neighbor.TryGetColoredItem(out ColoredItem neighborItem);
                var neighborcolor = neighborItem.Color;

                if (!visited[neighbor._coord.x, neighbor._coord.y] &&
                    neighborcolor == targetColor)
                {
                    toVisit.Push((neighbor._coord.x, neighbor._coord.y));
                }
            }
        }
        return counter >= 3;
    }


}