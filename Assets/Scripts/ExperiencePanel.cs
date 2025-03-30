using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ExperiencePanel : MonoBehaviour
{
    public Image sprite;
    public TextMeshProUGUI txtLevel;
    public TextMeshProUGUI txtName;
    public Image expBar;
    public Image backBar;
    public void ShowPanel(Monster monster)
    {
        sprite.enabled = true;
        sprite.sprite = monster.monsterSO.sprite;
        txtLevel.enabled = true;
        txtLevel.text = $"Lv.{monster.level}";
        txtName.enabled = true;
        txtName.text = monster.name;
        expBar.enabled = true;
        expBar.fillAmount = (float)monster.exp / (float)monster.maxExp;
        backBar.enabled = true;
    }
}
