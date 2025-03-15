using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "SKIncreaseStatsAll", menuName = "Scriptable Objects/Skills/IncreaseStatsAll")]
public class SKIncreaseStatsAll : SkillSO
{
    public enum StatType
    {
        Health,
        PhysicalDamage,
        SpeedAttack,
        Defense,
        Evasion,
        MagicalDamage,
        MagicalDefense,
    }

    public StatType statsToIncrease;
    [Tooltip("Cuanto se incrementara la habilidad")]
    public float increaseAmount;

    public override void ShootSkill(Monster owner)
    {
        IncreaseStat(owner.ownList);
    }

    private void IncreaseStat(List<GameObject> ownList)
    {
        foreach (GameObject monster in ownList)
        {
            Monster monsterStat = monster.GetComponent<Monster>();
            switch (statsToIncrease)
            {
                case StatType.Health:
                    monsterStat.health += increaseAmount;
                    break;
                case StatType.PhysicalDamage:
                    monsterStat.physicalDamage += increaseAmount;
                    break;
                case StatType.SpeedAttack:
                    monsterStat.speedAttack += increaseAmount;
                    break;
                case StatType.Defense:
                    monsterStat.defense += increaseAmount;
                    break;
                case StatType.Evasion:
                    monsterStat.evasion += increaseAmount;
                    break;
                case StatType.MagicalDamage:
                    monsterStat.magicalDamage += increaseAmount;
                    break;
                case StatType.MagicalDefense:
                    monsterStat.magicalDefense += increaseAmount;
                    break;
            }
        
        }

        
    }
}
