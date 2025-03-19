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

    private void Start()
    {
        enemiesSaved = new List<GameObject>(enemieList);
        friendsSaved = new List<GameObject>(friendsList);
        listLoot = new List<ItemSO>();
        inventory = FindAnyObjectByType<Inventory>();
        gameDataController = FindAnyObjectByType<GameDataController>();
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
