using UnityEngine;

[CreateAssetMenu(fileName = "StrongHit", menuName = "Scriptable Objects/Skills/StrongHit")]
public class SKStrongHit : SkillSO
{
    [Tooltip("Se multiplica la variable por el physicalDamage, e ignora la defensa")]
    public float attackMultiplier = 2;
    public override void ShootSkill(Monster owner)
    {
        StrongHit(owner.target, owner);
    }

    public void StrongHit(Monster target, Monster monsterOwner)
    {
        float damage = monsterOwner.monsterSO.physicalDamage * attackMultiplier;
        target.healthFigth -= damage;
        monsterOwner.AttackScreenInfo(damage, target.gameObject);
    }
}
