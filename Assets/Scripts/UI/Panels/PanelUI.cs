using TMPro;
using UnityEngine;

public class PanelUI : MonoBehaviour, IUIPanel
{
    [SerializeField] private TextMeshProUGUI panelText;
    public void SetPanelText(string text)
    {
        panelText.text = text;
    }
}