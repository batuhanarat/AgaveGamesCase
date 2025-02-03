
using System.Collections.Generic;

public interface IMatchManager
{
    bool IsValidMatchGroupPresent();
}

public class MatchManager : IProvidable , IMatchManager
{
    public MatchManager()
    {
        ServiceProvider.Register(this);
    }

    public bool IsValidMatchGroupPresent()
    {

        int columns = ServiceProvider.GameConfig.GridColumn;
        int rows = ServiceProvider.GameConfig.GridRow;
        bool[,] visited = new bool[ columns, rows];
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                if (!visited[col, row] && ServiceProvider.GameGrid.Tiles[col,row].HasItem)
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
        Tile startTile =  ServiceProvider.GameGrid.Tiles[startCol, startRow];
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

                if (!visited[neighbor.Index.x, neighbor.Index.y] &&
                    neighborcolor == targetColor)
                {
                    toVisit.Push((neighbor.Index.x, neighbor.Index.y));
                }
            }
        }
        return counter >= 3;
    }


}