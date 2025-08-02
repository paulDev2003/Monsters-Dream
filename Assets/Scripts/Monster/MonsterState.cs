using UnityEngine;

public class MonsterState
{
    protected Monster monster;
    protected MonsterStateMachine monsterStateMachine;
    public MonsterState(Monster monster, MonsterStateMachine monsterStateMachine)
    {
        this.monster = monster;
        this.monsterStateMachine = monsterStateMachine;
    }
    public virtual void EnterState() { }
    public virtual void ExitState() { }
    public virtual void FrameUpdate() { }
    public virtual void PhysicsUpdate() { }

}
