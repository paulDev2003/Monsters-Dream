using UnityEngine;
using UnityEngine.AI;

public class MonsterRunningAwayState : MonsterState
{
    protected GameObject gameObject;
    protected Transform transform;
    private float fearDistance = 5f;

    public MonsterRunningAwayState(Monster monster, MonsterStateMachine monsterStateMachine, float fearDistance) : base(monster, monsterStateMachine)
    {
        this.gameObject = monster.gameObject;
        this.transform = gameObject.transform;
        this.fearDistance = fearDistance;
    }

    public override void EnterState()
    {
        base.EnterState();
        RunAway();
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
        monster.RegenerateUpdate();
        monster.UpdateStatsEffects();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    private void RunAway()
    {
        // Dirección opuesta al miedo
        Vector3 fleeDirection = (transform.position - monster.transform.position).normalized;

        // Añadir algo de aleatoriedad al movimiento
        Vector3 random = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;
        Vector3 finalDirection = (fleeDirection + random * 0.3f).normalized;

        Vector3 targetPos = transform.position + finalDirection * fearDistance;

        // Verificar si el punto está en el NavMesh
        NavMeshHit hit;
        if (NavMesh.SamplePosition(targetPos, out hit, 5f, NavMesh.AllAreas))
        {
            monster.agent.SetDestination(hit.position);
        }
    }
}
