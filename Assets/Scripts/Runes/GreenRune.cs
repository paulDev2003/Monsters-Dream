using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "GreenRune", menuName = "Scriptable Objects/RuneS0/GreenRune")]
public class GreenRune : RuneSO
{
    public int healthIncreased;
    public int healthRegeneration;
    public override void UsePower(List<GameObject> friendList)
    {
        foreach (var monster in friendList)
        {
            Monster scriptMonster = monster.GetComponent<Monster>();
            scriptMonster.healthBuff += healthIncreased;
            scriptMonster.healthRegeneration += healthRegeneration;
        }
    }

    public override void UsePower(Monster monster)
    {
        monster.healthBuff += healthIncreased;
        monster.healthRegeneration += healthRegeneration;
    }
}
