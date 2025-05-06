using UnityEngine;
using TMPro;

public class BtnResetShopItems : MonoBehaviour
{
    public int cost = 5;
    public ShopManager shopManager;
    public TextMeshProUGUI txtMoney;
    public TextMeshProUGUI txtCost;

    private void Start()
    {
        txtCost.text = cost.ToString();
    }

    public void ResetItems()
    {
        int totalMoney = PlayerPrefs.GetInt("Money", 0);
        if (totalMoney >= cost)
        {
            shopManager.DestroyRunes();
            shopManager.DropItems();
            totalMoney -= cost;
            PlayerPrefs.SetInt("Money", totalMoney);
            txtMoney.text = totalMoney.ToString();
            cost *= 2;
            txtCost.text = cost.ToString();
        }
    }
}
