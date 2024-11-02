using UnityEditor;
using UnityEngine;

public class LevelLoader : EditorWindow
{
    private int levelIndex;

    [MenuItem("Window/LevelLoader")]
    public static void OpenWindow()
    {
        LevelLoader window = GetWindow<LevelLoader>();
        window.titleContent = new GUIContent("Level Loader");
    }

    private void OnGUI()
    {
        levelIndex = EditorGUILayout.IntField("Level Index", levelIndex);
        if (GUILayout.Button("Load Level"))
        {
            FindObjectOfType<BoardManager>().LoadLevel(levelIndex);
        }
    }
}
