using UnityEngine;

[CreateAssetMenu(fileName = "MonsterChase-Follow Enemy", menuName = "Monster Behaviour/Chase/Follow Enemy")]
public class MonsterChaseFollowEnemy : MonsterChaseSO
{
    public override void DoEnterState()
    {
        base.DoEnterState();
    }

    public override void DoExitState()
    {
        base.DoExitState();
    }

    public override void DoFrameUpdate()
    {
        base.DoFrameUpdate();
        if (monster.agent != null && monster.agent.enabled)
        {
            monster.agent.SetDestination(monster.target.transform.position);
        }
    }

    public override void DoPhysicsUpdate()
    {
        base.DoPhysicsUpdate();
    }

    public override void Initialize(GameObject gameObject, Monster monster)
    {
        base.Initialize(gameObject, monster);
    }
}
