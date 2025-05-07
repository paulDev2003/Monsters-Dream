using UnityEngine;

[CreateAssetMenu(fileName = "BleedEffect", menuName = "StatusEffect/BleedEffect")]
public class BleedEffect : StatusEffect
{
    public float tickInterval = 2f;
    private float tickTimer = 0f;
    public int damagePerTick;
    public Sprite sprite;

    public BleedEffect(float duration, int damage) : base(duration)
    {
        damagePerTick = damage;
    }

    public override void Tick(Monster target, float deltaTime, int acumulation)
    {
        base.Tick(target, deltaTime, acumulation);

        tickTimer -= deltaTime;
        if (tickTimer <= 0f)
        {
            tickTimer = tickInterval;
            int totalDamage = damagePerTick * acumulation;
            Debug.Log($"totalDamage = {totalDamage} ");
            target.TakeDamageHealth(totalDamage);
        }
    }

    public override void ApplyEffect(Monster target)
    {
        target.ApplyStatus(sprite);
    }

    public override void RemoveEffect(Monster target)
    {
        // Elimina efectos visuales si hay
    }
}
