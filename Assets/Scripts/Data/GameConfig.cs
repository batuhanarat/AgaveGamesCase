using UnityEngine;

[CreateAssetMenu(menuName = "AgaveGamesCase/GameConfig")]
public class GameConfig : ScriptableObject
{
    [Header("Grid Settings")]
    public int GridColumn = 8;
    public int GridRow = 8;

    [Header("Level Settings")]
    public int MoveCount = 10;
    public int ScoreLimit = 10;

}