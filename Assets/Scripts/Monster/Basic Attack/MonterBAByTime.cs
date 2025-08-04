using UnityEngine;

[CreateAssetMenu(fileName = "MonsterAttack-By Time", menuName =("Monster Behaviour/Attack/By Time"))]
public class MonterBAByTime : MonsterBasicAttackSO
{
    private float attackTime = 0;
    private float timeTillAttack = 1.5f;
    private int enemieRandomSpecialAttack = 0;
    private bool awaitToSpecialAttack = false;
    public override void DoEnterState()
    {
        base.DoEnterState();
        if (monster.agent != null && monster.agent.enabled)
        {
            monster.agent.ResetPath(); // Detener el movimiento al atacar
            //monster.agent.isStopped = true;
        }
        attackTime = 1 * monster.speedAttack;
        monster.currentState = "Basic Attack";
        if (attacksToSkill <= 0)
        {
            attacksToSkill = Random.Range(monster.rangeAttacksToSkill.x, monster.rangeAttacksToSkill.y);
        }
    }

    public override void DoExitState()
    {
        base.DoExitState();
    }

    public override void DoFrameUpdate()
    {
        base.DoFrameUpdate();
        if (attackTime > 0)
        {
            attackTime -= Time.deltaTime;
        }
        else
        {
            Attack();
        }
        monster.RotateTowardsTarget();
        CheckDistance();
    }

    public override void DoPhysicsUpdate()
    {
        base.DoPhysicsUpdate();
    }

    public override void Initialize(GameObject gameObject, Monster monster)
    {
        base.Initialize(gameObject, monster);
        monster.ReloadAttackToSkill(attacksToSkill);
    }

    private void Attack()
    {
        if (attacksToSkill > 0 || awaitToSpecialAttack || !monster.enemie)
        {
            Debug.Log("Attack");
            monster.BasicAttackDamage();
            monster.FillCircleSkillAttack();
            attacksToSkill -= 1;
            if (monster.enemie && attacksToSkill <= 0)
            {
                if (!awaitToSpecialAttack)
                {
                    awaitToSpecialAttack = true;
                    enemieRandomSpecialAttack = Random.Range(1, 2);
                }
                else
                {
                    enemieRandomSpecialAttack--;
                    if (enemieRandomSpecialAttack <= 0)
                    {
                        awaitToSpecialAttack = false;
                    }
                }
            }
        }
        else if (!awaitToSpecialAttack && monster.enemie)
        {
            monster.monsterStateMachine.ChangeState(monster.monsterSpecialAttackState);
        }

        if (monster.target.HealthFigth <= 0)
        {
            monster.target.monsterStateMachine.ChangeState(monster.target.monsterDeadState);
            monster.gameManager.CheckIfAnyAlive(monster.oppositeList);
            if (monster.oppositeList.Count != 0)
            {
                monster.target = monster.ChooseTarget(monster.oppositeList);
            }
            
        }
        attackTime = 1 * monster.speedAttack;
    }

    private void CheckDistance()
    {
        Vector3 closestPoint = monster.targetCollider.ClosestPoint(transform.position);
        if ((monster.transform.position - closestPoint).sqrMagnitude > monster.distanceAttack)
        {
            monster.monsterStateMachine.ChangeState(monster.monsterChaseState);
            Debug.Log("Cambia a chase");
            Debug.Log((monster.transform.position - closestPoint).sqrMagnitude);
        }
    }
}

