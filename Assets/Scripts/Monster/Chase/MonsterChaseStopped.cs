using UnityEngine;

[CreateAssetMenu(fileName = "MonsterChase-Stopped", menuName = "Monster Behaviour/Chase/Stopped")]
public class MonsterChaseStopped : MonsterChaseSO
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
        transform.position += Vector3.zero;
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
