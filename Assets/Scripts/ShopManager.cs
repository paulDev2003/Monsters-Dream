using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

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
    public TextMeshProUGUI txtMoney;

    public GameObject lineToConnectRunes;
    public GameObject prefabInfo;
    public TextMeshProUGUI txtInfoPrefab;
    public TextMeshProUGUI txtNamePrefab;

    private void Start()
    {
        DropItems();
        int totalMoney = PlayerPrefs.GetInt("Money", 0);
        txtMoney.text = totalMoney.ToString();
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
                item.spriteImage.enabled = true;
                item.spriteImage.sprite = upgrade.sprite;
                item.txtCost.text = item.costItem.ToString();
            }
            if (item.myType == ShopItem.TypeItem.rune)
            {
                RuneDataSO runeData = runesToDrop[Random.Range(0, runesToDrop.Count)];
                int level = Random.Range(runeManager.levelRunes.x, runeManager.levelRunes.y + 1);
                string info = runeData.LoadData(level);
                GameObject runeInstancied = Instantiate(runeData.runePrefab, item.spotRune.position, Quaternion.identity, item.targetImage.transform);
                runeInstancied.transform.localScale = Vector3.one;
                Rune scriptRune = runeInstancied.GetComponent<Rune>();
                scriptRune.onSell = true;
                scriptRune.isUsed = false;
                scriptRune.cost = runeData.finalCost;
                scriptRune.level = level;
                item.rune = scriptRune;
                runeInstancied.transform.position = item.spotRune.position;
                item.txtCost.text = scriptRune.cost.ToString();
            }
        }

    }

    public void SelectItem(ShopItem itemToSelect)
    {
        if (selectedItem != null)
        {
            selectedItem.targetImage.color = selectedItem.initialColor;
            
        }
        if (itemToSelect == selectedItem)
        {
            selectedItem = null;
        }
        else
        {
            selectedItem = itemToSelect;
        }
        

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
            totalMoney -= selectedItem.costItem;
            PlayerPrefs.SetInt("Money", totalMoney);
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
        selectedItem.rune.gameObject.transform.parent = spotRuneSelection.transform;
        selectedItem.rune.gameObject.transform.localScale = Vector3.one;
        runeManager.lineToConect = lineToConnectRunes;
        runeManager.prefabInfo = prefabInfo;
        runeManager.txtInfoPrefab = txtInfoPrefab;
        runeManager.txtNamePrefab = txtNamePrefab;
        selectedItem.rune.onSell = false;
    }
}

