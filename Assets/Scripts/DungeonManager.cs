using UnityEngine;
using UnityEngine.SceneManagement;

public class DungeonManager : MonoBehaviour
{
    public NodeRoom currentNode;
    public NodeRoom selectedRoom;
    public Color selectedColor;
    public Color normalColor;

    public void SelectRoom(NodeRoom selectedNode)
    {
        if (selectedRoom != null)
        {
            selectedRoom.GetComponent<SpriteRenderer>().color = normalColor;
        }
        selectedRoom = selectedNode;
        selectedNode.GetComponent<SpriteRenderer>().color = selectedColor;
    }

    public void LoadScene()
    {
        if (selectedRoom!=null)
        {
            SceneManager.LoadScene(selectedRoom.roomType.sceneAsset.name);
        }
        
    }

    public void EnableRooms()
    {
        foreach (var node in currentNode.outNodes)
        {
            node.canChoose = true;
        }
    }
}
