using UnityEngine;
using UnityEngine.UI;

public class UIShieldBar : MonoBehaviour
{
    public Image image;
    public void UpdateShield(Monster monster)
    {
        image.fillAmount = monster.shield / monster.health;
        if (monster.shield <= 0)
        {
            monster.shieldActivated = false;
            monster.UpdateBar();
        }
    }
}
