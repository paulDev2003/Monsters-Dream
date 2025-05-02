using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopItem : MonoBehaviour
{
    public Image spriteImage;
    public Image targetImage;
    public Color initialColor;
    public Color selectColor;
    public string costItem;
    public TextMeshProUGUI txtCost;
    public Transform spotRune;
    public RuneDataSO runeSO;
    public Upgrade upgradeSO;
    public ManagerRunes runeManager;
    public DungeonTeam dungeonTeam;
    public ShopManager shopManager;
    public enum TypeItem
    {
        rune,
        upgrade
    }
    public TypeItem myType;

    public void SelectItem()
    {
        targetImage.color = selectColor;
        shopManager.SelectItem(this);
    }
    public void BuyItem()
    {
        if (myType == ShopItem.TypeItem.upgrade)
        {
            runeManager.allUpgrades.Add(upgradeSO);
            if (upgradeSO.instantEffectMonsters)
            {
                upgradeSO.UseInstantEffectMonsters(dungeonTeam.allMonsters);
            }
            if (upgradeSO.instantEffectRunes)
            {
                upgradeSO.UseInstantEffectRunes(runeManager.allRunes);
            }
        }
    }
}
