using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "GreyRune", menuName = "Scriptable Objects/RuneS0/GreyRune")]
public class GreyRune : RuneSO
{
    public int physicIncreaseDefense;
    public int magicIncreaseDefense;

    public int additionalPhysicPerLevel;
    public int additionalMagicPerLevel;
    public override void UsePower(List<GameObject> friendList, int level)
    {
        int finalPhysic = physicIncreaseDefense + additionalPhysicPerLevel * (level - 1);
        int finalMagic = magicIncreaseDefense + additionalMagicPerLevel * (level - 1);
        foreach (var monster in friendList)
        {
            Monster scriptMonster = monster.GetComponent<Monster>();
            scriptMonster.physicIncreaseDefense += finalPhysic;
            scriptMonster.magicIncreaseDefense += finalMagic;
        }
    }

    public override void UsePower(Monster monster, int level)
    {
        int finalPhysic = physicIncreaseDefense + additionalPhysicPerLevel * (level - 1);
        int finalMagic = magicIncreaseDefense + additionalMagicPerLevel * (level - 1);
        monster.physicIncreaseDefense += finalPhysic;
        monster.magicIncreaseDefense += finalMagic;
    }

}
