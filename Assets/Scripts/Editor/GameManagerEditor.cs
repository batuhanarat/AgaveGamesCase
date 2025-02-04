using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GameManager))]
public class GameManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        GameManager gameManager = (GameManager)target;

        EditorGUILayout.Space();

        if(GUILayout.Button("Shuffle Grid"))
        {
            if(Application.isPlaying)
            {
                ServiceProvider.ShuffleManager.Shuffle();
            }
            else
            {
                Debug.LogWarning("Shuffle only works in play mode!");
            }
        }
    }
}