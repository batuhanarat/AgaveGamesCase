using UnityEngine;

public interface IGameManager
{
    void StartGame();
}

public class GameManager : MonoBehaviour, IGameManager
{
    public GameConfig GameConfig;
    public void Start()
    {
        StartGame();
    }

    public void StartGame()
    {
        GameConfig = ServiceProvider.GameConfig;
        int rows = GameConfig.GridRow;
        int columns = GameConfig.GridColumn;

        ServiceProvider.ItemFactory.Initialize(GameConfig);
        ServiceProvider.GameGrid.BuildGrid(rows,columns);

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