using UnityEngine;
using System.Collections.Generic;

public class Rune : MonoBehaviour
{
    public List<Vector2Int> positionsSlots;
    private ManagerRunes managerRunes;
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
