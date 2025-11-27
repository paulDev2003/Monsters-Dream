using UnityEngine;
using UnityEngine.UI;

public class TeamSlot : MonoBehaviour
{
    public Image img;
    public string monsterName;
    public int level;
    public TeamSelector teamSelector;

    public void FillOutStats()
    {
        teamSelector.FillOutStats(monsterName, level);
    }
}
