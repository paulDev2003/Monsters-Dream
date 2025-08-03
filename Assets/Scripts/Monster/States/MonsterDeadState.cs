using UnityEngine;

public class MonsterDeadState : MonsterState
{
    public MonsterDeadState(Monster monster, MonsterStateMachine monsterStateMachine) : base(monster, monsterStateMachine)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        monster.monsterDeadInstance.DoEnterState();
    }

    public override void ExitState()
    {
        base.ExitState();
        monster.monsterDeadInstance.DoExitState();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
        monster.monsterDeadInstance.DoFrameUpdate();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        monster.monsterDeadInstance.DoPhysicsUpdate();
    }
}
