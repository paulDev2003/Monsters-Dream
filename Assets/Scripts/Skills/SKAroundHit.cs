using UnityEngine;

[CreateAssetMenu(fileName = "Around Hit", menuName = "Scriptable Objects/Skills/Around Hit")]
public class SKAroundHit : SkillSO
{
    public GameObject areaHit;
    public override void ShootSkill(Monster owner)
    {
        if (!owner.enemie)
        {
            GameObject areaInstantiated = Instantiate(areaHit, owner.transform.position, Quaternion.identity);
            AroundHit circleArea = areaInstantiated.GetComponent<AroundHit>();
            circleArea.enemie = false;
            circleArea.owner = owner;
            circleArea.ActivateArea();
        }
        else
        {
            GameObject areaInstantiated = Instantiate(areaHit, owner.transform.position, Quaternion.identity);
            AroundHit circleArea = areaInstantiated.GetComponent<AroundHit>();
            circleArea.enemie = true;
            circleArea.owner = owner;
            circleArea.ActivateArea();
        }
    }
}
