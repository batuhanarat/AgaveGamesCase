using System;

public interface IUIObject
{
    public event Action OnContinueClicked;
    public void Show();
    public void Hide();
}