using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class ManagerRunes : MonoBehaviour
{
    public bool[,] slotsChecker = new bool[4, 8];
    public List<BoxRune> slotsRunes = new List<BoxRune>();
    public GameObject runePanel;
    public Rune runeSelected;
    public List<Rune> allRunes = new List<Rune>();
    public List<GameObject> prefabsRunes = new List<GameObject>();
    public List<GameObject> friendList = new List<GameObject>();
    public bool isFigth = false;
    public TextMeshProUGUI txtMoney;
    public List<GameObject> optionsRunes = new List<GameObject>();
    public List<RuneDataSO> runesToDrop = new List<RuneDataSO>();
    public Vector2Int levelRunes;
    public GameObject btnNextRoom;
    public RuneDataBase runeDataBase;
    public List<RuneClass> runesDungeon = new List<RuneClass>();


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
                rune.runeSO.UsePower(friendList, rune.level);
            }
            foreach (var monster in friendList)
            {
                Monster scriptMonster = monster.GetComponent<Monster>();
                scriptMonster.UpdateStats();
            }
        }
        
    }

    private void ChargeSavedRunes()
    {
        foreach (var rune in runesDungeon)
        {
            RuneBase runeBase = runeDataBase.GetRuneBaseByName(rune.runeName);
            BoxRune savedBox = slotsRunes[0];
            foreach (var slot in slotsRunes)
            {
                if (slot.horizontalPosition == rune.savePosition.x && slot.verticalPosition == rune.savePosition.y)
                {
                    savedBox = slot;
                    break;
                }
            }
            GameObject runeInstantiated = Instantiate(runeBase.runeDataSO.runePrefab, savedBox.gameObject.transform);

        }
    }

    public void ColocateRunes()
    {
        int money = PlayerPrefs.GetInt("Money", 0);
        txtMoney.text = money.ToString();
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
            rune.runeSO.UsePower(monster, rune.level);
        }
        monster.UpdateStats();
    }

    public void DesactiveteOptions()
    {
        foreach (var option in optionsRunes)
        {
            option.SetActive(false);
        }
    }

    public void DropRunes()
    {
        foreach (var option in optionsRunes)
        {
            RuneDataSO runeData = runesToDrop[Random.Range(0, runesToDrop.Count)];
            int level = Random.Range(levelRunes.x, levelRunes.y + 1);
            string info = runeData.LoadData(level);
            OptionRune scriptOption = option.GetComponent<OptionRune>();
            GameObject runeInstancied = Instantiate(runeData.runePrefab, scriptOption.spotPrefab.position, Quaternion.identity, option.transform);
            scriptOption.txtLevel.text = $"Lvl {level}";
            scriptOption.txtDescription.text = info;
            scriptOption.txtCost.text = runeData.finalCost.ToString();
            scriptOption.rune = runeInstancied.GetComponent<Rune>();
        }
    }
}
