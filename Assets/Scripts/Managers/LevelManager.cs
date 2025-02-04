using UnityEngine.SceneManagement;

public interface ILevelManager
{
    void DecideLevelStatus();
    void OnLevelFailed();
    void OnLevelSuccess();
    void RestartLevel();
}

public class LevelManager : IProvidable , ILevelManager
{
    public LevelManager()
    {
        ServiceProvider.Register(this);
    }

    public void DecideLevelStatus()
    {
        if(ServiceProvider.ScoreManager.IsScoreSufficient())
        {
            OnLevelSuccess();
        }
        else
        {
            OnLevelFailed();
        }
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(0);
    }

    public void OnLevelFailed()
    {
        ServiceProvider.UIManager.ShowFailScreen(RestartLevel);
    }

    public void OnLevelSuccess()
    {
        ServiceProvider.UIManager.ShowSuccessScreen(RestartLevel);
    }

}
