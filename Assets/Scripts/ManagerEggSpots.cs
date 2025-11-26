using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.Events;
using UnityEngine.UI;

public class ManagerEggSpots : MonoBehaviour
{
    public List<EggSpot> eggSpots = new List<EggSpot>();
    public MonstersHouse monstersHouse;
    public MonsterDataBase monsterDataBase;
    public Bestiary bestiary;
    public Inventory inventory;
    public EggPanel eggPanel;
    public Transform spawnEgg;
    public Transform spawnPanelEgg;
    public GameObject rightArrow;
    public GameObject leftArrow;
    public List<DiscoverMonster> monstersToEgg = new List<DiscoverMonster>();
    public UnityEvent ActivateEggs;
    public UnityEvent DesactiveEggs;
    public UnityEvent ActiveUI;
    public UnityEvent DesactiveUI;
    public UnityEvent ShowEggPanel;
    public TextMeshProUGUI txtRequirement;
    public Image imgItem;
    [HideInInspector] public GameObject eggInstantiated;
    private int currentEgg = 0;
    private int itemAmount = 0;
    private int amountForEgg;
    private ItemSO itemForEgg;
    [HideInInspector] public bool chooseEgg = false;
     public Egg eggInvoked;


    private void Start()
    {
        foreach (var egg in monstersHouse.eggs)
        {
            MonsterBase monsterBase = monsterDataBase.GetMonsterBaseByName(egg.monsterName);
            GameObject eggPrefab = monsterBase.monsterSO.egg;

            GameObject eggInstantiated = Instantiate(eggPrefab, eggSpots[egg.id].spawnEgg.position, eggPrefab.transform.rotation);
            Egg scriptEgg = eggInstantiated.GetComponent<Egg>();
            eggSpots[egg.id].progressBar.SetActive(true);
            eggSpots[egg.id].imgSuperiorBar.fillAmount = egg.currentPoints / (float)scriptEgg.eggSO.totalPoints;
            scriptEgg.growing = true;
            scriptEgg.managerEggSpots = this;
            scriptEgg.eggSpot = eggSpots[egg.id];
            scriptEgg.eggData = egg;
            if (egg.currentPoints == 0)
            {
                eggSpots[egg.id].imgSuperiorBar.fillAmount = 0.01f;
            }
        }
        CheckEggsAvailable();
    }

    public void CheckEggsAvailable()
    {
        bool eggsAvailable = false;
        foreach (var monster in monstersHouse.bestiary)
        {
            if (monster.wasFriend)
            {
                bool isInList = false;
                foreach (var monsterInLobby in monstersHouse.listMonsters)
                {
                    if (monsterInLobby.monsterName == monster.monsterName)
                    {
                        isInList = true;
                        break;
                    }

                }
                foreach (var eggSpot in eggSpots)
                {
                    if (monster.monsterName == eggSpot.monsterName)
                    {
                        isInList = true;
                        break;
                    }
                }
                if (!isInList)
                {
                    eggsAvailable = true;
                    monstersToEgg.Add(monster);
                }
            }
        }
        if (eggsAvailable)
        {
            ActivateEggs.Invoke();
            FillEggForm();
        }
    }

    private void FillEggForm()
    {
        if (eggInstantiated != null)
        {
            return;
        }
        MonsterBase monsterBase = monsterDataBase.GetMonsterBaseByName(monstersToEgg[0].monsterName);
        FillOutPanel(monsterBase);
        if (monstersToEgg.Count > 1)
        {
            rightArrow.SetActive(true);
        }
    }

    public void ChangeEgg(int i)
    {
        currentEgg = currentEgg + i;
        if (eggInstantiated != null)
        {
            Destroy(eggInstantiated);
        }       
        MonsterBase monsterBase = monsterDataBase.GetMonsterBaseByName(monstersToEgg[currentEgg].monsterName);
        FillOutPanel(monsterBase);
        if (currentEgg == 0)
            leftArrow.SetActive(false);
        else
            leftArrow.SetActive(true);

        if (monstersToEgg.Count <= currentEgg + 1)
            rightArrow.SetActive(false);
        else
            rightArrow.SetActive(true);
    }

    private void FillOutPanel(MonsterBase monsterBase)
    {
        eggInstantiated = Instantiate(monsterBase.monsterSO.egg, spawnEgg.position, monsterBase.monsterSO.egg.transform.rotation);
        ItemSO savedItem = monsterBase.monsterSO.itemForEgg;
        itemForEgg = savedItem;
        imgItem.sprite = savedItem.sprite;
        if (inventory.moleculeInventory.ContainsKey(savedItem.itemName))
        {
            itemAmount = inventory.countMolecules[savedItem.itemName];
        }
        else if (inventory.capturableInventory.ContainsKey(savedItem.itemName))
        {
            itemAmount = inventory.countCapturables[savedItem.itemName];
        }
        amountForEgg = monsterBase.monsterSO.amountForEgg;
        txtRequirement.text = $"{itemAmount} / {amountForEgg}";
    }

    public void CreateEgg()
    {
        bool spotAvailable = false;
        foreach (var eggSpot in eggSpots)
        {
            if (eggSpot.available)
            {
                spotAvailable = true;
            }
        }
        if (!spotAvailable)
        {
            return;
        }
        if (itemAmount < amountForEgg)
        {
            return;
        }       
        else
        {
            itemAmount -= amountForEgg;
            if (inventory.moleculeInventory.ContainsKey(itemForEgg.itemName))
            {
                itemAmount = inventory.countMolecules[itemForEgg.itemName] -= amountForEgg;
            }
            else if (inventory.capturableInventory.ContainsKey(itemForEgg.itemName))
            {
                itemAmount = inventory.countCapturables[itemForEgg.itemName] -= amountForEgg;
            }
            DesactiveUI.Invoke();
            chooseEgg = true;
            Egg scriptEgg = eggInstantiated.GetComponent<Egg>();
            scriptEgg.growing = true;
            scriptEgg.eggData.monsterName = monstersToEgg[currentEgg].monsterName;
            scriptEgg.managerEggSpots = this;
            monstersToEgg.Remove(monstersToEgg[currentEgg]);
            currentEgg = 0;
            leftArrow.SetActive(false);
            if (monstersToEgg.Count == 0)
            {
                DesactiveEggs.Invoke();
            }
            else if (monstersToEgg.Count > 1)
            {
                rightArrow.SetActive(true);
            }
            else
            {
                rightArrow.SetActive(false);
            }
            
        }
    }

    public void InspectEgg()
    {
        GameObject egg = Instantiate(eggInvoked.gameObject, spawnPanelEgg.position, eggInvoked.transform.rotation);
        Egg scriptEgg = egg.GetComponent<Egg>();
        eggPanel.ShowEgg(scriptEgg);
    }
}
