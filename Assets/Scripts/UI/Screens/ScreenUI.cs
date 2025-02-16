using System;
using UnityEngine;
using UnityEngine.UI;

public class ScreenUI : MonoBehaviour, IUIScreen
{
    [SerializeField] private Button ContinueButton;
    private void Awake()
    {
        ContinueButton.onClick.AddListener(() => {
        OnContinueClicked?.Invoke();
        });
    }

    public event Action OnContinueClicked;

    public void Hide()
    {
        var shadow = ServiceProvider.AssetLib.GetAsset<RectTransform>(AssetType.Shadow);
        shadow.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }

    public void Show()
    {
        var shadow = ServiceProvider.AssetLib.GetAsset<RectTransform>(AssetType.Shadow);
        shadow.gameObject.SetActive(true);
        gameObject.SetActive(true);
    }
}