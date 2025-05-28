using UnityEngine;

[CreateAssetMenu(fileName = "Fear On Area", menuName = "Scriptable Objects/Skills/Fear On Area")]
public class SKFearOnArea : SkillSO
{
    public GameObject areaFear;
    public override void ShootSkill(Monster owner)
    {
        if (!owner.enemie)
        {
            if (!owner.skillDrop.showingArea)
            {
                GameObject areaInstantiated = Instantiate(areaFear, owner.transform.position, Quaternion.identity);
                owner.skillDrop.areaInstantiated = areaInstantiated;
                FearArea fearArea = areaInstantiated.GetComponent<FearArea>();
                fearArea.enemie = false;
                fearArea.owner = owner;
            }
            else if (owner.skillDrop.showingArea)
            {
                owner.skillDrop.areaInstantiated.GetComponent<FearArea>().ActivateArea();
            }
        }
        else
        {
            GameObject areaInstantiated = Instantiate(areaFear, owner.target.transform.position, Quaternion.identity);
            FearArea fearArea = areaInstantiated.GetComponent<FearArea>();
            fearArea.enemie = true;
            fearArea.owner = owner;
            fearArea.ActivateArea();
        }
    }
}
