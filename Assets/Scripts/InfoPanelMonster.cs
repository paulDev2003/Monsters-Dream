using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InfoPanelMonster : MonoBehaviour
{
    
     public TextMeshProUGUI txtDam;
     public TextMeshProUGUI txtDef;
     public TextMeshProUGUI txtMDef;
     public TextMeshProUGUI txtMDam;
     public TextMeshProUGUI txtEva;
     public TextMeshProUGUI txtSpA;
     public TextMeshProUGUI txtHea;
     public TextMeshProUGUI txtLvl;
     public TextMeshProUGUI txtName;

    public PanelChooseMonster panelChooseMonster;
    public Image targetImage;
    public int savedLevel;
    public int savedHealth;
    public string savedName;
   
    
    public void ShowInfoPanel(int level, MonsterClass monsterClass, string monsterName)
    {
        txtLvl.text = $"Lv. {level}";
        savedLevel = level;
        txtName.text = monsterName;
        savedName = monsterName;
        txtDam.text = monsterClass.PhysicalDamage.ToString();
        txtDef.text = monsterClass.Defense.ToString();
        txtMDef.text = monsterClass.MagicalDefense.ToString();
        txtMDam.text = monsterClass.MagicalDamage.ToString();
        txtEva.text = monsterClass.Evasion.ToString();
        txtSpA.text = monsterClass.SpeedAttack.ToString();
        txtHea.text = monsterClass.Health.ToString();
        savedHealth = monsterClass.Health;
    }

    public void ChooseThis()
    {
        if (panelChooseMonster.monsterSelected != null)
        {
            panelChooseMonster.monsterSelected.targetImage.color = panelChooseMonster.deselectedColor;
        }
        panelChooseMonster.monsterSelected = this;
        targetImage.color = panelChooseMonster.selectedColor;
    }
    
}
