using UnityEngine;

public class BeginDungeon : MonoBehaviour
{
    public GameDataController gameDataController;
    public MonstersHouse monstersHouse;
    public DungeonTeam dungeonTeam;
    public MonsterDataBase monsterDataBase;
    public void InitializeDungeon()
    {
        PlayerPrefs.SetInt("BattleNumber", 0);
        PlayerPrefs.SetInt("MapGenerated", 0);
        dungeonTeam.firstTeam = dungeonTeam.allMonsters;
        EqualMonstersLevel();
    }

    private void EqualMonstersLevel()
    {
        foreach (var monster in dungeonTeam.allMonsters)
        {
            foreach (var data in monstersHouse.listMonsters)
            {
                if (monster.baseId == data.baseId && monster.baseId != 0)
                {
                    monster.level = data.level;
                    monster.currentXP = data.currentXP;
                    MonsterBase monsterBase = monsterDataBase.GetMonsterBaseByName(monster.monsterName);
                    MonsterClass monsterClass = new MonsterClass(monsterBase.monsterSO, monster.level);
                    monster.currentHealth = monsterClass.Health;
                    break;
                }
            }
        }
        gameDataController.SaveData();
    }
}
