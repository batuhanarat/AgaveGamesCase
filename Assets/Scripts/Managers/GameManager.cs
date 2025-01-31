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
}