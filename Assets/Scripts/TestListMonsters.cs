using UnityEngine;
using System.Collections.Generic;

public class TestListMonsters : MonoBehaviour
{
    public MonstersHouse monstersHouse;
    public List<GameObject> prefabMonsters = new List<GameObject>();

    public void InsertOnMonsterHouse()
    {
        monstersHouse.InsertOnMonsterHouse(prefabMonsters);
    }
}
