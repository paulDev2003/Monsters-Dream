using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "GreyRune", menuName = "Scriptable Objects/RuneS0/GreyRune")]
public class GreyRune : RuneSO
{
    public int physicIncreaseDefense;
    public int magicIncreaseDefense;
    public override void UsePower(List<GameObject> friendList)
    {
        foreach (var monster in friendList)
        {
            Monster scriptMonster = monster.GetComponent<Monster>();
            scriptMonster.physicIncreaseDefense += physicIncreaseDefense;
            scriptMonster.magicIncreaseDefense += magicIncreaseDefense;
        }
    }

    public override void UsePower(Monster monster)
    {
        monster.physicIncreaseDefense += physicIncreaseDefense;
        monster.magicIncreaseDefense += magicIncreaseDefense;
    }
}
