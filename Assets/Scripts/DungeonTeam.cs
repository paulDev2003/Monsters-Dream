using UnityEngine;
using System.Collections.Generic;

public class DungeonTeam : MonoBehaviour
{
    public List<MonsterData> allMonsters = new List<MonsterData>();

    public void CheckStartersAlive()
    {
        foreach (var monster in allMonsters)
        {
            if (monster.currentHealth <= 0)
            {
                monster.isStarter = false;
            }
        }
    }
}
