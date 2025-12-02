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

    public bool dragging = false;
    public bool draggingTeamSlot = false;
    public SummonSlot summonDragging;
    public TeamSlot teamSlotDragging;
    public bool isInPanel = false;
    public Color usedColor;
    

    private void Start()
    {
        
        FillOutImages();
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
            foreach (var teamSlot in teamSlots)
            {
                if (teamSlot.monsterName == summon.monsterName)
                {
                    inventorySlots[i].img.color = usedColor;
                    inventorySlots[i].isUsed = true;
                    break;
                }
            }
            MonsterBase monsterBase = monsterDataBase.GetMonsterBaseByName(summon.monsterName);
            inventorySlots[i].monsterName = summon.monsterName;
            inventorySlots[i].img.sprite = monsterBase.monsterSO.sprite;
            inventorySlots[i].gameObject.SetActive(true);
            i++;
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

    public void CheckIsInPanel()
    {
        isInPanel = true;
        Debug.Log("Entra al panel");
    }

    public void OutOfPanel()
    {
        isInPanel = false;
        Debug.Log("Sale del panel");
    }

    public void AddTeamMember()
    {
        Debug.Log("Click Up");
        if (isInPanel && dragging)
        {
            int i = 0;
            foreach (var teamSlot in teamSlots)
            {
                Debug.Log("Entra al foreach");
                
                if (teamSlot.gameObject.activeSelf == false)
                {
                    teamSlot.img.sprite = summonDragging.img.sprite;
                    teamSlot.monsterName = summonDragging.monsterName;
                    teamSlot.level = summonDragging.level;
                    teamSlot.gameObject.SetActive(true);
                    summonDragging.img.color = usedColor;
                    summonDragging.isUsed = true;
                    dragging = false;
                    summonDragging = null;
                    break;
                }
                i++;
            }
        }
    }

    public void AddTeamMember(SummonSlot summonSlot)
    {
        int i = 0;
        foreach (var teamSlot in teamSlots)
        {

            if (teamSlot.gameObject.activeSelf == false)
            {
                teamSlot.img.sprite = summonSlot.img.sprite;
                teamSlot.monsterName = summonSlot.monsterName;
                teamSlot.level = summonSlot.level;
                teamSlot.gameObject.SetActive(true);
                summonSlot.img.color = usedColor;
                summonSlot.isUsed = true;
                dragging = false;
                summonDragging = null;
                break;
            }
            i++;
        }
    }

    public void DeleteTeamMember(TeamSlot slotDeleted)
    {
        if (slotDeleted.id == 0 && teamSlots[1].gameObject.activeSelf == false)
        {
            return;
        }
        foreach (var summon in inventorySlots)
        {
            if (summon.monsterName == slotDeleted.monsterName)
            {
                summon.isUsed = false;
                summon.img.color = Color.white;
                break;
            }
        }
        teamSlots[slotDeleted.id].gameObject.SetActive(false);
        for (int i = slotDeleted.id; teamSlots[i + 1].gameObject.activeSelf == true; i++)
        {
            teamSlots[i].gameObject.SetActive(true);
            teamSlots[i].img.sprite = teamSlots[i + 1].img.sprite;
            teamSlots[i].monsterName = teamSlots[i + 1].monsterName;
            teamSlots[i].level = teamSlots[i + 1].level;
            teamSlots[i + 1].gameObject.SetActive(false);
        }
    }

    public void ChangeTeamMember(TeamSlot teamSlot)
    {
        if (dragging)
        {
            foreach (var summon in inventorySlots)
            {
                if (summon.monsterName == teamSlot.monsterName)
                {
                    summon.img.color = Color.white;
                    summon.isUsed = false;
                }
            }
            teamSlot.img.sprite = summonDragging.img.sprite;
            teamSlot.monsterName = summonDragging.monsterName;
            teamSlot.level = summonDragging.level;
            summonDragging.img.color = usedColor;
            summonDragging.isUsed = true;
            dragging = false;
            summonDragging = null;
            
        }
        
    }

    public void ChangeTeamSlot(TeamSlot changedSlot)
    {
        if (!draggingTeamSlot || teamSlotDragging == null)
        {
            Debug.Log("Sale por el if");
            return;
        }
        Debug.Log("Llega al change");
        string nameSaved = changedSlot.monsterName;
        int levelSaved = changedSlot.level;
        Sprite spriteSaved = changedSlot.img.sprite;
        changedSlot.monsterName = teamSlotDragging.monsterName;
        changedSlot.level = teamSlotDragging.level;
        changedSlot.img.sprite = teamSlotDragging.img.sprite;
        teamSlotDragging.monsterName = nameSaved;
        teamSlotDragging.level = levelSaved;
        teamSlotDragging.img.sprite = spriteSaved;
        draggingTeamSlot = false;
        teamSlotDragging = null;
    }

    public void FillTeamSprites()
    {
        int i = 0;
        foreach (var summon in teamSlots)
        {
            if (summon.gameObject.activeSelf)
            {
                teamSprites[i].sprite = summon.img.sprite;
                teamSprites[i].gameObject.SetActive(true);
                i++;
            }
        }
    }

}
