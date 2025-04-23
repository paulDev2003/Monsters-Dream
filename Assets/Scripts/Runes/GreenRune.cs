using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "GreenRune", menuName = "Scriptable Objects/RuneS0/GreenRune")]
public class GreenRune : RuneSO
{
    public int healthIncreased;
    public int healthRegeneration;

    public int additionalHealthPerLevel;
    public int additionalRegenerationPerLevel;
    public override void UsePower(List<GameObject> friendList, int level)
    {
        int finalHealth = healthIncreased + additionalHealthPerLevel * level;
        int finalRegeneration = healthRegeneration + additionalRegenerationPerLevel * level;
        foreach (var monster in friendList)
        {
            Monster scriptMonster = monster.GetComponent<Monster>();
            scriptMonster.healthBuff += finalHealth;
            scriptMonster.healthRegeneration += finalRegeneration;
        }
    }

    public override void UsePower(Monster monster, int level)
    {
        int finalHealth = healthIncreased + additionalHealthPerLevel * level;
        int finalRegeneration = healthRegeneration + additionalRegenerationPerLevel * level;
        monster.healthBuff += finalHealth;
        monster.healthRegeneration += finalRegeneration;
    }

}
