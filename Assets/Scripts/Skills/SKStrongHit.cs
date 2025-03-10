using UnityEngine;

[CreateAssetMenu(fileName = "StrongHit", menuName = "Scriptable Objects/Skills/StrongHit")]
public class SKStrongHit : SkillSO
{
    public float attackMultiplier = 2;
    public override void ShootSkill(Monster owner)
    {
        StrongHit(owner.target, owner);
    }

    public void StrongHit(Monster target, Monster monsterOwner)
    {
        float damage = monsterOwner.monsterSO.physicalDamage * attackMultiplier;
        target.healthFigth -= damage;
        monsterOwner.AttackScreenInfo(damage, target);
    }
}
