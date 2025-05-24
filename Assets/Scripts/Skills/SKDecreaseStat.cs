using UnityEngine;

[CreateAssetMenu(fileName = "SKDecreaseStat", menuName = "Scriptable Objects/Skills/Decrease Stat")]
public class SKDecreaseStat : SkillSO
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
        IncreaseStat(owner.target);
        owner.specialAttack = false;
    }

    private void IncreaseStat(Monster target)
    {
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
                Debug.Log("Decrementa defensa");
                break;
            
        }
        
    }
}