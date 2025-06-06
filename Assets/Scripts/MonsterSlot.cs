using UnityEngine;
using UnityEngine.UI;

public class MonsterSlot : MonoBehaviour
{
    public MonsterData monsterData;
    public LobbyMonsterLevel lobbyMonsters;
    public PanelChooseMonster panelChooseMonster;
    public Bestiary bestiary;
    public Image img;
    public bool available = false;
    public int savedI;

    public void ChangeMonsterPrefab()
    {
        lobbyMonsters.ChangeMonsterPrefab(monsterData);
    }

    public void ChangeEgg()
    {
        if (available)
        {
            bestiary.ChangeEgg(monsterData);
        }        
    }

    public void ChangeMember()
    {

    }
}
