using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "RedRune", menuName = "Scriptable Objects/RuneS0/RedRune")]
public class RedRune : RuneSO
{
    public int damageIncreased;
    public int basicDamageIncreased;
    public override void UsePower(List<GameObject> friendList)
    {
        foreach (var monster in friendList)
        {
            Monster scriptMonster = monster.GetComponent<Monster>();
            scriptMonster.basicDamageBuff += basicDamageIncreased;
            scriptMonster.damageBuff += damageIncreased;
        }
    }
}
