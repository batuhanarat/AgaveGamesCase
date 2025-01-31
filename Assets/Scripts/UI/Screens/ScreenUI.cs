using System;
using UnityEngine;
using UnityEngine.UI;

public class ScreenUI : MonoBehaviour, IUIScreen
{
    [SerializeField] private Button ContinueButton;
   // [SerializeField] private GameObject Shadow;
    private void Awake()
    {
        ContinueButton.onClick.AddListener(() => { OnContinueClicked?.Invoke(); });
    }

    public event Action OnContinueClicked;

    public void Hide()
    {
       // Shadow.SetActive(false);
        gameObject.SetActive(false);
    }

    public void Show()
    {
       // Shadow.SetActive(true);
        gameObject.SetActive(true);
    }
}