using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using TMPro;

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

    private void Start()
    {
        enemiesSaved = new List<GameObject>(enemieList);
        friendsSaved = new List<GameObject>(friendsList);
        listLoot = new List<ItemSO>();
        inventory = FindAnyObjectByType<Inventory>();
        gameDataController = FindAnyObjectByType<GameDataController>();
        int i = 0;
        foreach (var friend in friendsList)
        {
            lifeBarsFriends[i].SetActive(true);
            Monster script = friend.GetComponent<Monster>();
            lifeBarsFriends[i].GetComponent<Image>().sprite = script.monsterSO.sprite;
            script.lifeBar = superiorBarFriends[i];
            levelFriends[i].text = $"Lv.{script.level}";
            i++;
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
        finish = true;
        foreach (var lifeBar in lifeBarsFriends)
        {
            lifeBar.SetActive(false);
        }
        foreach (var lifeBar in lifeBarsEnemies)
        {
            lifeBar.SetActive(false);
        }
        if (list == enemieList)
        {
            victoryScreen.SetActive(true);
            panelVictory.enabled = true;
            panelLoot.enabled = true;
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
        else
        {
            defeatScreen.SetActive(true);
        }
    }
}
