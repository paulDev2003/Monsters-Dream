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
                areaInstantiated.GetComponent<PoisonArea>().enemie = false;
            }
            else if (owner.skillDrop.showingArea)
            {
                owner.skillDrop.areaInstantiated.GetComponent<PoisonArea>().ActivateArea();
            }
        }
        else
        {
            GameObject areaInstantiated = Instantiate(areaSlap, owner.target.transform.position, Quaternion.identity);
            PoisonArea posionArea = areaInstantiated.GetComponent<PoisonArea>();
            posionArea.enemie = true;
            posionArea.ActivateArea();
        }
        
    }
}
