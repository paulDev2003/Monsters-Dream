using UnityEngine;

[CreateAssetMenu(fileName = "MonsterSpecialAttack-Default", menuName = "Monster Behaviour/Special Attack/Default")]
public class MonsterSpecialAttackDefault : MonsterSpecialAttackSO
{
    public override void DoEnterState()
    {
        base.DoEnterState();
        monster.ShootSpecialAttack();
        monster.currentState = "special Attack";
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

    
}
