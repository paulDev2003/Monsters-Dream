using UnityEngine;

[CreateAssetMenu(fileName = "Increase Stat", menuName = "Scriptable Objects/Skills/Increase Stat")]
public class SKIncreaseStat : SkillSO
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

    public StatType statToIncrease;
    [Tooltip("Cuanto se incrementara la habilidad")]
    public float increaseAmount;

    public override void ShootSkill(Monster owner)
    {
        IncreaseStat(owner);
        owner.specialAttack = false;
    }

    private void IncreaseStat(Monster owner)
    {
        switch (statToIncrease)
        {
            case StatType.Health:
                owner.health += increaseAmount;
                break;
            case StatType.PhysicalDamage:
                owner.physicalDamage += increaseAmount;
                break;
            case StatType.SpeedAttack:
                owner.speedAttack += increaseAmount;
                break;
            case StatType.Defense:
                owner.defense += increaseAmount;
                break;
            case StatType.Evasion:
                owner.evasion += increaseAmount;
                break;
            case StatType.MagicalDamage:
                owner.magicalDamage += increaseAmount;
                break;
            case StatType.MagicalDefense:
                owner.magicalDefense += increaseAmount;
                break;
        }

        Debug.Log($"{statToIncrease} increased by {increaseAmount}");
    }
}
