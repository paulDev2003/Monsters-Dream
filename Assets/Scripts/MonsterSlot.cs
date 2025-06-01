using UnityEngine;

public class MonsterSlot : MonoBehaviour
{
    public MonsterData monsterData;
    public LobbyMonsterLevel lobbyMonsters;

    public void ChangeMonsterPrefab()
    {
        lobbyMonsters.ChangeMonsterPrefab(monsterData);
    }
}
