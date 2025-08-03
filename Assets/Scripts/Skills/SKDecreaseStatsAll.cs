using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "SKDecreaseStatsAll", menuName = "Scriptable Objects/Skills/Decrease Stats All")]
public class SKDecreaseStatsAll : SkillSO
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

    public StatType statToDecrease;
    [Tooltip("Cuanto se disminuye la habilidad")]
    public float decreaseAmount;

    public override void ShootSkill(Monster owner)
    {
        DecreaseStat(owner.oppositeList);
        owner.monsterStateMachine.ChangeState(owner.monsterBasicAttackState);
    }

    private void DecreaseStat(List<GameObject> oppositeList)
    {
        foreach (var monster in oppositeList)
        {
            Monster target = monster.GetComponent<Monster>();
            switch (statToDecrease)
            {
                case StatType.Health:
                    target.health -= decreaseAmount;
                    break;
                case StatType.PhysicalDamage:
                    target.physicalDamage -= decreaseAmount;
                    break;
                case StatType.SpeedAttack:
                    target.speedAttack -= decreaseAmount;
                    if (target.speedAttack <= 0.1)
                    {
                        target.speedAttack = 0.1f;
                    }
                    break;
                case StatType.Defense:
                    target.defense -= decreaseAmount;
                    break;
                case StatType.Evasion:
                    target.evasion -= decreaseAmount;
                    if (target.evasion < 0)
                    {
                        target.evasion = 0;
                    }
                    break;
                case StatType.MagicalDamage:
                    target.magicalDamage -= decreaseAmount;
                    break;
                case StatType.MagicalDefense:
                    target.magicalDefense -= decreaseAmount;
                    break;
            }
       

        }
    }
}
