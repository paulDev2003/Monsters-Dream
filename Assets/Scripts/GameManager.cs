using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public List<GameObject> friendsList = new List<GameObject>();
    public List<GameObject> enemieList = new List<GameObject>();
    public List<GameObject> enemiesSaved;
    private List<GameObject> friendsSaved;
    public GameObject victoryScreen;
    public Image panelVictory;
    public GameObject defeatScreen;
    public bool finish = false;
    public GameObject textInfoPrefab;
    public GameObject canvasWorld;
    public List<Image> imagesLoot;
    public Image panelLoot;
    public GameObject panelMonsters;
    private Inventory inventory;
    private List<ItemSO> listLoot;
    private GameDataController gameDataController;
    public List<GameObject> lifeBarsFriends = new List<GameObject>();
    public List<UILifeBar> superiorBarFriends = new List<UILifeBar>();
    public List<TextMeshProUGUI> levelFriends = new List<TextMeshProUGUI>();
    public List<GameObject> lifeBarsEnemies = new List<GameObject>();
    public List<UILifeBar> superiorBarEnemies = new List<UILifeBar>();
    public List<TextMeshProUGUI> levelEnemies = new List<TextMeshProUGUI>();
    public List<Image> monsterPanel = new List<Image>();
    public List<MonsterDrop> monsterDrop = new List<MonsterDrop>();
    public List<ExperiencePanel> experienceList = new List<ExperiencePanel>();
    public GameObject selector;
    public int countMonsters = 0;
    public GameObject monsterSelected;
    public GameObject selectorActive;
    public GameObject damageArea;
    public GameObject circleAttacksToSkill;
    public GameObject closestMark;
    public UnityEvent EventVictory;
    public UnityEvent EventLoot;
    public ManagerRunes runeManager;
    public bool expCompleted = false;
    private void Start()
    {
        enemiesSaved = new List<GameObject>(enemieList);
        friendsSaved = new List<GameObject>(friendsList);
        listLoot = new List<ItemSO>();
        gameDataController = FindAnyObjectByType<GameDataController>();
        UpdateInterface();
        if (runeManager != null)
        {
            runeManager.isFigth = true;
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (finish && expCompleted)
            {
                EventLoot.Invoke();
                expCompleted = false;
            }
        }
    }
    public void RemoveFromList(List<GameObject> list, Monster monsterdead)
    {
        for (int i = list.Count - 1; i >= 0; i--) // recorre la lista en reversa
        {
            if (list[i] == monsterdead.gameObject)
            {
                list.RemoveAt(i);
                Debug.Log("Encuentra el igual");
            }
        }
        foreach (var monster in list)
        {
            Debug.Log(monster.name);
        }
    }

    public void CheckIfAnyAlive(List<GameObject> list)
    {
        foreach (var monster in list)
        {
            Monster monsterScript = monster.GetComponent<Monster>();
            if (!monsterScript.dead)
            {
                return;
            }
        }
        DesactiveUI();
        finish = true;
        if (list == enemieList)
        {
            EventVictory.Invoke();
            
        }
        else
        {
            defeatScreen.SetActive(true);
        }
    }
    public void ChangeSelector(GameObject monsterObject)
    {
        if (selectorActive != null)
        {
            Destroy(selectorActive);
        }
        if (monsterSelected == monsterObject)
        {
            monsterSelected = null;
        }
        else
        {
            selectorActive = Instantiate(selector, monsterObject.transform.position + Vector3.up * 2, Quaternion.identity, monsterObject.transform);
            monsterSelected = monsterObject;
        }       
    }
    public void UpdateInterface()
    {
        countMonsters = 0;
        panelMonsters.SetActive(true);
        inventory = FindAnyObjectByType<Inventory>();
        int i = 0;
        foreach (var friend in friendsList)
        {
            lifeBarsFriends[i].SetActive(true);
            Monster script = friend.GetComponent<Monster>();
            script.valueI = i;
            lifeBarsFriends[i].GetComponent<Image>().sprite = script.monsterSO.sprite;
            script.lifeBar = superiorBarFriends[i];
            levelFriends[i].text = $"Lv.{script.level}";
            i++;
            countMonsters++;
        }
        int e = 0;
        foreach (var enemie in enemieList)
        {
            lifeBarsEnemies[e].SetActive(true);
            Monster script = enemie.GetComponent<Monster>();
            lifeBarsEnemies[e].GetComponent<Image>().sprite = script.monsterSO.sprite;
            script.lifeBar = superiorBarEnemies[e];
            levelEnemies[e].text = $"Lv.{script.level}";
            e++;
        }
        i = 0;
        foreach (var monster in inventory.monstersInventory)
        {
            monsterPanel[i].enabled = true;
            Monster monsterComponent = monster.GetComponent<Monster>();
            monsterPanel[i].sprite = monsterComponent.monsterSO.sprite;
            monsterDrop[i].monsterSaved = monster;
            i++;
        }
    }

    public void ShowExperience()
    {
        int totalExp = 0;
        foreach (var monster in enemiesSaved)
        {
            Monster scriptMonster = monster.GetComponent<Monster>();
            totalExp += scriptMonster.monsterSO.expPerLevel * scriptMonster.level;
        }
        Debug.Log($"TotalExp = {totalExp}");
        int i = 0;
        foreach (var monster in inventory.monstersInventory)
        {
            Monster monsterScript = monster.GetComponent<Monster>();
            experienceList[i].ShowPanel(monsterScript, totalExp);
            i++;
        }
        
    }
    public void DesactiveUI()
    {
        panelMonsters.SetActive(false);
        foreach (var lifeBar in lifeBarsFriends)
        {
            lifeBar.SetActive(false);
        }
        foreach (var lifeBar in lifeBarsEnemies)
        {
            lifeBar.SetActive(false);
        }
    }

    public void LootItems()
    {
        int i = 0;
        
        foreach (var enemie in enemiesSaved)
        {
            Monster monsterScript = enemie.GetComponent<Monster>();
            foreach (var drop in monsterScript.monsterSO.dropList)
            {
                bool dropped = drop.RandomDrop();
                if (dropped)
                {
                    imagesLoot[i].sprite = drop.sprite;
                    imagesLoot[i].enabled = true;
                    i++;
                    listLoot.Add(drop);
                    Debug.Log("Drop");
                }
            }
            Debug.Log("Change enemy");
        }
        inventory.Additems(listLoot);
        gameDataController.SaveData();
    }
}
