using UnityEngine;
using System.Collections.Generic;

public class MonstersHouse : MonoBehaviour
{
    public List<MonsterData> listMonsters = new List<MonsterData>();
    public int id = 0;

    public void InsertOnMonsterHouse(List<GameObject> monsterList)
    {
        foreach (var monster in monsterList)
        {
            Monster monsterScript = monster.GetComponent<Monster>();
            MonsterData m = new MonsterData()
            {
                id = id,
                monsterName = monsterScript.monsterName,
                level = monsterScript.level,
                currentXP = monsterScript.exp,
                currentHealth = monsterScript.healthFigth,
                monsterPrefab = monster
            };
            listMonsters.Add(m);
            id++;
        }
    }
}
