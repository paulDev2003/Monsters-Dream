using UnityEngine;

[CreateAssetMenu(fileName = "Status And Push", menuName = "Scriptable Objects/Skills/Status and Push")]
public class SKStatusAndPush : SkillSO
{
    public GameObject areaPush;
    public override void ShootSkill(Monster owner)
    {
        if (!owner.enemie)
        {
            if (!owner.skillDrop.showingArea)
            {
                GameObject areaInstantiated = Instantiate(areaPush, owner.transform.position, Quaternion.identity);
                owner.skillDrop.areaInstantiated = areaInstantiated;
                StatusAreaAndPush area = areaInstantiated.GetComponent<StatusAreaAndPush>();
                area.enemie = false;
                area.owner = owner;
            }
            else if (owner.skillDrop.showingArea)
            {
                owner.skillDrop.areaInstantiated.GetComponent<StatusAreaAndPush>().ActivateArea();
            }
        }
        else
        {
            GameObject areaInstantiated = Instantiate(areaPush, owner.target.transform.position, Quaternion.identity);
            StatusAreaAndPush area = areaInstantiated.GetComponent<StatusAreaAndPush>();
            area.enemie = true;
            area.ActivateArea();
            area.owner = owner;
        }

    }
}
