using UnityEngine;
using System.Collections.Generic;

public class ShopManager : MonoBehaviour
{
    public List<RuneDataSO> runesToDrop = new List<RuneDataSO>();
    public List<Upgrade> upgradesToDrop = new List<Upgrade>();

    public List<ShopItem> shopItems = new List<ShopItem>();

    public ShopItem selectedItem;

    public ManagerRunes runeManager;
    void Start()
    {
        
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
}
