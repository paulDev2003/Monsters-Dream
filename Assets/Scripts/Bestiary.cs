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
    public UnityEvent ShowEggPanel;
    public int page = 1;
    public Transform spawnCreateEgg;
    private bool insideTrigger = false;
    public Image btnLeftArrow;
    public Image btnRigthArrow;
    public GameObject eggInstantiated;
    private string savedName;
    public TextMeshProUGUI txtAmount;
    public TextMeshProUGUI txtName;
    public Image imgItem;
    public Camera cameraCreateEggs;
    public Camera cameraEggs;
    private ItemSO savedItem;
    public Sprite unknownMonster;
    private int amountEgg;
    public bool chooseEgg = false;
    private bool eggSpawned = false;
    public Egg eggInvoked;
    public Transform spawnEgg;
    public GameObject txtPressE;
    private bool onPanel = false;

    private void Update()
    {
        if (insideTrigger && Input.GetKeyDown(KeyCode.E) && !onPanel)
        {
            ShowPanel.Invoke();
            onPanel = true;
        }
        if (Input.GetKeyDown(KeyCode.Escape) && onPanel)
        {
            onPanel = false;
            ClosePanel.Invoke();
        }
    }
    public void FillMonstersSlots()
    {
        int i = 0;
        foreach (var monsterBase in monsterDataBase.allMonsters)
        {
            bool found = false;
            if (i < page * 9 && i >= (page - 1) * 9)
            {
                if (!eggSpawned)
                {
                    SpawnEgg(monsterBase);
                    eggSpawned = true;
                }
                monstersSlots[i].monsterData.monsterName = monsterBase.monsterName;
                monstersSlots[i].img.enabled = true;
                
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
                            found = true;
                            //monstersSlots[i].img.color = Color.black;
                        }
                        else if (monsterData.viewed)
                        {
                            monstersSlots[i].img.sprite = monsterBase.monsterSO.sprite;
                            monstersSlots[i].img.color = Color.black;
                            found = true;
                        }
                        i++;
                        break;
                    }
                }

            }
            else if (i >= 9)
            {
                btnRigthArrow.enabled = true;
                
            }
            if (!found)
            {
                i++;
            }

                  
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
        eggInstantiated = Instantiate(monsterBase.monsterSO.egg, spawnCreateEgg.position, monsterBase.monsterSO.egg.transform.rotation);
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
        Egg egg = eggInstantiated.GetComponent<Egg>();
        egg.bestiary = this;
        egg.growing = true;
        egg.eggData.monsterName = savedName;
        ClosePanel.Invoke();
        chooseEgg = true;
        cameraCreateEggs.enabled = true;
    }

    public void InspectEgg()
    {
        GameObject egg = Instantiate(eggInvoked.gameObject, spawnEgg.position, eggInvoked.transform.rotation);
    }

    public void LeftArrow()
    {
        int i = 0;
        btnRigthArrow.enabled = true;
        btnLeftArrow.enabled = false;
        page -= 1;
        foreach (var monsterBase in monsterDataBase.allMonsters)
        {
            if (i < (page - 1) * 9)
            {
                btnLeftArrow.enabled = true;
            }
            if (i < page * 9 && i >= (page - 1) * 9)
            {
                monstersSlots[i - (page - 1) * 9].monsterData.monsterName = monsterBase.monsterName;
                monstersSlots[i - (page - 1) * 9].img.enabled = true;
                monstersSlots[i - (page - 1) * 9].img.sprite = unknownMonster;
                monstersSlots[i - (page - 1) * 9].available = false;
                bool found = false;
                foreach (var monsterData in monstersHouse.listMonsters)
                {
                    if (monsterData.monsterName == monsterBase.monsterName)
                    {
                        monstersSlots[i - (page - 1) * 9].img.sprite = monsterBase.monsterSO.sprite;
                        monstersSlots[i - (page - 1) * 9].available = true;
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
                            monstersSlots[i - (page - 1) * 9].img.sprite = monsterBase.monsterSO.sprite;
                            monstersSlots[i - (page - 1) * 9].available = true;
                          //  monstersSlots[i - (page - 1) * 9].img.color = Color.black;
                        }
                        else if (monsterData.viewed)
                        {
                            monstersSlots[i - (page - 1) * 9].img.sprite = monsterBase.monsterSO.sprite;
                            monstersSlots[i - (page - 1) * 9].img.color = Color.black;
                        }
                        i++;
                        break;
                    }
                }
            }
            i++;
        }
    }
    private void CleanAll()
    {
        foreach (var slot in monstersSlots)
        {
            slot.img.enabled = false;
        }
    }
    public void RigthArrow()
    {
        CleanAll();
        int i = 0;
        btnRigthArrow.enabled = false;
        btnLeftArrow.enabled = true;
        page += 1;
        foreach (var monsterBase in monsterDataBase.allMonsters)
        {
            if (i < page * 9 && i >= (page - 1) * 9)
            {
                monstersSlots[i - (page - 1) * 9].monsterData.monsterName = monsterBase.monsterName;
                monstersSlots[i - (page - 1) * 9].img.enabled = true;
                monstersSlots[i - (page - 1) * 9].img.sprite = unknownMonster;
                monstersSlots[i - (page - 1) * 9].available = false;
                bool found = false;
                foreach (var monsterData in monstersHouse.listMonsters)
                {
                    if (monsterData.monsterName == monsterBase.monsterName)
                    {
                        monstersSlots[i - (page - 1) * 9].img.sprite = monsterBase.monsterSO.sprite;
                        monstersSlots[i - (page - 1) * 9].available = true;
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
                            monstersSlots[i - (page - 1) * 9].img.sprite = monsterBase.monsterSO.sprite;
                            monstersSlots[i - (page - 1) * 9].available = true;
                        }
                        else if (monsterData.viewed)
                        {
                            monstersSlots[i - (page - 1) * 9].img.sprite = monsterBase.monsterSO.sprite;
                            monstersSlots[i - (page - 1) * 9].img.color = Color.black;
                        }
                        i++;
                        break;
                    }
                }
            }
            if (i > page * 9)
            {
                btnRigthArrow.enabled = true;
            }
            i++;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            insideTrigger = true;
            txtPressE.SetActive(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            insideTrigger = false;
            txtPressE.SetActive(false);
        }
    }

    public void DesactiveTrigger()
    {
        insideTrigger = false;
        txtPressE.SetActive(false);
    }
}
