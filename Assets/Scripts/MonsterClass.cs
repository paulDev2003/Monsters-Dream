using UnityEngine;

public class MonsterClass
{
    MonsterSO monsterSO;
    int level;

    public MonsterClass(MonsterSO scriptMonster, int lvlMonster)
    {
        monsterSO = scriptMonster;
        level = lvlMonster;
    }

    public int Health{
        get { return Mathf.FloorToInt(monsterSO.health + level * monsterSO.levelUp.health); } 
    }
    public int PhysicalDamage
    {
        get { return Mathf.FloorToInt(monsterSO.physicalDamage + level * monsterSO.levelUp.physicalDamage); }
    }
    public int SpeedAttack
    {
        get { return Mathf.FloorToInt(monsterSO.speedAttack + level * monsterSO.levelUp.speedAttack); }
    }
    public int Defense
    {
        get { return Mathf.FloorToInt(monsterSO.defense + level * monsterSO.levelUp.defense); }
    }
    public int Evasion
    {
        get { return Mathf.FloorToInt(monsterSO.evasion + level * monsterSO.levelUp.evasion); }
    }
    public int MagicalDamage
    {
        get { return Mathf.FloorToInt(monsterSO.magicalDamage + level * monsterSO.levelUp.magicalDamage); }
    }
    public int MagicalDefense
    {
        get { return Mathf.FloorToInt(monsterSO.magicalDefense + level * monsterSO.levelUp.health); }
    }
}
