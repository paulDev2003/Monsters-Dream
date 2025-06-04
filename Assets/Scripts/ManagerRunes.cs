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
    public List<Upgrade> allUpgrades = new List<Upgrade>();
    public List<GameObject> prefabsRunes = new List<GameObject>();
    public List<GameObject> friendList = new List<GameObject>();
    public bool isFigth = false;
    public bool lobby = false;
    public TextMeshProUGUI txtMoney;
    public List<GameObject> optionsRunes = new List<GameObject>();
    public GameObject btnResetRunes;
    public GameObject btnSkipRunes;
    public List<RuneDataSO> runesToDrop = new List<RuneDataSO>();
    public Vector2Int levelRunes;
    public GameObject btnNextRoom;
    public RuneDataBase runeDataBase;
    public UpgradeDataBase upgradeDataBase;
    public List<RuneClass> runesDungeon = new List<RuneClass>();
    public List<UpgradeData> upgradesDungeon = new List<UpgradeData>();
    public GameObject prefabInfo;
    public TextMeshProUGUI txtNamePrefab;
    public TextMeshProUGUI txtInfoPrefab;
    public GameObject lineToConect;

    private void Start()
    {

        for (int i = 0; i < 4; i++)
        {
            for (int e = 0; e < 8; e++)
            {
                slotsChecker[i, e] = true;
            }
        }
        if (!lobby)
        {
            ChargeSavedRunes();
            ChargeSavedUpgrades();
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
                foreach (var upgrade in allUpgrades)
                {
                    if (upgrade.permanentEffect)
                    {
                        upgrade.UseUpgrade(scriptMonster);
                    }
                }
            }
            
        }

    }

    private void ChargeSavedRunes()
    {
        foreach (var rune in runesDungeon)
        {
            if (runePanel != null)
            {
                RuneBase runeBase = runeDataBase.GetRuneBaseByName(rune.runeName);
                GameObject runeInstantiated = Instantiate(runeBase.runeDataSO.runePrefab, runePanel.transform);
                prefabsRunes.Add(runeInstantiated);
                Rune scriptRune = runeInstantiated.GetComponent<Rune>();
                scriptRune.cost = rune.cost;
                scriptRune.level = rune.level;
                scriptRune.savePosition = rune.savePosition;
                scriptRune.isUsed = true;
                allRunes.Add(scriptRune);
            }          
        }
    }

    private void ChargeSavedUpgrades()
    {
        foreach (var upgrade in upgradesDungeon)
        {
            UpgradeBase upgradeBase = upgradeDataBase.GetUpgradeBaseByName(upgrade.upgradeName);
            allUpgrades.Add(upgradeBase.upgradeSO);
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
        foreach (var upgrade in allUpgrades)
        {
            if (upgrade.permanentEffect)
            {
                upgrade.UseUpgrade(monster);
            }           
        }
    }

    public void DesactiveteOptions()
    {
        foreach (var option in optionsRunes)
        {
            option.SetActive(false);
        }
        if (btnResetRunes != null)
        {
            btnResetRunes.SetActive(false);
        }
        if (btnSkipRunes != null)
        {
            btnSkipRunes.SetActive(false);
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
            scriptOption.txtDescription.fontSize = runeData.fontSize;
            scriptOption.txtCost.text = runeData.finalCost.ToString();
            Rune scriptRune = runeInstancied.GetComponent<Rune>();
            scriptOption.rune = scriptRune;
            scriptRune.isUsed = false;
            scriptRune.cost = runeData.finalCost;
            scriptRune.level = level;
        }
    }

    public void ShowPrefabInfo(Rune rune)
    {
        prefabInfo.SetActive(true);
        lineToConect.SetActive(true);
        RectTransform rectTransformRune = rune.GetComponent<RectTransform>();
        if (rune.onSell)
        {
            rectTransformRune = rune.transform.parent.parent.GetComponent<RectTransform>();
        }
        lineToConect.GetComponent<SimpleUILine>().ConectLine(rectTransformRune);
        string info = rune.runeData.LoadData(rune.level);
        txtNamePrefab.text = rune.runeName;
        txtInfoPrefab.text = info;
    }

    public void DisableAllPrefabInfo()
    {
        prefabInfo.SetActive(false);
        lineToConect.GetComponent<SimpleUILine>().destinyPoint.gameObject.SetActive(false);
        lineToConect.SetActive(false);
    }
}
