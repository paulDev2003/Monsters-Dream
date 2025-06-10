using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections.Generic;

public class PanelChooseMonster : MonoBehaviour
{
    public InfoPanelMonster monsterSelected;
    public Color selectedColor;
    public Color deselectedColor;
    public DungeonTeam dungeonTeam;
    public MonstersHouse monstersHouse;
    public GameObject panelChangeMonster;
    public List<MonsterSlot> monsterSlots = new List<MonsterSlot>();
    public List<TextMeshProUGUI> txtsLevels = new List<TextMeshProUGUI>();
    public MonsterDataBase monsterDataBase;
    public MonsterSlot monsterSlotChange;
    public TextMeshProUGUI txtLevelChange;
    public UnityEvent EventComplete;

    public void AddMonsterToTeam()
    {
        if (monsterSelected == null)
        {
            return;
        }
        foreach (var monster in dungeonTeam.allMonsters)
        {
            if (monster.monsterName == "")
            {

                monster.monsterName = monsterSelected.savedName;
                monster.level = monsterSelected.savedLevel;
                monster.currentHealth = monsterSelected.savedHealth;
                CheckBestiary(monster.monsterName);
                EventComplete.Invoke();
                return;
            }
        }
        panelChangeMonster.SetActive(true);
        FillDataTeam();
    }

    public void FillDataTeam()
    {
        int i = 0;
        foreach (var monster in dungeonTeam.allMonsters)
        {
            MonsterBase monsterBase = monsterDataBase.GetMonsterBaseByName(monster.monsterName);
            monsterSlots[i].img.sprite = monsterBase.monsterSO.sprite;
            monsterSlots[i].monsterData = monster;
            txtsLevels[i].text = $"Lv. {monster.level}";
            monsterSlots[i].savedI = i;
            i++;
        }
        MonsterBase monsterBaseChange = monsterDataBase.GetMonsterBaseByName(monsterSelected.savedName);
        monsterSlotChange.img.sprite = monsterBaseChange.monsterSO.sprite;
        txtLevelChange.text = $"Lv. {monsterSelected.savedLevel}";
        monsterSlotChange.monsterData.monsterName = monsterSelected.savedName;
        monsterSlotChange.monsterData.level = monsterSelected.savedLevel;
        monsterSlotChange.monsterData.currentHealth = monsterSelected.savedHealth;

    }

    public void ChangeMemberTeam(MonsterSlot selectedSlot)
    {
        MonsterData savedMonsterData = selectedSlot.monsterData;
        string savedLevel = txtsLevels[selectedSlot.savedI].text;
        Sprite savedSprite = selectedSlot.img.sprite;
        selectedSlot.monsterData = monsterSlotChange.monsterData;
        txtsLevels[selectedSlot.savedI].text = txtLevelChange.text;
        selectedSlot.img.sprite = monsterSlotChange.img.sprite;
        monsterSlotChange.monsterData = savedMonsterData;
        txtLevelChange.text = savedLevel;
        monsterSlotChange.img.sprite = savedSprite;

    }

    public void SaveMembersTeam()
    {
        int i = 0;
        foreach (var slot in monsterSlots)
        {
            dungeonTeam.allMonsters[i] = slot.monsterData;
            i++;
        }
        foreach (var data in dungeonTeam.allMonsters)
        {
            CheckBestiary(data.monsterName);
        }
    }

    private void CheckBestiary(string monsterName)
    {
        bool founded = false;
        foreach (var data in monstersHouse.bestiary)
        {
            if (data.monsterName == monsterName)
            {
                data.wasFriend = true;
                founded = true;
                return;
            }
        }
        if (!founded)
        {
            DiscoverMonster monsterDiscovered = new DiscoverMonster
            {
                monsterName = monsterName,
                viewed = false,
                wasFriend = true
            };
            monstersHouse.bestiary.Add(monsterDiscovered);
        }
    }
}
