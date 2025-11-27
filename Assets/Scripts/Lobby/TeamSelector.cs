using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class TeamSelector : MonoBehaviour
{
    public List<Image> teamSprites = new List<Image>();
    public List<TeamSlot> teamSlots = new List<TeamSlot>();
    public List<SummonSlot> inventorySlots = new List<SummonSlot>();
    public DungeonTeam dungeonTeam;
    public MonsterDataBase monsterDataBase;
    public MonstersHouse monstersHouse;

    [Header("Summon Stats")]
    public TextMeshProUGUI txtName;
    public TextMeshProUGUI txtLvl;
    public TextMeshProUGUI txtDam;
    public TextMeshProUGUI txtMDam;
    public TextMeshProUGUI txtDef;
    public TextMeshProUGUI txtAsp;
    public TextMeshProUGUI txtMDef;
    public TextMeshProUGUI txtEva;
    public TextMeshProUGUI txtHealth;
    public Transform spawnSummon;
    private GameObject summonInvoked;


    private void Start()
    {
        FillOutImages();
        FillOutAcquiredSummons();
    }

    public void FillOutImages()
    {
        if (dungeonTeam.firstTeam.Count != 0)
        {
            int i = 0;
            foreach (var summon in dungeonTeam.firstTeam)
            {
                if (summon.monsterName == "")
                {
                    continue;
                }
                MonsterBase monsterBase = monsterDataBase.GetMonsterBaseByName(summon.monsterName);
                teamSprites[i].gameObject.SetActive(true);
                teamSprites[i].sprite = monsterBase.monsterSO.sprite;
                teamSlots[i].img.sprite = teamSprites[i].sprite;
                teamSlots[i].monsterName = summon.monsterName;
                teamSlots[i].gameObject.SetActive(true);
                teamSlots[i].level = summon.level;
                i++;
            }
        }
        else if(dungeonTeam.allMonsters.Count != 0)
        {
            int i = 0;
            foreach (var summon in dungeonTeam.allMonsters)
            {
                if (summon.monsterName == "")
                {
                    continue;
                }
                MonsterBase monsterBase = monsterDataBase.GetMonsterBaseByName(summon.monsterName);
                teamSprites[i].gameObject.SetActive(true);
                teamSprites[i].sprite = monsterBase.monsterSO.sprite;
                teamSlots[i].img.sprite = teamSprites[i].sprite;
                teamSlots[i].monsterName = summon.monsterName;
                teamSlots[i].gameObject.SetActive(true);
                teamSlots[i].level = summon.level;
                i++;
            }
        }
        else
        {
            MonsterBase monsterBase = monsterDataBase.GetMonsterBaseByName(monstersHouse.listMonsters[0].monsterName);
            teamSprites[0].sprite = monsterBase.monsterSO.sprite;
            teamSlots[0].img.sprite = teamSprites[0].sprite;
            teamSlots[0].monsterName = monstersHouse.listMonsters[0].monsterName;
            teamSlots[0].level = monstersHouse.listMonsters[0].level;
        }
    }
    
    public void FillOutAcquiredSummons()
    {
        int i = 0;
        foreach (var summon in monstersHouse.listMonsters)
        {
            bool isOnTeam = false;
            foreach (var teamSlot in teamSlots)
            {
                if (teamSlot.monsterName == summon.monsterName)
                {
                    isOnTeam = true;
                    break;
                }
            }
            if (isOnTeam)
            {
                continue;
            }
            else
            {
                MonsterBase monsterBase = monsterDataBase.GetMonsterBaseByName(summon.monsterName);
                inventorySlots[i].monsterName = summon.monsterName;
                inventorySlots[i].img.sprite = monsterBase.monsterSO.sprite;
                inventorySlots[i].gameObject.SetActive(true);
                i++;
            }
        }
    }

    public void FillOutStats(string monsterName, int level)
    {
        MonsterBase monsterBase = monsterDataBase.GetMonsterBaseByName(monsterName);
        MonsterClass monsterClass = new MonsterClass(monsterBase.monsterSO, level);
        txtLvl.text = level.ToString();
        txtName.text = monsterName;
        txtDam.text = monsterClass.PhysicalDamage.ToString();
        txtMDam.text = monsterClass.MagicalDamage.ToString();
        txtDef.text = monsterClass.Defense.ToString();
        txtAsp.text = monsterClass.SpeedAttack.ToString();
        txtMDef.text = monsterClass.MagicalDefense.ToString();
        txtEva.text = monsterClass.Evasion.ToString();
        txtHealth.text = monsterClass.Health.ToString();
        if (summonInvoked != null)
        {
            Destroy(summonInvoked);
        }
        summonInvoked = Instantiate(monsterBase.prefabMonster, spawnSummon.position, Quaternion.identity);
        summonInvoked.GetComponent<Monster>().enabled = false;
        summonInvoked.GetComponentInChildren<Rigidbody>().useGravity = false;
        summonInvoked.AddComponent<CharacterPreviewRotation>();
    }
}
