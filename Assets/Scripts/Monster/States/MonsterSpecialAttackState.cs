using UnityEngine;

public class MonsterSpecialAttackState : MonsterState
{
    public MonsterSpecialAttackState(Monster monster, MonsterStateMachine monsterStateMachine) : base(monster, monsterStateMachine)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        monster.monsterSpecialAttackInstance.DoEnterState();
    }

    public override void ExitState()
    {
        base.ExitState();
        monster.monsterSpecialAttackInstance.DoExitState();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
        monster.monsterSpecialAttackInstance.DoFrameUpdate();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        monster.monsterSpecialAttackInstance.DoPhysicsUpdate();
    }
}
