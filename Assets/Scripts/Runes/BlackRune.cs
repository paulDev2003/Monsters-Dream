using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "BlackRune", menuName = "Scriptable Objects/RuneS0/BlackRune")]
public class BlackRune : RuneSO
{
    public float multiplierSpeedAttack;
    public float decreasedCoolDownPercentage;

    public float additionalSA_perLevel;
    public float additionalCDP_perLevel;


    public override void UsePower(List<GameObject> friendList, int level)
    {
        float decreasedCooldown = 0;
        float increasedSpeedAttack = 0;
        if (multiplierSpeedAttack != 0)
        {
            float finalSA = multiplierSpeedAttack + additionalSA_perLevel * (level - 1);
            increasedSpeedAttack = finalSA / 10;
        }
        if (decreasedCoolDownPercentage != 0)
        {
            float finalDC = decreasedCoolDownPercentage + additionalCDP_perLevel * (level - 1); 
            decreasedCooldown = finalDC / 100;
        }
        foreach (var monster in friendList)
        {
            Monster scriptMonster = monster.GetComponent<Monster>();
            scriptMonster.multiplierIncreasedSpeedAttack += increasedSpeedAttack;
            scriptMonster.decreasedCoolDown += decreasedCooldown;
        }
    }

    public override void UsePower(Monster monster, int level)
    {
        float decreasedCooldown = 0;
        float increasedSpeedAttack = 0;
        if (increasedSpeedAttack != 0)
        {
            float finalSA = multiplierSpeedAttack + additionalSA_perLevel * (level - 1);
            increasedSpeedAttack = finalSA / 10;
        }
        if (decreasedCoolDownPercentage != 0)
        {
            float finalDC = decreasedCoolDownPercentage + additionalCDP_perLevel * (level - 1);
            decreasedCooldown = finalDC / 100;
        }
        monster.multiplierIncreasedSpeedAttack += increasedSpeedAttack;
        monster.decreasedCoolDown += decreasedCooldown;
    }
}
