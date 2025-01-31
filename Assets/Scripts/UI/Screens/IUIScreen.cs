using System;

public interface IUIScreen
{
    public event Action OnContinueClicked;
    public void Show();
    public void Hide();
}