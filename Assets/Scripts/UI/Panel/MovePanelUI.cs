using TMPro;
using UnityEngine;

public class MovePanelUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI moveCountText;
    public void SetMoveCount(int moveCount)
    {
        moveCountText.text = moveCount.ToString();
    }


}