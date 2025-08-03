using UnityEngine;

public class MonsterDeadSO : ScriptableObject
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
    public virtual void DoEnterState() { }
    public virtual void DoExitState() { }
    public virtual void DoFrameUpdate() { }
    public virtual void DoPhysicsUpdate() { }
}
