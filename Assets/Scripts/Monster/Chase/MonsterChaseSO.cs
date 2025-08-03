using UnityEngine;

public class MonsterChaseSO : ScriptableObject
{
    protected GameObject gameObject;
    protected Transform transform;
    protected Monster monster;
    public virtual void Initialize(GameObject gameObject, Monster monster)
    {
        this.gameObject = gameObject;
        this.transform = gameObject.transform;
        this.monster = monster;
    }
    public virtual void DoEnterState() 
    {
        if (monster.animator != null)
        {
            monster.animator.SetBool("Attacking", false);
        }
    }
    public virtual void DoExitState() { }
    public virtual void DoFrameUpdate() 
    {
        monster.RegenerateUpdate();
        monster.UpdateStatsEffects();
        if (monster.target != null)
        {
            if (monster.targetCollider != null)
            {
                Vector3 closestPoint = monster.targetCollider.ClosestPoint(transform.position);

                if ((transform.position - closestPoint).sqrMagnitude < monster.distanceAttack)
                {
                    monster.monsterStateMachine.ChangeState(monster.monsterBasicAttackState);
                }

            }
            else if((transform.position - monster.target.transform.position).sqrMagnitude > monster.distanceAttack)
            {
                monster.monsterStateMachine.ChangeState(monster.monsterBasicAttackState);
            }
        }
    }
    public virtual void DoPhysicsUpdate() { }
    
    
}
