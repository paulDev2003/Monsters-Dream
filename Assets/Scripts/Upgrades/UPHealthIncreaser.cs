using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UPHealthIncreaser", menuName = "Upgrades/UPHealthIncreaser")]
public class UPHealthIncreaser : Upgrade
{
    public float multiplierToIncrease;

    public override void UseInstantEffectMonsters(List<MonsterData> dungeonTeam)
    {
        foreach (var monster in dungeonTeam)
        {
            monster.currentHealth *= multiplierToIncrease;
        }
    }

    public override void UseUpgrade(Monster monster)
    {
        monster.health *= multiplierToIncrease;
    }


    public override void UseInstantEffectRunes(List<Rune> allRunes)
    {
        throw new System.NotImplementedException();
    }

}
