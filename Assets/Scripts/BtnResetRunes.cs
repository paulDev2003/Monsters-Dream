using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class BtnResetRunes : MonoBehaviour
{
    public int cost;
    public TextMeshProUGUI txtCost;
    public TextMeshProUGUI txtMoney;
    public List<OptionRune> optionList = new List<OptionRune>();
    public void RestMoney()
    {
        int totalMoney = PlayerPrefs.GetInt("Money", 0);
        if (cost <= totalMoney)
        {
            totalMoney -= cost;
            cost *= 2;
            txtCost.text = cost.ToString();
            PlayerPrefs.SetInt("Money", totalMoney);
            txtMoney.text = totalMoney.ToString();
            foreach (var option in optionList)
            {
                Destroy(option.rune.gameObject);
            }
        }
        
    }
}
