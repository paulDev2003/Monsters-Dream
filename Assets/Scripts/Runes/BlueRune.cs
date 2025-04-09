using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "BlueRune", menuName = "Scriptable Objects/RuneS0/BlueRune")]
public class BlueRune : RuneSO
{
    public int magicDamageIncreased;
    
    public override void UsePower(List<GameObject> friendList)
    {
        foreach (var monster in friendList)
        {
            Monster scriptMonster = monster.GetComponent<Monster>();
            scriptMonster.magicDamageBuff += magicDamageIncreased;
        }
    }

    public override void UsePower(Monster monster)
    {
        monster.magicDamageBuff += magicDamageIncreased;
    }
}