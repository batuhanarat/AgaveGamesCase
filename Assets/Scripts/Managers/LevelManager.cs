using UnityEngine.SceneManagement;

public class LevelManager : IProvidable
{
    public LevelManager()
    {
        ServiceProvider.Register(this);
    }

    public void DecideLevelStatus()
    {
        if(ServiceProvider.ScoreManager.IsWin())
        {
            OnLevelSuccess();
        } else
        {
            OnLevelFailed();
        }
    }

    public void TryAgain()
    {
        SceneManager.LoadScene(0);
    }

    public void OnLevelFailed()
    {
        ServiceProvider.UIManager.ShowFailScreen(TryAgain);
    }

    public void OnLevelSuccess()
    {
        ServiceProvider.UIManager.ShowSuccessScreen(TryAgain);
    }
}
