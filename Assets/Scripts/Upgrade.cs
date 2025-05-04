using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public abstract class Upgrade : ScriptableObject
{
    public string upgradeName;
    public int cost;
    public string txtInfo;
    public Sprite sprite;
    public bool permanentEffect;
    public bool instantEffectMonsters;
    public bool instantEffectRunes;
    public abstract void UseUpgrade(Monster monster);
    public abstract void UseInstantEffectMonsters(List<MonsterData> dungeonTeam);

    public abstract void UseInstantEffectRunes(List<Rune> allRunes);

}
