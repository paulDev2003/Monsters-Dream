using UnityEngine;

public class MonsterSlot : MonoBehaviour
{
    public string monsterName;
    public LobbyMonsterLevel lobbyMonsters;

    public void ChangeMonsterPrefab()
    {
        lobbyMonsters.ChangeMonsterPrefab(monsterName);
    }
}
