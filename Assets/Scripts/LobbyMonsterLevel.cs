using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
public class LobbyMonsterLevel : MonoBehaviour
{
    public List<Image> monstersImg = new List<Image>();
    public MonstersHouse monstersHouse;
    public MonsterDataBase monsterDataBase;
    public Inventory inventory;
    public int pageNumber = 1;
    public UnityEvent EnabledInterface;
    public UnityEvent DisabledInterface;
    public Transform spawnRender;
    private GameObject monsterPrefab;
    private bool insideCollider = false;
    private bool monsterSpawned = false;
    public TextMeshProUGUI txtDam;
    public TextMeshProUGUI txtMDam;
    public TextMeshProUGUI txtDef;
    public TextMeshProUGUI txtASp;
    public TextMeshProUGUI txtMDef;
    public TextMeshProUGUI txtEva;
    public TextMeshProUGUI txtHealth;
    public TextMeshProUGUI txtLevel;
    public TextMeshProUGUI txtItemAmount;
    public TextMeshProUGUI txtMonsterName;
    private MonsterData currentMonster;
    public Image imgItem;
    public Image superiorBarExp;
    public int feedAmount = 25;
    private ItemSO itemForFeed;
    private int amountItem;
    private bool onInterface = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && insideCollider && !onInterface)
        {
            onInterface = true;
            EnabledInterface.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.Escape) && onInterface)
        {
            DisabledInterface.Invoke();
            onInterface = false;
        }
    }
    public void FillImages()
    {
        int i = 0;
        foreach (var monster in monstersHouse.listMonsters)
        {
            if (i < pageNumber * 9 && i >= (pageNumber - 1) * 9)
            {               
                monstersImg[i].enabled = true;
                MonsterBase monsterBase = monsterDataBase.GetMonsterBaseByName(monster.monsterName);
                monstersImg[i].sprite = monsterBase.monsterSO.sprite;
                monstersImg[i].GetComponent<MonsterSlot>().monsterData = monster;
                if (!monsterSpawned)
                {
                    monsterPrefab = Instantiate(monsterBase.prefabMonster, spawnRender.position, spawnRender.transform.rotation);
                    MonsterClass monsterClass = new MonsterClass(monsterBase.monsterSO, monster.level);
                    txtLevel.text = monster.level.ToString();
                    txtMonsterName.text = monster.monsterName;
                    imgItem.sprite = monsterBase.monsterSO.itemForUpLevel.sprite;
                    itemForFeed = monsterBase.monsterSO.itemForUpLevel;
                    amountItem = monster.level * 5;
                    txtItemAmount.text = amountItem.ToString();
                    float maxExp = 50 * monster.level;
                    superiorBarExp.fillAmount = monster.currentXP / maxExp;
                    currentMonster = monster;
                    ShowStats(monsterClass);
                    InstantiateMonsterRender();
                }
            }
            i++;
        }
    }

    private void ShowStats(MonsterClass monsterClass)
    {
        txtDam.text = monsterClass.PhysicalDamage.ToString();
        txtMDam.text = monsterClass.MagicalDamage.ToString();
        txtDef.text = monsterClass.Defense.ToString();
        txtASp.text = monsterClass.SpeedAttack.ToString();
        txtMDef.text = monsterClass.MagicalDefense.ToString();
        txtEva.text = monsterClass.Evasion.ToString();
        txtHealth.text = monsterClass.Health.ToString();
        
    }

    public void ChangeMonsterPrefab(MonsterData monsterData)
    {
        MonsterBase monsterBase = monsterDataBase.GetMonsterBaseByName(monsterData.monsterName);
        Destroy(monsterPrefab);
        monsterPrefab = Instantiate(monsterBase.prefabMonster, spawnRender.position, spawnRender.transform.rotation);
        MonsterClass monsterClass = new MonsterClass(monsterBase.monsterSO, monsterData.level);
        txtLevel.text = monsterData.level.ToString();
        imgItem.sprite = monsterBase.monsterSO.itemForUpLevel.sprite;
        itemForFeed = monsterBase.monsterSO.itemForUpLevel;
        amountItem = monsterData.level * 5;
        txtItemAmount.text = amountItem.ToString();
        txtMonsterName.text = monsterData.monsterName;
        float maxExp = 50 * monsterData.level;
        superiorBarExp.fillAmount = monsterData.currentXP / maxExp;
        currentMonster = monsterData;
        ShowStats(monsterClass);
        InstantiateMonsterRender();
    }

    private void InstantiateMonsterRender()
    {
        monsterSpawned = true;
        monsterPrefab.GetComponent<Monster>().enabled = false;
        Rigidbody rb = monsterPrefab.GetComponentInChildren<Rigidbody>();
        rb.useGravity = false;
        Transform firstChild = monsterPrefab.transform.GetChild(0);
        firstChild.gameObject.AddComponent<CharacterPreviewRotation>();
        
    }

    public void FeedMonster()
    {
    
        if (inventory.moleculeInventory.ContainsKey(itemForFeed.itemName))
        {
            Debug.Log("No es null molecule");
            if (inventory.countMolecules[itemForFeed.itemName] >= amountItem)
            {
                Debug.Log("Es mayor");
                inventory.countMolecules[itemForFeed.itemName] -= amountItem;
                ApplyFeed();
            }
        }
        else if(inventory.capturableInventory.ContainsKey(itemForFeed.itemName))
        {
            Debug.Log("No es null Capturable");
            if (inventory.countCapturables[itemForFeed.itemName] >= amountItem)
            {               
                Debug.Log("Es mayor");
                inventory.countCapturables[itemForFeed.itemName] -= amountItem;
                ApplyFeed();
            }
        }
        
    }

    private void ApplyFeed()
    {
        currentMonster.currentXP += feedAmount;
        int maxExp = currentMonster.level * 50;
        if (currentMonster.currentXP >= maxExp)
        {
            currentMonster.currentXP -= maxExp;
            currentMonster.level += 1;
            txtLevel.text = currentMonster.level.ToString();
        }
        superiorBarExp.fillAmount = (float)currentMonster.currentXP / (float)maxExp;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            insideCollider = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            insideCollider = false;
        }
    }
}
