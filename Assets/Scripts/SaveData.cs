using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class SaveData
{
    public List<string> capturableKeys;
    public List<string> capturableIDs;
    public List<int> capturableAmount;

    public List<string> molecularKeys;
    public List<string> molecularIDs;
    public List<int> molecularAmount;

    public List<MonsterData> monstersHouse;
    public List<MonsterData> monstersDungeon;
}
