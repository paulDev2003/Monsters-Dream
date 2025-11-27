using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class ManagerProgression : MonoBehaviour
{
    public MonstersHouse monstersHouse;
    public List<UnlockedSummonUI> summons = new List<UnlockedSummonUI>();
    private int summonsUnlocked = 0;
    public TextMeshProUGUI txtSummonsUnlocked;

    private void Start()
    {
        CheckMonstersUnlocked();
    }

    public void CheckMonstersUnlocked()
    {
        foreach (var summon in summons)
        {
            foreach (var monster in monstersHouse.bestiary)
            {
                if (monster.monsterName == summon.monsterName)
                {
                    if (monster.wasFriend)
                    {
                        summonsUnlocked++;
                        summon.ActivateCard();
                    }                    
                    break;
                }
            }
        }
        txtSummonsUnlocked.text = $"{summonsUnlocked} / {summons.Count}";

    }

}
