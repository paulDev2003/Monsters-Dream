using UnityEngine;

[CreateAssetMenu(fileName = "Monster", menuName = "CreateMonster", order = 0)]
public class MonsterSO : ScriptableObject
{
    public string monsterName;
    public float health;
    public float physicalDamage;
    public float speedAttack;
    public float defense;
    public float evasion;
    public float magicalDamage;
    public float magicalDefense;
    public SkillSO skill;
}
