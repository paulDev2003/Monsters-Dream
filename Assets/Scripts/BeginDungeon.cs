using UnityEngine;

public class BeginDungeon : MonoBehaviour
{
    public GameDataController gameDataController;
    public MonstersHouse monstersHouse;
    public DungeonTeam dungeonTeam;
    public void InitializeDungeon()
    {
        PlayerPrefs.SetInt("BattleNumber", 0);
        PlayerPrefs.SetInt("MapGenerated", 0);
        EqualMonstersLevel();
    }

    private void EqualMonstersLevel()
    {
        foreach (var monster in dungeonTeam.allMonsters)
        {
            foreach (var data in monstersHouse.listMonsters)
            {
                if (monster.baseId == data.baseId)
                {
                    monster.level = data.level;
                    monster.currentXP = data.currentXP;
                    break;
                }
            }
        }
        gameDataController.SaveData();
    }
}
