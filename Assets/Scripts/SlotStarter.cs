using UnityEngine;
using UnityEngine.UI;

public class SlotStarter : MonoBehaviour
{
    public MonsterData monsterData;
    public Image starterAuraImg;
    public DungeonManager dungeonManager;

    public void MadeStarter()
    {
        if (dungeonManager.countStarters >= 3 && !monsterData.isStarter || monsterData.currentHealth == 0)
        {
            return;
        }
        if (monsterData.isStarter && dungeonManager.countStarters > 1)
        {
            monsterData.isStarter = false;
            starterAuraImg.enabled = false;
            dungeonManager.countStarters--;
        }
        else if(!monsterData.isStarter)
        {
            monsterData.isStarter = true;
            dungeonManager.countStarters++;
            starterAuraImg.enabled = true;
        }
        dungeonManager.SaveStarters();
    }
}
