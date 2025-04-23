using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "BlueRune", menuName = "Scriptable Objects/RuneS0/BlueRune")]
public class BlueRune : RuneSO
{
    public int magicDamageIncreased;

    public int additionalDamagePerLevel;
    
    public override void UsePower(List<GameObject> friendList, int level)
    {
        int finalDamage = magicDamageIncreased + additionalDamagePerLevel * level;
        foreach (var monster in friendList)
        {
            Monster scriptMonster = monster.GetComponent<Monster>();
            scriptMonster.magicDamageBuff += finalDamage;
        }
    }

    public override void UsePower(Monster monster, int level)
    {
        int finalDamage = magicDamageIncreased + additionalDamagePerLevel * level;
        monster.magicDamageBuff += finalDamage;
    }

}