using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "RedRune", menuName = "Scriptable Objects/RuneS0/RedRune")]
public class RedRune : RuneSO
{
    public int damageIncreased;
    public int basicDamageIncreased;

    public int damagePerLevel;
    public int basicDamagePerLevel;
    public override void UsePower(List<GameObject> friendList, int level)
    {
        int finalDamage = damageIncreased + damagePerLevel * level;
        int finalBasicDamage = basicDamageIncreased + basicDamagePerLevel * level;
        foreach (var monster in friendList)
        {
            Monster scriptMonster = monster.GetComponent<Monster>();
            scriptMonster.basicDamageBuff += finalBasicDamage;
            scriptMonster.damageBuff += finalDamage;
        }
    }

    public override void UsePower(Monster monster, int level)
    {
        int finalDamage = damageIncreased + damagePerLevel * level;
        int finalBasicDamage = basicDamageIncreased + basicDamagePerLevel * level;
        monster.basicDamageBuff += finalBasicDamage;
        monster.damageBuff += finalDamage;
    }
}
