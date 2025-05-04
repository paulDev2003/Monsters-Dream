using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UPShieldincreaser", menuName = "Upgrades/UPShieldincreaser")]
public class UPShieldincreaser : Upgrade
{
    public float multiplierToIncrease;
    public override void UseUpgrade(Monster monster)
    {
        monster.shield *= multiplierToIncrease;
    }


    public override void UseInstantEffectMonsters(List<MonsterData> dungeonTeam)
    {
        throw new System.NotImplementedException();
    }

    public override void UseInstantEffectRunes(List<Rune> allRunes)
    {
        throw new System.NotImplementedException();
    }

}
