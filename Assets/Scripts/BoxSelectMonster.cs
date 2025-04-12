using UnityEngine;

public class BoxSelectMonster : MonoBehaviour
{
    public MonsterData monsterData;
    private CurrentTeam current;
    public bool isUsed = false;
    public int i;
    public BoxSelectMonster savedBox;
    public void SelectMonster()
    {
        current = FindAnyObjectByType<CurrentTeam>();
        current.monsterSelected = this;      
    }

    public void SelectTeamMonster()
    {
        current = FindAnyObjectByType<CurrentTeam>();
        current.teamMonsterSelected = this;
    }
}
