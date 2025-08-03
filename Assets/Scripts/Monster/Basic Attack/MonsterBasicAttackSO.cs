using UnityEngine;

public class MonsterBasicAttackSO : ScriptableObject
{
    protected GameObject gameObject;
    protected Transform transform;
    protected Monster monster;
    public int attacksToSkill;

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
            monster.animator.SetBool("Attacking", true);
        }
    }
    public virtual void DoExitState() { }
    public virtual void DoFrameUpdate() 
    {
        monster.RegenerateUpdate();
        monster.UpdateStatsEffects();
    }
    public virtual void DoPhysicsUpdate() { }
}
