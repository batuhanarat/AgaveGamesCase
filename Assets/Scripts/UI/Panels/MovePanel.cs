public class MovePanel : PanelUI
{
    void Awake()
    {
        var moveCount = ServiceProvider.GameConfig.MoveCount;
        SetPanelText(moveCount.ToString());
    }
}