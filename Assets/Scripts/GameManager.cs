using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public List<GameObject> enemiesToChoice = new List<GameObject>();
    public Vector2Int levelToSpawn;
    public List<GameObject> friendsList = new List<GameObject>();
    public List<GameObject> enemieList = new List<GameObject>();
    public List<GameObject> enemiesSaved;
    private List<GameObject> friendsSaved;
    public Vector2Int totalEnemiesToSpawn;
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
    public List<UIShieldBar> shieldsFriends = new List<UIShieldBar>();
    public List<TextMeshProUGUI> levelFriends = new List<TextMeshProUGUI>();
    public List<GameObject> lifeBarsEnemies = new List<GameObject>();
    public List<UILifeBar> superiorBarEnemies = new List<UILifeBar>();
    public List<UIShieldBar> shieldsEnemies = new List<UIShieldBar>();
    public List<TextMeshProUGUI> levelEnemies = new List<TextMeshProUGUI>();
    public List<SkillDrop> attacksPanel = new List<SkillDrop>();
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
    public List<Transform> friendSpawnPoints = new List<Transform>();
    public List<Transform> enemySpawnPoints = new List<Transform>();
    public DungeonTeam dungeonTeam;
    public MonsterDataBase monsterDataBase;
    private List<Monster> friendsMonsters = new List<Monster>();
    public TextMeshProUGUI txtMoney;
    public TextMeshProUGUI txtMoneyReward;
    public Image imageMoneyReward;
    public bool specialEvent = false;
    private void Start()
    { 
        friendsSaved = new List<GameObject>(friendsList);
        listLoot = new List<ItemSO>();
        gameDataController = FindAnyObjectByType<GameDataController>();
        SpawnMonsters();
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

    public void SpawnMonsters()
    {
        int countEnemies = Random.Range(totalEnemiesToSpawn.x, totalEnemiesToSpawn.y);
        for (int i = 0; i < countEnemies; i++)
        {
            GameObject enemyToSpawn = enemiesToChoice[Random.Range(0, enemiesToChoice.Count)];
            GameObject enemySpawned = Instantiate(enemyToSpawn, enemySpawnPoints[i].position, Quaternion.identity);
            Monster enemyScript = enemyToSpawn.GetComponent<Monster>();
            enemyScript.level = Random.Range(levelToSpawn.x, levelToSpawn.y);
            enemyScript.enemie = true;
            enemieList.Add(enemySpawned);
        }
        enemiesSaved = new List<GameObject>(enemieList);
        int e = 0;
        foreach (var monster in dungeonTeam.allMonsters)
        {
            if (monster.isStarter)
            {
                MonsterBase monsterBase = monsterDataBase.GetMonsterBaseByName(monster.monsterName);
                GameObject friendSpawned = Instantiate(monsterBase.prefabMonster, friendSpawnPoints[e].position, Quaternion.identity);
                Monster scriptMonster = friendSpawned.GetComponent<Monster>();
                scriptMonster.enemie = false;
                scriptMonster.level = monster.level;
                scriptMonster.monsterData = monster;
                scriptMonster.wasSpawned = true;
                scriptMonster.valueI = e;
                friendsMonsters.Add(scriptMonster);
                friendsList.Add(friendSpawned);
                scriptMonster.skillDrop = attacksPanel[e];
                attacksPanel[e].skill = scriptMonster.monsterSO.skill;
                attacksPanel[e].targetImage.enabled = true;               
                if (scriptMonster.monsterSO.skill.sprite != null)
                {
                    attacksPanel[e].targetImage.sprite = scriptMonster.monsterSO.skill.sprite;
                }
                e++;
            }
        }
        UpdateInterface();
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
            if (monsterScript == null)
            {
                monsterScript = monster.GetComponentInChildren<Monster>();
            }
            if (!monsterScript.dead)
            {
                return;
            }
        }        
        if (list == enemieList)
        {
            if (!specialEvent)
            {
                DesactiveUI();
                finish = true;
                EventVictory.Invoke();
            }       
        }
        else
        {
            DesactiveUI();
            finish = true;
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
        AntiBugEnemie();
        countMonsters = 0;
        panelMonsters.SetActive(true);
        inventory = FindAnyObjectByType<Inventory>();
        int i = 0;
        foreach (var monster in inventory.monstersInventory)
        {
            lifeBarsFriends[i].SetActive(true);
            Monster monsterComponent = monster.GetComponent<Monster>();
            lifeBarsFriends[i].GetComponent<Image>().sprite = monsterComponent.monsterSO.sprite;
            monsterDrop[i].monsterSaved = monster;
            monsterDrop[i].valueI = i;
            
            foreach (var monsterPrefab in friendsList)
            {
                Monster monsterScript = monsterPrefab.GetComponent<Monster>();
                if (monsterScript.monsterName == monsterComponent.monsterName)
                {
                    monsterDrop[i].instantiatedMonster = monsterPrefab;
                    monsterDrop[i].monsterScript = monsterScript;
                    monsterScript.lifeBar = superiorBarFriends[i];
                    monsterScript.shieldBar = shieldsFriends[i];
                }
            }
            foreach (var monsterData in dungeonTeam.allMonsters)
            {
                
                if (monsterData.monsterName == monsterComponent.monsterName)
                {
                    levelFriends[i].text = $"Lv.{monsterData.level}";
                    monsterComponent.monsterData = monsterData;
                    monsterDrop[i].monsterData = monsterData;
                    if (monsterData.isStarter)
                    {
                        monsterDrop[i].isUsed = true;
                    }
                }
            }
            i++;
        }
        i = 0;
        foreach (var friend in friendsList)
        {
            countMonsters++;
        }
        int e = 0;
        foreach (var enemie in enemieList)
        {
            Monster scriptMonster = enemie.GetComponent<Monster>();
            if (scriptMonster.lifeBar == null)
            {
                lifeBarsEnemies[e].SetActive(true);
                Monster script = enemie.GetComponent<Monster>();
                lifeBarsEnemies[e].GetComponent<Image>().sprite = script.monsterSO.sprite;
                script.lifeBar = superiorBarEnemies[e];
                script.shieldBar = shieldsEnemies[e];
                levelEnemies[e].text = $"Lv.{script.level}";
                e++;
            }            
        }
        
    }

    public void ShowExperience()
    {
        int money = PlayerPrefs.GetInt("Money", 0);
        txtMoney.text = money.ToString();
        int totalExp = 0;
        foreach (var monster in enemiesSaved)
        {
            Monster scriptMonster = monster.GetComponent<Monster>();
            totalExp += scriptMonster.monsterSO.expPerLevel * scriptMonster.level;
        }
        Debug.Log($"TotalExp = {totalExp}");
        int i = 0;
        foreach (var monster in dungeonTeam.allMonsters)
        {
            MonsterBase monsterBase = monsterDataBase.GetMonsterBaseByName(monster.monsterName);
            if (monster.monsterName != "")
            {
                MonsterData updatedData = experienceList[i].ShowPanel(monsterBase.monsterSO, totalExp, monster);
                monster.level = updatedData.level;
                monster.currentXP = updatedData.currentXP;
                i++;

            }
            
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
        int moneyReward = 0;
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
                }
            }           
            moneyReward += monsterScript.monsterSO.moneyPerLevel * monsterScript.level;
        }
        txtMoneyReward.enabled = true;
        imageMoneyReward.enabled = true;
        txtMoneyReward.text = moneyReward.ToString();
        int totalMoney = PlayerPrefs.GetInt("Money", 0);
        totalMoney += moneyReward;
        PlayerPrefs.SetInt("Money", totalMoney);
        SaveStats();
        inventory.Additems(listLoot);
        int battleNumber = PlayerPrefs.GetInt("BattleNumber");
        battleNumber++;
        PlayerPrefs.SetInt("BattleNumber", battleNumber);
    }

    private void SaveStats()
    {
        foreach (var monsterDropped in monsterDrop)
        {
            if (monsterDropped.instantiatedMonster != null)
            {
                Monster monsterScript = monsterDropped.instantiatedMonster.GetComponent<Monster>();
                MonsterData monsterDataDrop = monsterScript.monsterData;
                foreach (var monsterData in dungeonTeam.allMonsters)
                {
                    if (monsterData.monsterName == monsterDataDrop.monsterName)
                    {
                        monsterData.currentHealth = monsterScript.healthFigth;
                        //monsterData.currentXP = monsterScript.exp;
                        //monsterData.level = monsterScript.level;
                    }
                }
            }
        }
    }

    private void AntiBugEnemie()
    {
        foreach (var enemie in enemieList)
        {
            enemie.GetComponent<Monster>().enemie = true;
        }
    }
}
