using UnityEditor;
using UnityEngine;

public class LevelLoader : EditorWindow
{
    private LevelData selectedLevel;


    [MenuItem("Window/LevelLoader")]
    public static void OpenWindow()
    {
        LevelLoader window = GetWindow<LevelLoader>();
        window.titleContent = new GUIContent("level loader");
    }
    private void OnGUI()
    {
        selectedLevel= (LevelData) EditorGUILayout.ObjectField(selectedLevel, typeof(LevelData));
       if (GUILayout.Button("Load level"))
         {

            FindObjectOfType<BoardManager>().LoadLevel(selectedLevel);
        }
    }
}
