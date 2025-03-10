using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "SlowAll", menuName = "Scriptable Objects/Skills/SlowAll")]
public class SKSlowAll : SkillSO
{
    public float speedReducedBy = 0.75f;
    public override void ShootSkill(Monster owner)
    {
        SlowAll(owner.oppositeList);
    }

    public void SlowAll(List<GameObject> rivalsList)
    {
        foreach (var monster in rivalsList)
        {
            Monster monsterComp = monster.GetComponent<Monster>();
            monsterComp.speedAttack = monsterComp.speedAttack * speedReducedBy;
        }
    }
}
