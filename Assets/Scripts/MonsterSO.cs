using UnityEngine;

[CreateAssetMenu(fileName = "Monster", menuName = "CreateMonster", order = 0)]
public class MonsterSO : ScriptableObject
{
    public string monsterName;
    public int health;
    public int physicalDamage;
    public float speedAttack;
    public int defense;
    public int evasion;
    public int magicalDamage;
    public int magicalDefense;
    public SkillSO skill;
    public LevelUpSO levelUp;

}
