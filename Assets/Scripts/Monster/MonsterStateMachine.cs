using UnityEngine;

public class MonsterStateMachine
{
    public MonsterState currentState;

    public void Initialize(MonsterState initialState)
    {
        currentState = initialState;
        currentState.EnterState();
    }

    public void ChangeState(MonsterState newState)
    {
        currentState.ExitState();
        currentState = newState;
        currentState.EnterState();
    }
}
