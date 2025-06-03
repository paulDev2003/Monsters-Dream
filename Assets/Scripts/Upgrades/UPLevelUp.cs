using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "UPLevelUp", menuName = "Upgrades/UPLevelUp")]
public class UPLevelUp : Upgrade
{
    public MonsterDataBase monsterDataBase;

    public override void UseInstantEffectMonsters(List<MonsterData> dungeonTeam)
    {
        int i = 0;
        ContainerExperiencePanel contExpPanel = FindAnyObjectByType<ContainerExperiencePanel>();
        contExpPanel.gameObject.GetComponent<Image>().enabled = true;
        contExpPanel.btnPanel.enabled = true;
        foreach (var monster in dungeonTeam)
        {
            MonsterBase monsterBase = monsterDataBase.GetMonsterBaseByName(monster.monsterName);
            int totalExp = monster.level * 50;
            if (monster.monsterName != "")
            {
                MonsterData updatedData = contExpPanel.listExperienciePanel[i].ShowPanel(monsterBase.monsterSO, totalExp, monster);
                monster.level = updatedData.level;
                monster.currentXP = updatedData.currentXP;
                i++;

            }
            
        }
    }

    public override void UseInstantEffectRunes(List<Rune> allRunes)
    {
        throw new System.NotImplementedException();
    }

    public override void UseUpgrade(Monster monster)
    {
        throw new System.NotImplementedException();
    }
}
