using UnityEngine;
using System.Collections.Generic;

public class Rune : MonoBehaviour
{
    public List<Vector2Int> positionsSlots;
    private ManagerRunes managerRunes;
    public bool isUsed = false;
    public Vector2Int savePosition;
    public RuneSO runeSO;
    public int level = 1;
    void Start()
    {
        managerRunes = FindAnyObjectByType<ManagerRunes>();
    }

    public void SelectRune()
    {
        managerRunes.runeSelected = this;
    }

    public void DropRune()
    {
        managerRunes.runeSelected = null;
    }
}
