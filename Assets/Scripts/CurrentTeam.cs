using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class CurrentTeam : MonoBehaviour
{
    public MonstersHouse monstersHouse;
    public GameObject[] iconsList = new GameObject[6];
    public TextMeshProUGUI[] textsList = new TextMeshProUGUI[6];
    public List<GameObject> allIcons = new List<GameObject>();
    public List<TextMeshProUGUI> allTexts = new List<TextMeshProUGUI>();
    public int i = 0;

    public void ActivateAllIcons()
    {
        int e = 0;
        foreach (var monster in monstersHouse.listMonsters)
        {
            Monster monsterScript = monster.monsterPrefab.GetComponent<Monster>();
            allIcons[e].SetActive(true);
            allIcons[e].GetComponent<Image>().sprite = monsterScript.monsterSO.sprite;
            allTexts[e].text = monster.monsterName;
            e++;
        }
    }
    public void AddMonster(MonsterData monsterData)
    {
        Monster monsterScript = monsterData.monsterPrefab.GetComponent<Monster>();
        iconsList[i].SetActive(true);
        iconsList[i].GetComponent<Image>().sprite = monsterScript.monsterSO.sprite;
        textsList[i].text = monsterData.monsterName;
        i++;
    }
}
