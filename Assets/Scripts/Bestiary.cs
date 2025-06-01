using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class Bestiary : MonoBehaviour
{
    public List<MonsterSlot> monstersSlots = new List<MonsterSlot>();
    public MonsterDataBase monsterDataBase;
    public MonstersHouse monstersHouse;
    public Inventory inventory;
    public UnityEvent ShowPanel;
    public UnityEvent ClosePanel;
    public int page = 1;
    public Transform spawnEgg;
    private bool insideTrigger = false;
    public GameObject eggInstantiated;
    private string savedName;
    public TextMeshProUGUI txtAmount;
    public TextMeshProUGUI txtName;
    public Image imgItem;
    public Camera cameraEggs;
    private ItemSO savedItem;
    private int amountEgg;
    public bool chooseEgg = false;
    private bool eggSpawned = false;

    private void Update()
    {
        if (insideTrigger && Input.GetKeyDown(KeyCode.E))
        {
            ShowPanel.Invoke();
        }
    }
    public void FillMonstersSlots()
    {
        int i = 0;
        foreach (var monsterBase in monsterDataBase.allMonsters)
        {
            if (i >= page * 9 || i < (page - 1) * 9)
            {
                return;
            }
            if (!eggSpawned)
            {
                SpawnEgg(monsterBase);
                eggSpawned = true;
            }
            monstersSlots[i].monsterData.monsterName = monsterBase.monsterName;
            monstersSlots[i].img.enabled = true;
            bool found = false;
            foreach (var monsterData in monstersHouse.listMonsters)
            {
                if (monsterData.monsterName == monsterBase.monsterName)
                {
                    monstersSlots[i].img.sprite = monsterBase.monsterSO.sprite;
                    monstersSlots[i].available = true;
                    found = true;
                    break;
                }
            }
            if (found)
            {
                i++;
                continue;
            }
            foreach (var monsterData in monstersHouse.bestiary)
            {
                if (monsterData.monsterName == monsterBase.monsterName)
                {
                    if (monsterData.wasFriend)
                    {
                        monstersSlots[i].img.sprite = monsterBase.monsterSO.sprite;
                        monstersSlots[i].available = true;
                        monstersSlots[i].img.color = Color.black;
                    }
                    else if (monsterData.viewed)
                    {
                        monstersSlots[i].img.sprite = monsterBase.monsterSO.sprite;
                        monstersSlots[i].img.color = Color.black;
                    }
                    i++;
                    break;
                }
            }
            i++;
        }
    }

    public void ChangeEgg(MonsterData monsterData)
    {
        Destroy(eggInstantiated);
        MonsterBase monsterBase = monsterDataBase.GetMonsterBaseByName(monsterData.monsterName);
        SpawnEgg(monsterBase);
    }

    private void SpawnEgg(MonsterBase monsterBase)
    {
        eggInstantiated = Instantiate(monsterBase.monsterSO.egg, spawnEgg.position, monsterBase.monsterSO.egg.transform.rotation);
        savedName = monsterBase.monsterName;
        imgItem.sprite = monsterBase.monsterSO.itemForEgg.sprite;
        savedItem = monsterBase.monsterSO.itemForEgg;
        amountEgg = monsterBase.monsterSO.amountForEgg;
        txtAmount.text = amountEgg.ToString();
        txtName.text = savedName;
    }

    public void CreateEgg()
    {
        if (inventory.moleculeInventory.ContainsKey(savedItem.itemName))
        {
            Debug.Log("No es null molecule");
            if (inventory.countMolecules[savedItem.itemName] >= amountEgg)
            {
                Debug.Log("Es mayor");
                inventory.countMolecules[savedItem.itemName] -= amountEgg;
                EggToNursery();
            }
        }
        else if (inventory.capturableInventory.ContainsKey(savedItem.itemName))
        {
            Debug.Log("No es null Capturable");
            if (inventory.countCapturables[savedItem.itemName] >= amountEgg)
            {
                Debug.Log("Es mayor");
                inventory.countCapturables[savedItem.itemName] -= amountEgg;
                EggToNursery();
            }
        }
    }

    private void EggToNursery()
    {
        ClosePanel.Invoke();
        chooseEgg = true;
        cameraEggs.enabled = true;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            insideTrigger = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            insideTrigger = false;
        }
    }
}
