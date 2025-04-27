using UnityEngine;

public class BeginDungeon : MonoBehaviour
{

    public void InitializeDungeon()
    {
        PlayerPrefs.SetInt("BattleNumber", 0);
    }
}
