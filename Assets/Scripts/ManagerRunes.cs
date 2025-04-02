using UnityEngine;

public class ManagerRunes : MonoBehaviour
{
    public bool[,] slotsChecker = new bool[4,8];
    public Rune runeSelected;

    private void Start()
    {
        for(int i = 0; i < 4; i++)
        {
            for (int e = 0; e < 8; e++)
            {
                slotsChecker[i, e] = true;
            }
        }
    }
}
