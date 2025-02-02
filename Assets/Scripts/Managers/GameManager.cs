using UnityEngine;

public class GameManager : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            ServiceProvider.LevelManager.OnLevelFailed();
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            ServiceProvider.LevelManager.OnLevelSuccess();
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            ServiceProvider.MoveManager.MakeMove();
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            ServiceProvider.ScoreManager.IncrementScore(1);
        }
    }

    public void Start()
    {
        StartGame();
    }

    public void StartGame()
    {
        // Take the grid data from game config
        ServiceProvider.ItemFactory.InitializePool();

        int rows = 4;
        int columns = 4;
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