using UnityEngine;
using TMPro;

public class BtnSkipRunes : MonoBehaviour
{
    public TextMeshProUGUI txtMoney;
    public int reward;

    public void SkipRunes()
    {
        int totalMoney = PlayerPrefs.GetInt("Money", 0);
        totalMoney += reward;
        txtMoney.text = totalMoney.ToString();
        PlayerPrefs.SetInt("Money", totalMoney);
    }
}
