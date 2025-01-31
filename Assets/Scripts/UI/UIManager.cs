using System;

public class UIManager: IProvidable
{
    private ScreenChangeUI failScreen, successScreen;

    public UIManager()
    {
        ServiceProvider.Register(this);
    }

    public void ShowFailScreen(Action tryAgainAction)
    {
        if(failScreen == null) {
            failScreen = ServiceProvider.AssetLib.GetAsset<ScreenChangeUI>(AssetType.FailScreen);
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
            successScreen = ServiceProvider.AssetLib.GetAsset<ScreenChangeUI>(AssetType.SuccessScreen);
        }
        successScreen.Show();
        successScreen.OnContinueClicked += () => {
            startNextLevel();
            successScreen.Hide();
        };
    }


}