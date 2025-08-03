using UnityEngine;

[CreateAssetMenu(fileName = "Damage By Status", menuName = "Scriptable Objects/Skills/Damage By Status")]
public class SKDamageByStatus : SkillSO
{
    public StatusEffect statusEffect;
    public int baseDamage;
    public override void ShootSkill(Monster owner)
    {
        bool kill;
        int totalMultiplier = 1;
        int i = 0;
        foreach (var effect in owner.target.activeEffects)
        {
            if (effect == statusEffect)
            {
                totalMultiplier = owner.target.intStatesEffects[i];
            }
            i++;
        }
        int totalDamage = baseDamage * totalMultiplier;
        owner.target.TakeDamage(totalDamage);
        kill = CalculateDamage(totalDamage, owner.target);
        if (kill)
        {
            owner.circleAttacksToSkill.fillAmount = 1;
            owner.skillDrop.cooldownImage.fillAmount = 1;
            owner.skillDrop.cooldownImage.fillAmount = 1;
            owner.monsterBasicAttackInstance.attacksToSkill = 0;
        }
        owner.monsterStateMachine.ChangeState(owner.monsterBasicAttackState);
    }

    private bool CalculateDamage(int damage, Monster target)
    {
        float shield = target.shield;
        float healthFigth = target.healthFigth;
        bool dead = false;
        if (target.shieldActivated)
        {
            shield -= damage;
            if (damage > shield)
            {
                float restDamage = damage - shield;
                healthFigth -= restDamage;
            }
        }
        else
        {
            healthFigth -= damage;
        }

        if (healthFigth <= 0)
        {
            dead = true;
        }
        Debug.Log(healthFigth);
        return dead;
    }
}
