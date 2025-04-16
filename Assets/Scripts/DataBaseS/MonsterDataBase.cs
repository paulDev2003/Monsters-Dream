using UnityEngine;
using System.Collections.Generic; 

[CreateAssetMenu(fileName = "MonsterDataBase", menuName = "Scriptable Objects/MonsterDataBase")]
public class MonsterDataBase : ScriptableObject
{
    public List<MonsterBase> allMonsters;

    private Dictionary<string, MonsterBase> monsterDict;

    public void Initialize()
    {
        monsterDict = new Dictionary<string, MonsterBase>();
        foreach (var monster in allMonsters)
        {
            if (!monsterDict.ContainsKey(monster.monsterName))
                monsterDict.Add(monster.monsterName, monster);
        }
    }

    public MonsterBase GetMonsterBaseByName(string monsterName)
    {
        if (monsterDict == null)
            Initialize();
        return monsterDict.TryGetValue(monsterName, out var monsterBase) ? monsterBase : null;
    }
}
