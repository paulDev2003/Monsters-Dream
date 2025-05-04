using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor.EventSystems;

public class ShopItem : MonoBehaviour
{
    public Image spriteImage;
    public Image targetImage;
    public Color initialColor;
    public Color selectColor;
    public Color buyedColor;

    public int costItem;
    public TextMeshProUGUI txtCost;
    public Transform spotRune;
    public Rune rune;
    public Upgrade upgradeSO;
    public ManagerRunes runeManager;
    public DungeonTeam dungeonTeam;
    public ShopManager shopManager;
    public bool buyed = false;
    
    public enum TypeItem
    {
        rune,
        upgrade
    }
    public TypeItem myType;

    public void SelectItem()
    {
        if (buyed)
        {
            return;
        }
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
        else
        {
            shopManager.ActivateRuneSelection.Invoke(); 
        }
    }
}
