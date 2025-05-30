using UnityEngine;

[CreateAssetMenu(fileName = "Health Area", menuName = "Scriptable Objects/Skills/Health Area")]
public class SKHealthArea : SkillSO
{
    public GameObject areaHealth;
    public override void ShootSkill(Monster owner)
    {
        if (!owner.enemie)
        {
            GameObject areaInstantiated = Instantiate(areaHealth, owner.transform.position, Quaternion.identity);
            HealthArea circleArea = areaInstantiated.GetComponent<HealthArea>();
            circleArea.enemie = false;
            circleArea.ActivateArea();
        }
        else
        {
            GameObject areaInstantiated = Instantiate(areaHealth, owner.transform.position, Quaternion.identity);
            HealthArea circleArea = areaInstantiated.GetComponent<HealthArea>();
            circleArea.enemie = true;
            circleArea.ActivateArea();
        }
    }
}
