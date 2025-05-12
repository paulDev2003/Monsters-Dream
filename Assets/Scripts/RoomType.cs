using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "RoomType", menuName = "Scriptable Objects/RoomType")]
public class RoomType : ScriptableObject
{
    public Sprite sprite;
    public UnityEditor.SceneAsset sceneAsset;
}
