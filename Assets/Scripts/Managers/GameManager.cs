using UnityEngine;

public class GameManager : MonoBehaviour
{
    public void Start()
    {
        StartGame();
    }

    public void StartGame()
    {
        var gameConfig = ServiceProvider.GameConfig;
        int rows = gameConfig.GridRow;
        int columns = gameConfig.GridColumn;
        var totalItems = rows * columns;

        ServiceProvider.ItemFactory.InitializePool(totalItems, totalItems*2);
        ServiceProvider.GameGrid.Initialize(rows,columns);

        for (int i = 0; i < rows; i++) {
            for (int j = 0; j < columns; j++)
            {
                ColoredItem coloredItem = ServiceProvider.ItemFactory.GetRandomColoredItem();
                ServiceProvider.GameGrid.AddToGrid(coloredItem, new Vector2Int(j,i));
            }
        }
        ServiceProvider.ShuffleManager.TryShuffle();
    }

}