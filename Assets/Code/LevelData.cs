using UnityEngine;

[CreateAssetMenu]
public class LevelData:ScriptableObject
{
    [TextArea(minLines: 10, maxLines: 10)] public string content;
}
