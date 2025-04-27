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
        int finalHealth = healthIncreased + additionalHealthPerLevel * (level - 1);
        int finalRegeneration = healthRegeneration + additionalRegenerationPerLevel * (level - 1);
        foreach (var monster in friendList)
        {
            Monster scriptMonster = monster.GetComponent<Monster>();
            scriptMonster.healthBuff += finalHealth;
            scriptMonster.healthRegeneration += finalRegeneration;
        }
    }

    public override void UsePower(Monster monster, int level)
    {
        int finalHealth = healthIncreased + additionalHealthPerLevel * (level - 1);
        int finalRegeneration = healthRegeneration + additionalRegenerationPerLevel * (level - 1);
        monster.healthBuff += finalHealth;
        monster.healthRegeneration += finalRegeneration;
    }

}
