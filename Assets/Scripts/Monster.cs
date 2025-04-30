using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Assertions;
using TMPro;
using UnityEngine.AI;
using UnityEngine.UI;

public class Monster : MonoBehaviour
{
    public string monsterName = "name";
    public MonsterSO monsterSO;
    public MonsterData monsterData;
    public bool enemie;
    public int level = 1;
    public float health;
    public float physicalDamage;
    public float speedAttack;
    public float defense;
    public float evasion;
    public float magicalDamage;
    public float magicalDefense;
    public UILifeBar lifeBar;
    public bool wasSpawned = false;
    public enum typeDamage
    { 
        Physical,
        Magical
    };
    public typeDamage damageType;

    [Space(10)]
    public float distanceAttack;
    public float speedMovement;
    public float rotationSpeed = 5;
    public float regenerationTime = 3.5f;
    private float currentTimeRegeneration;
    private float enemieDistance;
    private float attackTime;
    private int attacksToSkill;
    private float totalAmountToSkill;
    [Space(10)]
    public float healthFigth;
    public Monster target;
    public bool dead;
    public int exp;
    public int maxExp;
    [Tooltip("Para saber si está invocado o no")]
    public bool inUse = false;
    

    private GameManager gameManager;
     public List<GameObject> oppositeList;
    [HideInInspector] public List<GameObject> ownList;
    private bool stopFigth = false;
    private MonsterClass monsterClass;
    private NavMeshAgent agent;
    public int valueI;
    public GameObject areaAttack;
    private Image circleAttacksToSkill;
    private GameObject objCircleAttacks;

    [Space(10)]
    public int damageBuff;
    public int basicDamageBuff;
    public int healthBuff;
    public int healthRegeneration;
    public int magicDamageBuff;
    public float decreasedCoolDown = 1;
    public float multiplierIncreasedSpeedAttack = 1;
    public int physicIncreaseDefense;
    public int magicIncreaseDefense;

    private void Start()
    {
        ReloadAttackToSkill();
        currentTimeRegeneration = regenerationTime;
        inUse = true;
        agent = FindObjectOfType<NavMeshAgent>();
        maxExp = level * 50;
        
        gameManager = FindAnyObjectByType<GameManager>();
        Assert.IsNotNull(gameManager, "No encuentra gameManager");
        if (enemie)
        {
            oppositeList = gameManager.friendsList;
            ownList = gameManager.enemieList;
        }
        else
        {
            oppositeList = gameManager.enemieList;
            ownList = gameManager.friendsList;
            gameManager.monsterDrop[valueI].monsterScript = this;
        }
        objCircleAttacks = Instantiate(gameManager.circleAttacksToSkill, transform.position + Vector3.up, Quaternion.identity, 
            gameManager.canvasWorld.transform);
        circleAttacksToSkill = objCircleAttacks.GetComponent<Image>();
        circleAttacksToSkill.fillAmount = 0;
        target = ChooseTarget(oppositeList);
        if (areaAttack != null)
        {
            Destroy(areaAttack);
        }
    }

