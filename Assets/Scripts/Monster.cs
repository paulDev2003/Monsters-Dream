using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Assertions;
using TMPro;

public class Monster : MonoBehaviour
{
    public MonsterSO monsterSO;
    public bool enemie;
    public float physicalDamage;
    public float speedAttack;
    public float defense;
    public float evasion;
    public float magicalDamage;
    public float magicalDefense;
    [Space(10)]
    public float distanceAttack;
    public float speedMovement;
    public float rotationSpeed = 5;
    private float enemieDistance;
    private float attackTime;
    private int attacksToSkill;
    [Space(10)]
    public float healthFigth = 1;
    public Monster target;
    public bool dead;

    private GameManager gameManager;
 [HideInInspector]public List<GameObject> oppositeList;
 [HideInInspector]public List<GameObject> ownList;
    private bool stopFigth = false;


    private void Start()
    {
        attacksToSkill = Random.Range(4, 7);
        physicalDamage = monsterSO.physicalDamage;
        speedAttack = monsterSO.speedAttack;
        defense = monsterSO.defense;
        evasion = monsterSO.evasion;
        magicalDamage = monsterSO.magicalDamage;
        magicalDefense = monsterSO.magicalDefense;
        gameManager = FindObjectOfType<GameManager>();
        Assert.IsNotNull(gameManager, "No encuentra gameManager");
        healthFigth = monsterSO.health;
        if (enemie)
        {
            oppositeList = gameManager.friendsList;
            ownList = gameManager.enemieList;
        }
        else
        {
            oppositeList = gameManager.enemieList;
            ownList = gameManager.friendsList;
        }
        target = ChooseTarget(oppositeList);
    }

    private void Update()
    {
        if (gameManager.finish)
        {
            stopFigth = true;
        }
        if (!dead && !stopFigth)
        {
            if (target != null)
            {
                enemieDistance = Vector3.Distance(transform.position, target.transform.position);
                RotateTowardsTarget();
            }
            if (attackTime > 0)
            {
                attackTime -= Time.deltaTime;
            }
            else
            {
                Attack();
            }

            if (healthFigth <= 0)
            {
                Debug.Log("Hola");
                healthFigth = 0;
                gameManager.RemoveFromList(ownList, this);
                dead = true;
            }
        }       
    }

    private Monster ChooseTarget(List<GameObject> listOpposite)
    {
        GameObject _target = null;
        float distance = float.MaxValue; 
        foreach (var enemie in listOpposite)
        {
            if (distance > Vector3.Distance(transform.position, enemie.transform.position))
            {
                distance = Vector3.Distance(transform.position, enemie.transform.position);
                _target = enemie;
            }   
        }
        Monster scriptTarget = null;
        if (_target != null)
        {
            scriptTarget = _target.GetComponent<Monster>();
        }  
        return scriptTarget;
    }

    private void Attack()
    {
        //attacking = true;
        if (distanceAttack < enemieDistance)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, speedMovement * Time.deltaTime);
        }
        else if(attackTime <= 0)
        {
            if (attacksToSkill > 0)
            {
                float damage = CalculateDamage();
                target.healthFigth -= damage;
                AttackScreenInfo(damage, target);
            }
            else
            {
                Debug.Log($"Lanza su ataque {monsterSO.name}");
                monsterSO.skill.ShootSkill(this);
                attacksToSkill = Random.Range(4, 7);
                attackTime = 1 / speedAttack;
            }
            attacksToSkill -= 1;
            if (target.healthFigth <= 0)
            {
                target.healthFigth = 0;
                gameManager.CheckIfAnyAlive(oppositeList);
                target = ChooseTarget(oppositeList);
            }
        }
    }

    private float CalculateDamage()
    {
        attackTime = 1 / speedAttack;
        float randomChance = Random.Range(0f, 100f);
        if (randomChance < evasion)
        {
            Debug.Log("¡El objetivo esquivó el ataque!");
            return 0; // No hay daño si esquiva
        }
        float damageDone = physicalDamage - target.defense;
        if (damageDone < 1)
        {
            return 1;
        }
        return damageDone;
    }

    private void RotateTowardsTarget()
    {
        if (target == null) return;

        Vector3 direction = (target.transform.position - transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    public void AttackScreenInfo(float damage, Monster monsterDamaged)
    {
        GameObject textInstanced = Instantiate(gameManager.textInfoPrefab, monsterDamaged.transform.position + Vector3.up * 2, Quaternion.identity, gameManager.canvasWorld.transform);
        TextMeshProUGUI textComponent = textInstanced.GetComponent<TextMeshProUGUI>();
        if (damage == 0)
        {
            textComponent.text = "Evade";
        }
        else
        {
            textComponent.text = $"-{damage}";
        }
        Destroy(textInstanced, 1.2f);
    }
    //Los monstruos tienen su skill (Scriptable Object?), sus atributos (variables)
    //La lógica de movimiento (en teoría en este script), hay momentos el que el monstruo no ataca pero va a estar en la escena
    //
}
