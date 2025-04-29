using UnityEngine;
using System.Collections.Generic;

public class Rune : MonoBehaviour
{
    public string runeName;
    public List<Vector2Int> positionsSlots;
    private ManagerRunes managerRunes;
    public bool isUsed = false;
    public Vector2Int savePosition;
    public RuneSO runeSO;
    public RuneDataSO runeData;
    public int level = 1;
    public int cost = 10;
    void Start()
    {
        managerRunes = FindAnyObjectByType<ManagerRunes>();
    }

    public void SelectRune()
    {
        managerRunes.runeSelected = this;
        gameObject.transform.parent = managerRunes.runePanel.transform;
        if (isUsed)
        {
            foreach (var saveSlot in positionsSlots)
            {
                managerRunes.slotsChecker[savePosition.x + saveSlot.x, savePosition.y + saveSlot.y] = true;
            }
        }
    }

    public void DropRune()
    {
        managerRunes.runeSelected = null;
    }

    public void ShowInfo()
    {
        if (isUsed)
        {
            managerRunes.ShowPrefabInfo(this);
        }       
    }

    public void DisableInfo()
    {
        managerRunes.DisableAllPrefabInfo();
    }
}
