using UnityEngine.SceneManagement;

public interface ILevelManager
{
    void DecideLevelStatus();
    void OnLevelFailed();
    void OnLevelSuccess();
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

    private void TryAgain()
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
