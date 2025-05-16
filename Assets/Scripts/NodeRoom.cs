using UnityEngine;
using System.Collections.Generic;

public class NodeRoom : MonoBehaviour
{
    public List<NodeRoom> outNodes = new List<NodeRoom>();
    public bool canChoose = false;
    public RoomType roomType;
    public GameObject sonSprite;


    public void SelectRoom()
    {
        if (canChoose)
        {
            DungeonManager dungeonManager = FindAnyObjectByType<DungeonManager>();
            dungeonManager.SelectRoom(this);
            
        }
    }

    private void OnMouseDown()
    {
        SelectRoom();
    }

    public void ShowSpriteRoom()
    {
        sonSprite.transform.localScale = roomType.spriteScale;
        sonSprite.GetComponent<SpriteRenderer>().sprite = roomType.sprite;        
    }
}
