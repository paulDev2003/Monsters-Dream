using UnityEngine;
using TMPro;

public class BtnTrashRunes : MonoBehaviour
{
    public ManagerRunes runesManager;
    public TextMeshProUGUI txtCost;
    public TextMeshProUGUI txtMoney;
    public GameObject window;
    public int currentCost;

    
    public void OpenWindow()
    {
        if (runesManager.runeSelected != null)
        {
            window.SetActive(true);
            currentCost = runesManager.runeSelected.cost;
            txtCost.text = $"{currentCost}?";
        }        
    }

    public void DestroyRune()
    {
        int totalMoney = PlayerPrefs.GetInt("Money", 0);
        totalMoney += currentCost;
        txtMoney.text = totalMoney.ToString();
        PlayerPrefs.SetInt("Money", totalMoney);
        runesManager.allRunes.Remove(runesManager.runeSelected);
        Destroy(runesManager.runeSelected.gameObject);
    }
}
