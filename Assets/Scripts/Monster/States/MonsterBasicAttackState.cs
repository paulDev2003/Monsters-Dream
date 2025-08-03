using UnityEngine;

public class MonsterBasicAttackState : MonsterState
{
    public MonsterBasicAttackState(Monster monster, MonsterStateMachine monsterStateMachine) : base(monster, monsterStateMachine)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        monster.monsterBasicAttackInstance.DoEnterState();
    }

    public override void ExitState()
    {
        base.ExitState();
        monster.monsterBasicAttackInstance.DoExitState();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
        monster.monsterBasicAttackInstance.DoFrameUpdate();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        monster.monsterBasicAttackInstance.DoPhysicsUpdate();
    }
}
