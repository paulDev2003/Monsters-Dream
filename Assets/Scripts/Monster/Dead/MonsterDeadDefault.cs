using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "MonsterDead-Default", menuName = "Monster Behaviour/Dead/Default")]
public class MonsterDeadDefault : MonsterDeadSO
{
    public float secondsToDissapear = 2.5f;
    public override void DoEnterState()
    {
        base.DoEnterState();
        if (monster.agent != null && monster.agent.enabled)
        {
            monster.agent.isStopped = true;
            monster.agent.enabled = false;
        }
        monster.gameManager.RemoveFromList(monster.ownList, monster);
        if (!monster.enemie)
        {
            monster.gameManager.attacksPanel[monster.valueI].targetImage.enabled = false;
            monster.gameManager.attacksPanel[monster.valueI].cooldownImage.fillAmount = 0;
            monster.gameManager.countMonsters--;
        }       
        if (!monster.notShowInterface)
        {
            monster.lifeBar.UpdateFill(monster);
        }
        if (!monster.boss && monster.enemie)
        {
            monster.RunSkillCoroutine(DesactivateMonster());
        }
    }

    public override void DoExitState()
    {
        base.DoExitState();
    }

    public override void DoFrameUpdate()
    {
        base.DoFrameUpdate();
    }

    public override void DoPhysicsUpdate()
    {
        base.DoPhysicsUpdate();
    }

    public override void Initialize(GameObject gameObject, Monster monster)
    {
        base.Initialize(gameObject, monster);
    }

    private IEnumerator DesactivateMonster()
    {
        yield return new WaitForSeconds(secondsToDissapear);
        gameObject.SetActive(false);
    }
}
