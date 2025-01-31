using System;

public class UIManager: IProvidable
{
    private ScreenUI failScreen, successScreen;
    private PanelUI scorePanel, movePanel;

    public UIManager()
    {
        ServiceProvider.Register(this);
    }

    public void ShowFailScreen(Action tryAgainAction)
    {
        if(failScreen == null) {
            failScreen = ServiceProvider.AssetLib.GetAsset<ScreenUI>(AssetType.FailScreen);
        }
        failScreen.Show();
        failScreen.OnContinueClicked += () => {
            tryAgainAction();
            failScreen.Hide();
        };
    }

    public void ShowSuccessScreen(Action startNextLevel)
    {
        if(successScreen == null) {
            successScreen = ServiceProvider.AssetLib.GetAsset<ScreenUI>(AssetType.SuccessScreen);
        }
        successScreen.Show();
        successScreen.OnContinueClicked += () => {
            startNextLevel();
            successScreen.Hide();
        };
    }

    public void OnMoveCountUIChanged(int newMoveCount)
    {
        if(movePanel == null) {
            movePanel = ServiceProvider.AssetLib.GetAsset<PanelUI>(AssetType.MovePanel);
        }
        movePanel.SetPanelText(newMoveCount.ToString());
    }

    public void OnScoreCountUIChanged(int newScoreCount)
    {
        if(scorePanel == null) {
            scorePanel = ServiceProvider.AssetLib.GetAsset<PanelUI>(AssetType.ScorePanel);
        }
        scorePanel.SetPanelText(newScoreCount.ToString());
    }


}