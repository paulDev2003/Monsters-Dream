using UnityEngine;

public abstract class StatusEffect: ScriptableObject
{
    public float duration;
    protected float timeRemaining;

    public StatusEffect(float duration)
    {
        this.duration = duration;
        this.timeRemaining = duration;
    }

    public virtual void ApplyEffect(Monster target) { }
    public virtual void RemoveEffect(Monster target) { }
    public virtual void Tick(Monster target, float deltaTime, int acumulation)
    {
        timeRemaining -= deltaTime;
        if (timeRemaining <= 0)
        {
            RemoveEffect(target);
        }
    }

   // public bool IsFinished => timeRemaining <= 0;
}
