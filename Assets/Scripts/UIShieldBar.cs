using UnityEngine;
using UnityEngine.UI;

public class UIShieldBar : MonoBehaviour
{
    public Image image;
    public StatusEffect statusEffect;
    public GameObject areaExplosion;
    public void UpdateShield(Monster monster)
    {
        image.fillAmount = monster.shield / monster.health;
        if (monster.shield <= 0)
        {
            monster.shieldActivated = false;
            monster.UpdateBar();
            if (statusEffect != null)
            {
                ShieldExplosion(monster);
            }
        }
    }

    private void ShieldExplosion(Monster owner)
    {
        if (!owner.enemie)
        {
            GameObject areaInstantiated = Instantiate(areaExplosion, owner.transform.position, Quaternion.identity);
            AreaShieldExplosion circleArea = areaInstantiated.GetComponent<AreaShieldExplosion>();
            circleArea.effect = statusEffect;
            circleArea.enemie = false;
            circleArea.ActivateArea();
        }
        else
        {
            GameObject areaInstantiated = Instantiate(areaExplosion, owner.transform.position, Quaternion.identity);
            AreaShieldExplosion circleArea = areaInstantiated.GetComponent<AreaShieldExplosion>();
            circleArea.effect = statusEffect;
            circleArea.enemie = true;
            circleArea.ActivateArea();
        }
    }
}