    private void Update()
    {
        objCircleAttacks.transform.position = transform.position + Vector3.up * 1.5f;
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
            if (currentTimeRegeneration > 0)
            {
                currentTimeRegeneration -= Time.deltaTime;
            }
            else if(healthRegeneration > 0)
            {
                RegenerateHealth();
                currentTimeRegeneration = regenerationTime;
            }
            if (healthFigth <= 0)
            {
                Debug.Log("Hola");
                healthFigth = 0;
                gameManager.RemoveFromList(ownList, this);
                dead = true;
                if (!enemie)
                {
                    gameManager.countMonsters--; 
                }
            }
        }
    }

    public Monster ChooseTarget(List<GameObject> listOpposite)
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
        else if (attackTime <= 0)
        {
            if (attacksToSkill > 0)
            {
                float damage = CalculateDamage() + basicDamageBuff;
                target.healthFigth -= damage;
                AttackScreenInfo(damage, target);
                target.lifeBar.UpdateFill(target);
                circleAttacksToSkill.fillAmount += totalAmountToSkill;
                attacksToSkill -= 1;
            }
            else
            {
                ReloadAttackToSkill();
                circleAttacksToSkill.fillAmount = 0;
                attackTime = 1 / speedAttack;
            }
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
        float damageDone;
        if (damageType == typeDamage.Physical)
        {
            damageDone = physicalDamage - target.defense;
        }
        else
        {
            damageDone = magicalDamage - target.magicalDefense;
        }
        
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
        direction = new Vector3(direction.x, 0, direction.z);
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

    public void UpdateStats()
    {
        monsterClass = new MonsterClass(monsterSO, monsterData.level);
        level = monsterData.level;
        exp = monsterData.currentXP;
        health = monsterClass.Health + healthBuff;
        if (PlayerPrefs.GetInt("BattleNumber") == 0)
        {
            healthFigth = monsterClass.Health + healthBuff;
            Debug.Log("Entra en battleNumber 0");
        }
        else
        {
            healthFigth = monsterData.currentHealth;
            Debug.Log("Entra en con la vida previa");
        }
        physicalDamage = monsterClass.PhysicalDamage + damageBuff;
        speedAttack = monsterClass.SpeedAttack * multiplierIncreasedSpeedAttack;
        defense = monsterClass.Defense + physicIncreaseDefense;
        evasion = monsterClass.Evasion;
        magicalDamage = monsterClass.MagicalDamage + magicDamageBuff;
        magicalDefense = monsterClass.MagicalDefense + magicIncreaseDefense;
        if (monsterData.isStarter)
        {
            lifeBar.UpdateFill(this);
        }       
    }

    public void ChangeSelector()
    {
        if (!enemie)
        {
            gameManager.ChangeSelector(this.gameObject);
        }
    }

    public void ShowAreaDistanceAttack()
    {
        GameManager gameManagerNow = FindObjectOfType<GameManager>();
        float y = gameManagerNow.damageArea.transform.position.y;
        if (areaAttack != null)
        {
            DestroyImmediate(areaAttack);
        }
        Vector3 spawnArea = new Vector3(transform.position.x, y, transform.position.z);
        areaAttack = Instantiate(gameManagerNow.damageArea, spawnArea, Quaternion.identity);       
        areaAttack.transform.localScale = Vector3.one * distanceAttack * 0.25f;
    }
    public void CleanAreaDistanceAttack()
    {
        DestroyImmediate(areaAttack);
    }

    private void RegenerateHealth()
    {
        healthFigth += healthRegeneration;
        if (healthFigth > health)
        {
            healthFigth = health;
        }
        GameObject textInstanced = Instantiate(gameManager.textInfoPrefab, transform.position + Vector3.up * 2, Quaternion.identity, gameManager.canvasWorld.transform);
        TextMeshProUGUI textComponent = textInstanced.GetComponent<TextMeshProUGUI>();
        textComponent.text = $"+{healthRegeneration}";
        textComponent.color = Color.green;
        lifeBar.UpdateFill(this);
    }

    private void ReloadAttackToSkill()
    {
        attacksToSkill = Random.Range(4, 7);
        attacksToSkill = Mathf.RoundToInt(attacksToSkill / decreasedCoolDown);
        totalAmountToSkill = 1f / (float)attacksToSkill;
    }

    public void UpdateBar()
    {
        lifeBar.UpdateFill(this);
    }

    private void OnDisable()
    {
        if (objCircleAttacks != null)
        {
            objCircleAttacks.SetActive(false);
        }       
    }
    private void OnEnable()
    {
        if (objCircleAttacks != null)
        {
            objCircleAttacks.SetActive(true);
        }      
    }
    //Los monstruos tienen su skill (Scriptable Object?), sus atributos (variables)
    //La lógica de movimiento (en teoría en este script), hay momentos el que el monstruo no ataca pero va a estar en la escena
    //
}
