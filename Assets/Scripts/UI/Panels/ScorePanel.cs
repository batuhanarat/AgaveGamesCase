public class ScorePanel : PanelUI
{
    void Awake()
    {
        var scoreLimit = ServiceProvider.GameConfig.ScoreLimit;
        SetPanelText(scoreLimit.ToString());
    }
}