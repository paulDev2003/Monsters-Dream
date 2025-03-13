using UnityEngine;

[CreateAssetMenu(fileName = "LevelUpSO", menuName = "Scriptable Objects/LevelUpSO")]
public class LevelUpSO : ScriptableObject
{
    public float health;
    public float physicalDamage;
    public float speedAttack;
    public float defense;
    public float evasion;
    public float magicalDamage;
    public float magicalDefense;

    public void LevelUp(Monster monster)
    {
        monster.health += health;
        monster.physicalDamage += physicalDamage;
        monster.speedAttack += speedAttack;
        monster.defense += defense;
        monster.evasion += evasion;
        monster.magicalDamage += magicalDamage;
        monster.magicalDefense += magicalDefense;
    }
}
