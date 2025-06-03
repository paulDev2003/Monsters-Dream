using UnityEngine;
using System.Collections.Generic;

public class MonstersHouse : MonoBehaviour
{
    public List<MonsterData> listMonsters = new List<MonsterData>();
    public List<DiscoverMonster> bestiary = new List<DiscoverMonster>();
    public List<EggData> eggs = new List<EggData>();
    public int id = 0;

    public void InsertOnMonsterHouse(List<GameObject> monsterList)
    {
        foreach (var monster in monsterList)
        {
            Monster monsterScript = monster.GetComponent<Monster>();
            MonsterData m = new MonsterData()
            {
                baseId = id,
                monsterName = monsterScript.monsterName,
                level = monsterScript.level,
                currentXP = monsterScript.exp,
                currentHealth = monsterScript.healthFigth,
            };
            listMonsters.Add(m);
            id++;
        }
    }

    public void InsertOnMonsterHouse(string monsterName)
    {
        MonsterData m = new MonsterData()
        {
            baseId = listMonsters.Count,
            monsterName = monsterName,
            level = 1,
            currentXP = 0,
            currentHealth = 5,
        };
        listMonsters.Add(m);
        id++;
    }

    public void RemoveEgg(int id)
    {
        foreach (var egg in eggs)
        {
            if (egg.id == id)
            {
                eggs.Remove(egg);
                return;
            }
        }
    }

    public void AddPointsToEggs()
    {
        int pointsGained = PlayerPrefs.GetInt("BattleNumber", 0);
        Debug.Log(pointsGained);
        foreach (var egg in eggs)
        {
            egg.currentPoints += pointsGained;
        }
    }
}
