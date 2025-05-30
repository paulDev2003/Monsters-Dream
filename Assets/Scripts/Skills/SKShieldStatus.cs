using UnityEngine;

[CreateAssetMenu(fileName = "Shield Status", menuName = "Scriptable Objects/Skills/Shield Status")]
public class SKShieldStatus : SkillSO
{
    public GameObject shieldArea;
    public StatusEffect effect;
    public override void ShootSkill(Monster owner)
    {
        if (!owner.enemie)
        {
            GameObject areaInstantiated = Instantiate(shieldArea, owner.transform.position, Quaternion.identity);
            GiveShieldArea circleArea = areaInstantiated.GetComponent<GiveShieldArea>();
            circleArea.effect = effect;
            circleArea.enemie = false;
            circleArea.ActivateArea();
        }
        else
        {
            GameObject areaInstantiated = Instantiate(shieldArea, owner.transform.position, Quaternion.identity);
            GiveShieldArea circleArea = areaInstantiated.GetComponent<GiveShieldArea>();
            circleArea.effect = effect;
            circleArea.enemie = true;
            circleArea.ActivateArea();
        }
    }
}
