using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "BlackRune", menuName = "Scriptable Objects/RuneS0/BlackRune")]
public class BlackRune : RuneSO
{
    public float multiplierSpeedAttack;
    public float decreasedCoolDownPercentage;
    public override void UsePower(List<GameObject> friendList)
    {
        float decreasedCooldown = 0;
        float increasedSpeedAttack = 0;
        if (increasedSpeedAttack != 0)
        {
            increasedSpeedAttack = multiplierSpeedAttack / 10;
        }
        if (decreasedCoolDownPercentage != 0)
        {
            decreasedCooldown = decreasedCoolDownPercentage / 100;
        }
        foreach (var monster in friendList)
        {
            Monster scriptMonster = monster.GetComponent<Monster>();
            scriptMonster.multiplierIncreasedSpeedAttack += multiplierSpeedAttack;
            scriptMonster.decreasedCoolDown += decreasedCooldown;
        }
    }

    public override void UsePower(Monster monster)
    {
        float decreasedCooldown = 0;
        float increasedSpeedAttack = 0;
        if (increasedSpeedAttack != 0)
        {
            increasedSpeedAttack = multiplierSpeedAttack / 10;
        }
        if (decreasedCoolDownPercentage != 0)
        {
            decreasedCooldown = decreasedCoolDownPercentage / 100;
        }
        monster.multiplierIncreasedSpeedAttack += multiplierSpeedAttack;
        monster.decreasedCoolDown += decreasedCooldown;
    }
}
