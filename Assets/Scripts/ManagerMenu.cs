using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections.Generic;

public class ManagerMenu : MonoBehaviour
{
    public MonstersHouse monstersHouse;
    public DungeonTeam dungeonTeam;
    private bool saved = false;
    public Button btnLoadGame;
    public Image imgLoadGame;
    public Color disabledColor;
    public GameObject emergentWindow;
    public UnityEvent EventNewGame;
    public List<MonsterData> newMonsters = new List<MonsterData>();
    void Start()
    {
        if (monstersHouse.listMonsters.Count > 0)
        {
            if (monstersHouse.listMonsters[0].monsterName != "")
            {
                saved = true;              
            }
            else
            {
                btnLoadGame.enabled = false;
                imgLoadGame.color = disabledColor;
            }
        }
        else
        {
            btnLoadGame.enabled = false;
            imgLoadGame.color = disabledColor;
        }
    }

    public void NewGame()
    {
        PlayerPrefs.SetInt("BattleNumber", 0);
        if (saved)
        {
            emergentWindow.SetActive(true);
        }
        else
        {
            EventNewGame.Invoke();
        }
    }

    public void AddNewMonsters()
    {
        dungeonTeam.allMonsters = new List<MonsterData>() { newMonsters[0], newMonsters[1], null, null, null, null };
        //int i = 0;
        /*
        foreach (var monster in newMonsters)
        {
            dungeonTeam.allMonsters[i].monsterName = monster.monsterName;
            dungeonTeam.allMonsters[i].level = monster.level;
            dungeonTeam.allMonsters[i].isStarter = monster.isStarter;
            i++;
        }
        */
    }
}
