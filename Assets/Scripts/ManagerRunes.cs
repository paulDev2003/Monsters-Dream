using UnityEngine;
using System.Collections.Generic;

public class ManagerRunes : MonoBehaviour
{
    public bool[,] slotsChecker = new bool[4,8];
    public List<BoxRune> slotsRunes = new List<BoxRune>();
    public Rune runeSelected;
    public List<Rune> allRunes = new List<Rune>();
    public List<GameObject> prefabsRunes = new List<GameObject>();
    public List<GameObject> friendList = new List<GameObject>();
    public bool isFigth = false;

    private void Start()
    {
        
        for(int i = 0; i < 4; i++)
        {
            for (int e = 0; e < 8; e++)
            {
                slotsChecker[i, e] = true;
            }
        }
        if (isFigth)
        {
            friendList = FindAnyObjectByType<GameManager>().friendsList;
            foreach (var rune in allRunes)
            {
                rune.runeSO.UsePower(friendList);
            }
            foreach (var monster in friendList)
            {
                Monster scriptMonster = monster.GetComponent<Monster>();
                scriptMonster.UpdateStats();
            }
        }
        
    }

    public void ColocateRunes()
    {
        foreach (var rune in allRunes)
        {
            bool foundSlot = false; 
            foreach (var slot in slotsRunes)
            {
                if (rune.savePosition == new Vector2(slot.horizontalPosition, slot.verticalPosition))
                {
                    rune.transform.position = slot.transform.position;
                    foreach (var saveSlot in rune.positionsSlots)
                    {
                        slotsChecker[slot.horizontalPosition + saveSlot.x, slot.verticalPosition + saveSlot.y] = false;
                    }
                    foundSlot = true;
                    break;
                }
            }
            if (foundSlot)
                continue; // va al siguiente rune
        }
    }

    //Para cuando aparece un monstruo que o estaba en la escena
    public void AddBuffs(Monster monster)
    {
        foreach (var rune in allRunes)
        {
            rune.runeSO.UsePower(monster);
        }
        monster.UpdateStats();
    }
}
