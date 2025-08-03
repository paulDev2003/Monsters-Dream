using UnityEngine;

[CreateAssetMenu(fileName = "Toxic Slap", menuName = "Scriptable Objects/Skills/Toxic Slap")]
public class SKToxicSlap : SkillSO
{
    public GameObject areaSlap;
    public override void ShootSkill(Monster owner)
    {
        if (!owner.enemie)
        {
            if (!owner.skillDrop.showingArea)
            {
                GameObject areaInstantiated = Instantiate(areaSlap, owner.transform.position, Quaternion.identity);
                owner.skillDrop.areaInstantiated = areaInstantiated;
                PoisonArea poisonArea = areaInstantiated.GetComponent<PoisonArea>();
                poisonArea.enemie = false;
                poisonArea.owner = owner;

            }
            else if (owner.skillDrop.showingArea)
            {
                owner.skillDrop.areaInstantiated.GetComponent<PoisonArea>().ActivateArea();
            }
        }
        else
        {
            GameObject areaInstantiated = Instantiate(areaSlap, owner.target.transform.position, Quaternion.identity);
            PoisonArea poisonArea = areaInstantiated.GetComponent<PoisonArea>();
            poisonArea.enemie = true;
            poisonArea.owner = owner;
            poisonArea.ActivateArea();
        }
    }
}
