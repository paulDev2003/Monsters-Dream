using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "SKAbsorbTargets", menuName = "Scriptable Objects/Skills/AbsorbTargets")]
public class SKAbsorbTargets : SkillSO
{
    public override void ShootSkill(Monster owner)
    {
        AbsorbTargets(owner.oppositeList, owner);
        owner.monsterStateMachine.ChangeState(owner.monsterBasicAttackState);
    }

    private void AbsorbTargets(List<GameObject> oppositeList, Monster owner)
    {
        foreach (GameObject monster in oppositeList)
        {
            Monster monsterScript = monster.GetComponent<Monster>();
            monsterScript.target = owner;
        }
    }
}
