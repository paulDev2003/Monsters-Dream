using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class DungeonManager : MonoBehaviour
{
    public NodeRoom currentNode;
    public NodeRoom selectedRoom;
    public Color selectedColor;
    public Color normalColor;
    public Color currentRoomColor;
    public DungeonTeam dungeonTeam;
    public List<Image> monstersImg = new List<Image>();
    public List<Image> starterImg = new List<Image>();
    public List<TextMeshProUGUI> txtsLvl = new List<TextMeshProUGUI>();
    public MonsterDataBase monsterDataBase;
    public int countStarters = 0;

    private void Start()
    {
        currentNode.GetComponent<SpriteRenderer>().color = currentRoomColor;
        FillMonsterSlots();
    }

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

    private void FillMonsterSlots()
    {
        int i = 0;
        foreach (var monster in dungeonTeam.allMonsters)
        {
            if (monster.monsterName != "")
            {
                MonsterBase monsterBase = monsterDataBase.GetMonsterBaseByName(monster.monsterName);
                monstersImg[i].enabled = true;
                monstersImg[i].sprite = monsterBase.monsterSO.sprite;
                monstersImg[i].GetComponent<SlotStarter>().monsterData = monster;
                txtsLvl[i].text = $"Lv.{monster.level}";
                if (monster.isStarter)
                {
                    starterImg[i].enabled = true;
                    countStarters++;
                }
            }
            i++;
        }
        if (countStarters == 0)
        {
            foreach (var img in monstersImg)
            {
                SlotStarter slotStarter = img.GetComponent<SlotStarter>();
                if (slotStarter.monsterData.currentHealth > 0)
                {
                    slotStarter.monsterData.isStarter = true;
                    slotStarter.starterAuraImg.enabled = true;
                    return;
                }
            }
        }
    }


    public void SaveStarters()
    {
        int i = 0;
        foreach (var img in monstersImg)
        {
            if (img.enabled)
            {
                SlotStarter slotStarter = img.GetComponent<SlotStarter>();
                dungeonTeam.allMonsters[i] = slotStarter.monsterData;
            }
            i++;
        }
    }
}
