using UnityEngine;

public class MonsterChaseState : MonsterState
{
    public MonsterChaseState(Monster monster, MonsterStateMachine monsterStateMachine) : base(monster, monsterStateMachine)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        monster.monsterChaseInstance.DoEnterState();
    }

    public override void ExitState()
    {
        base.ExitState();
        monster.monsterChaseInstance.DoExitState();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
        monster.monsterChaseInstance.DoFrameUpdate();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        monster.monsterChaseInstance.DoPhysicsUpdate();
    }
}
