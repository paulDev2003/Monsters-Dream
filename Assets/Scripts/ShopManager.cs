using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Events;

public class ShopManager : MonoBehaviour
{
    public List<RuneDataSO> runesToDrop = new List<RuneDataSO>();
    public List<Upgrade> upgradesToDrop = new List<Upgrade>();

    public List<ShopItem> shopItems = new List<ShopItem>();

    public ShopItem selectedItem;

    public ManagerRunes runeManager;
    public DungeonTeam dungeonTeam;
    public Image targetImageBtn;
    public Color initialColorTargetBtn;
    public Color finalColorTargetBtn;

    public UnityEvent ActivateRuneSelection;
    public Transform spotRuneSelection;

    private void Start()
    {
        DropItems();
    }
    private void Update()
    {
        if (selectedItem == null)
        {
            targetImageBtn.color = initialColorTargetBtn;
        }
        else
        {
            targetImageBtn.color = finalColorTargetBtn;
        }
    }

    public void DropItems()
    {
        foreach (var item in shopItems)
        {
            int randomChoice = Random.Range(0, 2);
            switch (randomChoice)
            {
                case 0:
                    item.myType = ShopItem.TypeItem.upgrade;
                    break;
                case 1:
                    item.myType = ShopItem.TypeItem.rune;
                    break;
                default:
                    item.myType = ShopItem.TypeItem.upgrade;
                    break;
            }
            if (item.myType == ShopItem.TypeItem.upgrade)
            {
                Upgrade upgrade = upgradesToDrop[Random.Range(0, upgradesToDrop.Count)];
                item.upgradeSO = upgrade;
                item.costItem = upgrade.cost;
                item.spriteImage.sprite = upgrade.sprite;
            }
            if (item.myType == ShopItem.TypeItem.rune)
            {
                RuneDataSO runeData = runesToDrop[Random.Range(0, runesToDrop.Count)];
                int level = Random.Range(runeManager.levelRunes.x, runeManager.levelRunes.y + 1);
                string info = runeData.LoadData(level);
                GameObject runeInstancied = Instantiate(runeData.runePrefab, item.spotRune.position, Quaternion.identity, item.transform);
                // scriptOption.txtLevel.text = $"Lvl {level}";
                // scriptOption.txtDescription.text = info;
                // scriptOption.txtDescription.fontSize = runeData.fontSize;
                // scriptOption.txtCost.text = runeData.finalCost.ToString();
                Rune scriptRune = runeInstancied.GetComponent<Rune>();
                // scriptOption.rune = scriptRune;
                scriptRune.isUsed = false;
                scriptRune.cost = runeData.finalCost;
                scriptRune.level = level;
                item.rune = scriptRune;
                runeInstancied.transform.position = item.spotRune.position;
            }
        }

    }

    public void SelectItem(ShopItem itemToSelect)
    {
        if (selectedItem != null)
        {
            selectedItem.targetImage.color = selectedItem.initialColor;
        }
        selectedItem = itemToSelect;

    }

    public void Buy()
    {
        int totalMoney = PlayerPrefs.GetInt("Money", 0);
        if (selectedItem == null || totalMoney < selectedItem.costItem)
        {
            return;
        }
        else
        {
            selectedItem.buyed = true;
            selectedItem.targetImage.color = selectedItem.buyedColor;
            if (selectedItem.myType == ShopItem.TypeItem.upgrade)
            {
                runeManager.allUpgrades.Add(selectedItem.upgradeSO);
                if (selectedItem.upgradeSO.instantEffectMonsters)
                {
                    selectedItem.upgradeSO.UseInstantEffectMonsters(dungeonTeam.allMonsters);
                }
                if (selectedItem.upgradeSO.instantEffectRunes)
                {
                    selectedItem.upgradeSO.UseInstantEffectRunes(runeManager.allRunes);
                }
            }
            else
            {
                ActivateRuneSelection.Invoke();
            }
            selectedItem = null;
        }
        
    }

    public void ChangeRunePosition()
    {
        selectedItem.rune.gameObject.transform.position = spotRuneSelection.position;
        selectedItem.rune.gameObject.transform.parent = runeManager.runePanel.transform;
    }
}

